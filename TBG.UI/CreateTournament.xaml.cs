﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Classes;
using TBG.UI.Models;

namespace TBG.UI
{
    public partial class CreateTournament : Window
    {
        private IProvider source;
        private ITournamentController tournamentController;
        public List<ITeam> teams;   //All existing teams
        public List<ITournamentEntry> teamsInTournament;    //Selected teams. Implements ITournamentEntry
        public List<IPrize> prizes; //All existing prizes
        public List<IPrize> prizesInTournament;  //Selected prizes
        public double prizePool;
        private IUser user;
        private ITournament tournament;
        private List<ParticipantTree> theTeams = new List<ParticipantTree>();
        private HashSet<int> personIdsInTournament = new HashSet<int>();

        public CreateTournament(IUser user)
        {
            InitializeComponent();
            this.user = user;
            tournamentController = ApplicationController.getTournamentController();
            source = ApplicationController.getProvider();
            tournamentTypesComboBox.ItemsSource = source.getTournamentTypes();
            teams = source.getAllTeams();
            teamsInTournament = new List<ITournamentEntry>();
            selectionListBox.ItemsSource = teams;
            tournament = new Tournament();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            prizesInTournament = new List<IPrize>();
            prizes = source.getAllPrizes();
            prizes.ForEach(x => prizeDataGrid.Items.Add(x));
            prizePool = 0;
            SeedToggle_Unchecked(sender, e);
            Update_Place_Values();
        }

        private void SetEntryFee_Click(object sender, RoutedEventArgs e)
        {
            string entryFeeInput = entryFeeTextBox.Text;
            var validate = tournamentController.validateEntryFee(entryFeeInput);

            if (validate != -1)
            {
                if (int.TryParse(entryFeeInput, out int value))
                {
                    prizePool = teamsInTournament.Count * value;
                }
                if (double.TryParse(entryFeeInput, out double doubleValue))
                {
                    prizePool = (double)teamsInTournament.Count * doubleValue;
                }
                totalPrizePool.Text = prizePool.ToString();
                entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
                setPrizeAmounts();
            }
            else
            {
                entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void setPrizeAmounts()
        {
            foreach (IPrize prize in prizes)
            {
                decimal convertPercent = prize.PrizePercent > 1 ? prize.PrizePercent / 100 : prize.PrizePercent;
                prize.PrizeAmount = Math.Round(Int32.Parse(totalPrizePool.Text) * convertPercent, 2);
            }

            prizeDataGrid.Items.Refresh();
        }

        public List<ITournamentEntry> convertToEntries(List<ITeam> list)
        {
            List<ITournamentEntry> results = new List<ITournamentEntry>();

            foreach(ITeam team in list)
            {
                ObservableCollection<IPerson> teamMembers = new ObservableCollection<IPerson>();
                int teamId = team.TeamId;
                foreach (var person in team.TeamMembers)
                {
                    teamMembers.Add(person);
                }

                results.Add(new TournamentEntry() {
                    TeamId = teamId,
                    TeamName = team.TeamName,
                    Members = teamMembers
                });
            }

            return results;
        }

        private void AddSelectedTeam_Click(object sender, RoutedEventArgs e)
        {
            List<ITeam> selectedTeams = new List<ITeam>();
            foreach (ITeam team in selectionListBox.SelectedItems)
            {
                bool isDuplicateTeam = teamsInTournament.Where(t => t.TeamId == team.TeamId).Any();
                bool hasDuplicatePlayer = team.TeamMembers.Where(t => personIdsInTournament.Contains(t.PersonId)).Any();
                if (!isDuplicateTeam && !hasDuplicatePlayer)
                {
                    selectedTeams.Add(team);
                    errorMessages.Text = "";
                }
                else if (hasDuplicatePlayer)
                {
                    errorMessages.Text = "One of players you tried adding is already in tournament";
                }
            }
            tournament.Teams.AddRange(selectedTeams);
            //Add selected teams to TreeView and Tournaments list
            var converted = convertToEntries(selectedTeams);
            teamsInTournament.AddRange(converted);

            foreach (var team in converted)
            {
                var newTeam = new ParticipantTree()
                {
                    TeamId = team.TeamId,
                    Seed = team.Seed,
                    TeamName = selectedTeams.Find(x => x.TeamId == team.TeamId).TeamName
                };

                foreach (var member in team.Members)
                {
                    newTeam.Members.Add(new Person()
                    {
                        FirstName = member.FirstName,
                        LastName = member.LastName
                    });
                    personIdsInTournament.Add(member.PersonId);
                }

                theTeams.Add(newTeam);
            }
            participantsTreeView.ItemsSource = theTeams;
            participantsTreeView.Items.Refresh();

            //Update place values
            Update_Place_Values();

            //Update PrizePool
            SetEntryFee_Click(sender, e);

            setPrizeAmounts();
        }

        private void Update_Place_Values()
        {
            placeComboBox.Items.Clear();
            for (int i = 1; i <= teamsInTournament.Count; i++)
            {
                placeComboBox.Items.Add(i);
            }
        }

        private void Create_New_Team_Click(object sender, RoutedEventArgs e)
        {
            TeamWindow teamWindow = new TeamWindow(user);
            teamWindow.Show();
            this.Close();
        }

        private void Create_New_Prize_Click(object sender, RoutedEventArgs e)
        {
            PrizeUI prizes = new PrizeUI(this);
            prizes.Show();
        }

        private void DeleteTeamButton_Click(object sender, RoutedEventArgs e)
        {
            if (participantsTreeView.SelectedItem is ParticipantTree selectedTeamItem)
            {
                var theTeam = teamsInTournament.Find(x => x.TeamId == selectedTeamItem.TeamId);
                teamsInTournament.Remove(theTeam);
                theTeams.Remove(selectedTeamItem);
                participantsTreeView.Items.Refresh();

                //Update PrizePool
                SetEntryFee_Click(sender, e);
                //Update PrizeAmounts
                setPrizeAmounts();
            }
            Update_Place_Values();

            //Remove prizes for places > new count
            foreach (var prize in prizesInTournament.ToList())
            {
                if (prize.PlaceNumber > teamsInTournament.Count)
                {
                    prizesInTournament.Remove(prize);
                }
            }

            prizesListBox.Items.Refresh();
        }

        private void DeletePrizeButton_Click(object sender, RoutedEventArgs e)
        {
            List<IPrize> remove = new List<IPrize>();
            foreach (var prize in prizesListBox.SelectedItems)
            {
                remove.Add((IPrize)prize);
            }

            foreach (IPrize p in remove)
            {
                prizesInTournament.Remove(p);
            }
            prizesListBox.Items.Refresh();
        }

        private void PrizeComboBox_Selected(object sender, RoutedEventArgs e)
        {
            
        }

        private void SeedToggle_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < teamsInTournament.Count; i++)
            {
                var team = teams.Where(x => x.TeamId == teamsInTournament[i].TeamId).First();
                double seed = Math.Round(calculateWinPercentage(team.Wins, team.Losses), 3);
                teamsInTournament[i].Seed = seed;
                theTeams[i].Seed = seed;
            }

            participantsTreeView.Items.Refresh();
        }

        private void SeedToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < teamsInTournament.Count; i++)
            {
                theTeams[i].Seed = 0;
            }

            participantsTreeView.Items.Refresh();
        }

        private void Create_Tournament_Click(object sender, RoutedEventArgs e)
        {
            var tournamentName = tournamentNameTextBox.Text;
            var entryFee = entryFeeTextBox.Text;
            var TournamentType = (ITournamentType)tournamentTypesComboBox.SelectedItem;
            var prizePool = totalPrizePool.Text;
            var numParticipants = teamsInTournament.Count;

            var validTournamentName = tournamentController.validateTournamentName(tournamentName);
            var validEntryFee = tournamentController.validateEntryFee(entryFee);
            var validTournamentTypeId = tournamentController.validateTournamentType(TournamentType);
            var validParticipantCount = tournamentController.validateParticipantCount(numParticipants, TournamentType);
            var validTotalPrizePool = tournamentController.validateTotalPrizePool(prizePool, numParticipants, validEntryFee);
            var validatedPrizes = tournamentController.validatePrizes(prizesInTournament);
            //Add prize structure to create tournament call

            if (!validTournamentName)
            {
                errorMessages.Text = "Please enter a tournament name";
                return;
            }           

            if (validEntryFee == -1)
            {
                errorMessages.Text = "Entry Fee must be an number (ex: 100 or 100.0)";
                entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (!validTournamentTypeId)
            {
                errorMessages.Text = "Please select a Tournament Type";
                return;
            }

            if (!validParticipantCount)
            {
                errorMessages.Text = "Please add a valid amount of teams to the tournament";
                return;
            }

            if (validTotalPrizePool == -1)
            {
                errorMessages.Text = "Prize pool is not valid";
                return;
            }

            if (validatedPrizes == null)
            {
                errorMessages.Text = "Prizes are not valid";
                return;
            }

            errorMessages.Text = "";
            entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.DarkGray);

            ITournament newTournament = tournamentController.createTournament(tournamentName, 
                TournamentType, 
                user.UserId, 
                validEntryFee, 
                validTotalPrizePool, 
                teamsInTournament,
                tournament.Teams,
                validatedPrizes);

            newTournament.Teams.AddRange(tournament.Teams);

            if (newTournament != null)
            {
                newTournament = source.createTournament(newTournament); //Create Entire Tournament, Set IDs Inside
                TournamentViewUI viewUI = new TournamentViewUI(newTournament);
                viewUI.Show();
                this.Close();
            }
            else //error
            {
                errorMessages.Text = "Must define tournament name and teams in order to continue";
            }
        }

        private double calculateWinPercentage(int wins, int losses)
        {
            if (wins + losses == 0)
            {
                return 1;
            }

            return (double) wins /  (wins + losses);
        }

        private void Add_Prize_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem = prizeDataGrid.SelectedItem;
            IPrize selectedPrize = (IPrize)selectedItem;

            if (placeComboBox.SelectedItem == null)
            {
                errorMessages.Text = "Must select a place";
                return;
            }

            var selectedPlace = placeComboBox.SelectedItem;
            int selectedPlaceNumber = (int)selectedPlace;

            decimal percentTotal = prizesInTournament.Sum(x => x.PrizePercent);

            //validate that prize doesn't already exist
            errorMessages.Text = "";
            if (prizesInTournament.Any(x => x.PlaceNumber == selectedPlaceNumber))
            {
                errorMessages.Text = "There is already a prize for the selected place.";
                return;
            }

            if (percentTotal + selectedPrize.PrizePercent > 1)
            {
                errorMessages.Text = "Total prize percent exceeds 100%";
                return;
            }

            prizesInTournament.Add(new Prize()
            {
                PrizeId = selectedPrize.PrizeId,
                PrizeName = selectedPrize.PrizeName,
                PrizeAmount = selectedPrize.PrizeAmount,
                PrizePercent = selectedPrize.PrizePercent,
                PlaceNumber = selectedPlaceNumber
            });
            prizesListBox.ItemsSource = prizesInTournament;
            prizesListBox.Items.Refresh();
        }
    }
}
