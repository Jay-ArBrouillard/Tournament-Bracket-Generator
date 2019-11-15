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
        private ITournamentController business;
        public List<ITeam> teams;   //All existing teams
        public List<TournamentEntryView> teamsInTournament;    //Selected teams. Implements ITournamentEntry
        public List<IPrize> prizes; //All existing prizes
        public List<IPrize> prizesInTournament;  //Selected prizes
        public double prizePool;
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
            bool validateTournamentTypeId = business.validateTournamentType((ITournamentType)tournamentTypesComboBox.SelectedItem);

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
                tournament = new SingleEliminationTournament()
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
                tournament = new SingleEliminationTournament()
                {
                    TournamentName = tournamentNameTextBox.Text,
                    TournamentTypeId = ((ITournamentType)tournamentTypesComboBox.SelectedItem).TournamentTypeId,
                    UserId = user.UserId,
                };
            }

            int tournamentId = source.createTournament(tournament).TournamentId;    //Add a tournament to TournamentTable
            tournament.TournamentId = tournamentId;
            //Convert TournamentEntryView objects to ITournmentEntries
            List<ITournamentEntry> tournamentEntries = ConvertTournamentEntryViewToITournmentEntries(teamsInTournament, tournamentId); //Convert to useable object
            tournament.Participants.AddRange(tournamentEntries);    //Add all converted participants to the tournament

            bool validate = business.validateSingleEliminationTournament(tournament);
            if (validate)
            {
                tournamentEntries.ForEach(entry => source.createTournamentEntry(entry));    

                //Create Backend Logic
                ITournament newTournament = business.createSingleEliminationTournament(tournament);

                if (newTournament != null)
                {
                    //Add the Starting Matchups and MatchupEntries to DB
                    initializeMatchups(newTournament);

                    TournamentViewUI viewUI = new TournamentViewUI(newTournament);
                    viewUI.Show();
                }
            }
            else //error
            {
                errorMessages.Text = "Must define tournament name and teams in order to continue";
                source.deleteTournament(tournament);
            }
            
        }

        private void initializeMatchups(ITournament newTournament)
        {
            var numberList = Enumerable.Range(0, newTournament.Participants.Count).ToList();

            for (int i = 0; i < newTournament.Rounds.Count; i++)
            {
                //Create a Round
                IRound round = source.createRound(new Round()
                {
                    RoundNum = i + 1,
                    TournamentId = newTournament.TournamentId
                });

                for (int j = 0; j < newTournament.Rounds[i].Pairings.Count; j+=2)
                {
                    //Create a Matchup and two MatchEntries
                    IMatchup matchup = source.createMatchup(new Matchup());
                    source.createRoundMatchup(new RoundMatchup()
                    {
                        MatchupId = matchup.MatchupId,
                        RoundId = round.RoundId,
                    });

                    IMatchupEntry matchupEntry1 = source.createMatchupEntry(new MatchupEntry()
                    {
                        MatchupId = matchup.MatchupId,
                        TournamentEntryId = newTournament.Participants[numberList[0]].TournamentEntryId,
                        Score = 0
                    });
                    IMatchupEntry matchupEntry2 = source.createMatchupEntry(new MatchupEntry()
                    {
                        MatchupId = matchup.MatchupId,
                        TournamentEntryId = newTournament.Participants[numberList[1]].TournamentEntryId,
                        Score = 0
                    });
                }

                numberList.RemoveAt(0);
                numberList.RemoveAt(0);
            }


        }

        private List<ITournamentEntry> ConvertTournamentEntryViewToITournmentEntries(List<TournamentEntryView> teamsInTournament, int tournamentId)
        {
            List<ITournamentEntry> results = new List<ITournamentEntry>();
            foreach (TournamentEntryView entry in teamsInTournament)
            {
                ITournamentEntry tournamentEntry = new TournamentEntry()
                {
                    TournamentId = tournamentId,
                    TeamId = entry.TeamId,
                    Seed = 0    //CHANGE later when seeding is implemented
                };
                results.Add(tournamentEntry);
            }

            return results;
        }
    }
}
