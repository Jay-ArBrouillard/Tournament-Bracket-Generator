using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ITournamentController business;
        public List<ITeam> teams;   //All existing teams
        public List<TournamentEntryView> teamsInTournament;    //Selected teams. Implements ITournamentEntry
        public List<IPrize> prizes; //All existing prizes
        public List<IPrize> prizesInTournament;  //Selected prizes
        public int prizePool;
        private IUser user;

        public CreateTournament(IUser user)
        {
            InitializeComponent();
            this.user = user;
            business = ApplicationController.getTournamentController();
            source = ApplicationController.getProvider();
            tournamentTypesComboBox.ItemsSource = source.getTournamentTypes();
            teams = source.getAllTeams();
            teamsInTournament = new List<TournamentEntryView>();
            selectionListBox.ItemsSource = teams;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            prizesInTournament = new List<IPrize>();
            prizes = source.getAllPrizes();
            prizeComboBox.ItemsSource = prizes;
            prizePool = 0;
        }

        private void SetEntryFee_Click(object sender, RoutedEventArgs e)
        {
            string entryFeeInput = entryFeeTextBox.Text;
            bool validate = business.validateEntryFee(entryFeeInput);

            if (validate)
            {
                prizePool = teamsInTournament.Count * int.Parse(entryFeeInput);
                totalPrizePool.Text = prizePool.ToString();
                entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                entryFeeTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        public List<TournamentEntryView> convertToTeam(List<ITeam> list)
        {
            List<TournamentEntryView> result = new List<TournamentEntryView>();

            foreach(ITeam team in list)
            {
                ObservableCollection<TeamMember> teamMembers = new ObservableCollection<TeamMember>();
                int teamId = team.TeamId;
                foreach (ITeamMember teamMember in source.getTeamMembersByTeamId(teamId))
                {
                    IPerson person = source.getPerson(teamMember.PersonId);
                    teamMembers.Add(new TeamMember()
                    {
                        PersonId = person.PersonId,
                        TeamName = team.TeamName,
                        FirstName = person.FirstName,
                        LastName = person.LastName
                    }); 
                }

                result.Add(new TournamentEntryView() {
                    TeamId = teamId,
                    TeamName = team.TeamName,
                    Members = teamMembers
                });
            }

            return result;
        }

        private void AddSelectedTeam_Click(object sender, RoutedEventArgs e)
        {
            List<ITeam> selectedTeams = new List<ITeam>();
            foreach (ITeam team in selectionListBox.SelectedItems)
            {
                bool duplicateTeam = false;
                string teamName = team.TeamName;
                foreach (TournamentEntryView view in teamsInTournament)
                {
                    if (view.TeamName.Equals(teamName))
                    {
                        duplicateTeam = true;
                        break;
                    }
                }

                if (duplicateTeam == false)
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
            if (participantsTreeView.SelectedItem is TournamentEntryView selectedTeamItem)
            {
                teamsInTournament.Remove(selectedTeamItem);
                participantsTreeView.Items.Refresh();

                //Update PrizePool
                SetEntryFee_Click(sender, e);
            }
            else if (participantsTreeView.SelectedItem is TeamMember selectedMemberItem)
            {
                //Iterate each team in the tournament
                foreach (TournamentEntryView view in teamsInTournament)
                {
                    if (!selectedMemberItem.TeamName.Equals(view.TeamName))
                    {
                        continue;   //Saves unnecessary iterations
                    }

                    //Iterate each teamMember and check if their Id matches
                    foreach (TeamMember member in view.Members)
                    {
                        if (member.PersonId == selectedMemberItem.PersonId)
                        {
                            view.Members.Remove(member);
                            break;
                        }
                    }
                }
            }

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
            object selectedItem = ((ComboBox)sender).SelectedItem;
            IPrize selectedPrize = (IPrize)selectedItem;
            prizesInTournament.Add(selectedPrize);
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
            bool validateEntryFee = business.validateEntryFee(entryFeeTextBox.Text);
            ITournamentType tournamentType = (ITournamentType)tournamentTypesComboBox.SelectedItem;
            bool validateTournamentTypeId = business.validateTournamentType(tournamentType.TournamentTypeName);
            bool validateTotalPrizePool = business.validateTotalPrizePool(totalPrizePool.Text);

            if (!validateEntryFee)
            {
                errorMessages.Text = "Entry Fee must be an Integer";
                return;
            }

            if (!validateTournamentTypeId)
            {
                errorMessages.Text = "Please select a Tournament Type";
                return;
            }

            if (validateEntryFee && !validateTotalPrizePool)
            {
                errorMessages.Text = "Set the entry fee before proceeding or remove entry fee entirely";
                return;
            }

            //TODO: FIX VALIDATION AND THEN CREATE MATCHUPS AND MATCHUP ENTRIES IN TOURNAMENT VIEWER UI

            errorMessages.Text = "";

            ITournament tournament = new SingleEliminationTournament()
            {
                TournamentName = tournamentNameTextBox.Text,
                EntryFee = int.Parse(entryFeeTextBox.Text),
                TournamentTypeId = tournamentType.TournamentTypeId,
                TotalPrizePool = int.Parse(totalPrizePool.Text), 
                UserId = user.UserId,
            };

            bool validate = business.validateSingleEliminationTournament(tournament);

            if (!validate)
            {
                return;
            }

            int tournamentId = source.createTournament(tournament).TournamentId;
            tournament.TournamentId = tournamentId;

            //Convert TournamentEntryView objects to ITournmentEntries
            foreach (TournamentEntryView entry in teamsInTournament)
            {
                ITournamentEntry tournamentEntry = new TournamentEntry()
                {
                    TournamentId = tournamentId,
                    TeamId = entry.TeamId,
                    Seed = 0    //CHANGE later
                };

                tournamentEntry = source.createTournamentEntry(tournamentEntry);
                tournament.Participants.Add(tournamentEntry);
            }

            //Create Backend Logic
            ITournament newTournament = business.createSingleEliminationTournament(tournament);

            //Add the Starting Matchups and MatchupEntries to DB
            int numberOfMatchups = (int)Math.Log(newTournament.Participants.Count, 2);
            for (int i = 0; i < numberOfMatchups; i++) 
            {
                //Create a Matchup and two MatchEntries
                IMatchup matchup = source.createMatchup(new Matchup());

                const int numberOfTeamsPerMatchup = 2;
                for (int j = 0; j < numberOfTeamsPerMatchup; j+=2)
                {
                    IMatchupEntry matchupEntry1 = source.createMatchupEntry(new MatchupEntry()
                    {
                        MatchupId = matchup.MatchupId,
                        TournamentEntryId = newTournament.Participants[j].TournamentEntryId,
                        Score = 0
                    });
                    IMatchupEntry matchupEntry2 = source.createMatchupEntry(new MatchupEntry()
                    {
                        MatchupId = matchup.MatchupId,
                        TournamentEntryId = newTournament.Participants[j + 1].TournamentEntryId,
                        Score = 0
                    });
                }
            }

            if (newTournament != null)
            {
                TournamentViewUI viewUI = new TournamentViewUI(newTournament);
                viewUI.Show();
            }
            
        }

    }
}
