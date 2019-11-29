using System;
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
        public List<ITournamentPrize> prizesInTournament;  //Selected prizes
        public double prizePool;
        private IUser user;
        private ITournament tournament;

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
            prizesInTournament = new List<ITournamentPrize>();
            prizes = source.getAllPrizes();
            prizeComboBox.ItemsSource = prizes;
            prizePool = 0;
            SeedToggle_Unchecked(sender, e);
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
            }
            else
            {
                entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
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
                bool isDuplicate = teamsInTournament.Where(t => t.TeamId == team.TeamId).Any();

                if (!isDuplicate)
                {
                    selectedTeams.Add(team);
                }
            }
            tournament.Teams.AddRange(selectedTeams);
            //Add selected teams to TreeView and Tournaments list
            teamsInTournament.AddRange(convertToEntries(selectedTeams));
            participantsTreeView.ItemsSource = teamsInTournament;
            participantsTreeView.Items.Refresh();

            //Update PrizePool
            SetEntryFee_Click(sender, e);
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
            if (participantsTreeView.SelectedItem is ITournamentEntry selectedTeamItem)
            {
                teamsInTournament.Remove(selectedTeamItem);
                participantsTreeView.Items.Refresh();

                //Update PrizePool
                SetEntryFee_Click(sender, e);
            }
        }

        private void DeletePrizeButton_Click(object sender, RoutedEventArgs e)
        {
            List<ITournamentPrize> remove = new List<ITournamentPrize>();
            foreach (var prize in prizesListBox.SelectedItems)
            {
                remove.Add((ITournamentPrize)prize);
            }

            foreach (ITournamentPrize p in remove)
            {
                prizesInTournament.Remove(p);
            }

            prizesListBox.Items.Refresh();
        }

        private void PrizeComboBox_Selected(object sender, RoutedEventArgs e)
        {
            object selectedItem = ((ComboBox)sender).SelectedItem;
            IPrize selectedPrize = (IPrize)selectedItem;
            prizesInTournament.Add(new TournamentPrize()
            {
                PrizeId = selectedPrize.PrizeId,
            });
            prizesListBox.ItemsSource = prizesInTournament;
            prizesListBox.Items.Refresh();
        }

        private void SeedToggle_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < teamsInTournament.Count; i++)
            {
                var team = teams.Where(x => x.TeamId == teamsInTournament[i].TeamId).First();
                teamsInTournament[i].Seed = Math.Round(calculateWinPercentage(team.Wins, team.Losses), 3);
            }

            participantsTreeView.Items.Refresh();
        }

        private void SeedToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < teamsInTournament.Count; i++)
            {
                teamsInTournament[i].Seed = 0;
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
            var validParticipantCount = tournamentController.validateParticipantCount(numParticipants);
            var validTotalPrizePool = tournamentController.validateTotalPrizePool(prizePool, numParticipants, validEntryFee);

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

            errorMessages.Text = "";
            entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.DarkGray);

            ITournament newTournament = tournamentController.createTournament(tournamentName, 
                TournamentType, 
                user.UserId, 
                validEntryFee, 
                validTotalPrizePool, 
                teamsInTournament);

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
    }
}
