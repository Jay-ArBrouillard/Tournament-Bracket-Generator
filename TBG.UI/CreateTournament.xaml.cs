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
        private ITournamentController tournamentControl;
        public List<ITeam> teams;   //All existing teams
        public List<ITournamentEntry> teamsInTournament;    //Selected teams. Implements ITournamentEntry
        public List<IPrize> prizes; //All existing prizes
        public List<ITournamentPrize> prizesInTournament;  //Selected prizes
        public double prizePool;
        private IUser user;

        public CreateTournament(IUser user)
        {
            InitializeComponent();
            this.user = user;
            tournamentControl = ApplicationController.getTournamentController();
            source = ApplicationController.getProvider();
            tournamentTypesComboBox.ItemsSource = source.getTournamentTypes();
            teams = source.getAllTeams();
            teamsInTournament = new List<ITournamentEntry>();
            selectionListBox.ItemsSource = teams;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            prizesInTournament = new List<ITournamentPrize>();
            prizes = source.getAllPrizes();
            prizeComboBox.ItemsSource = prizes;
            prizePool = 0;
        }

        private void SetEntryFee_Click(object sender, RoutedEventArgs e)
        {
            string entryFeeInput = entryFeeTextBox.Text;
            bool validate = tournamentControl.validateEntryFee(entryFeeInput);

            if (validate)
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

        public List<ITournamentEntry> convertToTeam(List<ITeam> list)
        {
            List<ITournamentEntry> results = new List<ITournamentEntry>();

            foreach(ITeam team in list)
            {
                ObservableCollection<IPerson> teamMembers = new ObservableCollection<IPerson>();
                int teamId = team.TeamId;
                foreach (ITeamMember teamMember in source.getTeamMembersByTeamId(teamId))
                {
                    IPerson person = source.getPerson(teamMember.PersonId);
                    teamMembers.Add(new Person()
                    {
                        PersonId = person.PersonId,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Email = person.Email,
                        Phone = person.Phone,
                        Wins = person.Wins,
                        Losses = person.Losses
                    }); 
                }

                results.Add(new TournamentEntry() {
                    TeamId = teamId,
                    TeamName = team.TeamName,
                    Members = teamMembers,
                    Seed = 0 //change later
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

                if (isDuplicate == false)
                {
                    selectedTeams.Add(team);
                }
            }
            //Add selected teams to TreeView and Tournaments list
            teamsInTournament.AddRange(convertToTeam(selectedTeams));
            participantsTreeView.ItemsSource = teamsInTournament;
            participantsTreeView.Items.Refresh();

            //Update PrizePool
            SetEntryFee_Click(sender, e);
        }

        private void Create_New_Team_Click(object sender, RoutedEventArgs e)
        {
            TeamWindow teamWindow = new TeamWindow(this);
            teamWindow.Show();
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
                teamsInTournament[i].Seed = i + 1;
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
            bool validateEntryFee = tournamentControl.validateEntryFee(entryFeeTextBox.Text);
            bool validateTournamentTypeId = tournamentControl.validateTournamentType((ITournamentType)tournamentTypesComboBox.SelectedItem);

            if (!validateEntryFee)
            {
                errorMessages.Text = "Entry Fee must be an number (ex: 100 or 100.0)";
                entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (!validateTournamentTypeId)
            {
                errorMessages.Text = "Please select a Tournament Type";
                return;
            }

            errorMessages.Text = "";
            entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.DarkGray);

            ITournament tournament = null;
            if (!string.IsNullOrEmpty(entryFeeTextBox.Text) && !string.IsNullOrEmpty(totalPrizePool.Text))
            {
                tournament = new Tournament()
                {
                    TournamentName = tournamentNameTextBox.Text,
                    EntryFee = int.Parse(entryFeeTextBox.Text),
                    TournamentTypeId = ((ITournamentType)tournamentTypesComboBox.SelectedItem).TournamentTypeId,
                    TotalPrizePool = int.Parse(totalPrizePool.Text),
                    UserId = user.UserId,
                };
            }
            else
            {
                tournament = new Tournament()
                {
                    TournamentName = tournamentNameTextBox.Text,
                    TournamentTypeId = ((ITournamentType)tournamentTypesComboBox.SelectedItem).TournamentTypeId,
                    UserId = user.UserId,
                };
            }

            tournament.TournamentId = source.createTournament(tournament).TournamentId;    //Add a tournament to TournamentTable
            tournament.Participants = tournamentControl.ConvertITournmentEntries(teamsInTournament, tournament);  //Convert TournamentEntryView objects to ITournmentEntries
            ITournament newTournament = tournamentControl.createTournament(tournament);

            if (newTournament != null)
            {
                source.setupTournamentData(newTournament);
                TournamentViewUI viewUI = new TournamentViewUI(newTournament);
                viewUI.Show();
            }
            else //error
            {
                errorMessages.Text = "Must define tournament name and teams in order to continue";
                source.deleteTournament(tournament);
            }
        }
    }
}
