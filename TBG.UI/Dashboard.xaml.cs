using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private IProvider source;
        private ITournamentController business;
        private IUser user;

        public Dashboard(User user)
        {
            InitializeComponent();
            this.user = user;
            this.source = ApplicationController.getProvider();
            business = ApplicationController.getTournamentController();

            var tournaments = source.getAllTournaments();
            var tournamentTypes = source.getTournamentTypes();

            foreach (var tournament in tournaments)
            {
                var item = new TournamentListBoxItem(tournament.TournamentName);
                this.tournamentList.Items.Add(new ListBoxItem {
                    Content = item.Name
                });
            }

            foreach (var tournamentType in tournamentTypes)
            {
                this.typeFilter.Items.Add(new ListBoxItem {
                    Content = new Label {
                        Content = tournamentType.TournamentTypeName
                    }
                });
            }

            //Logged in as guest -> don't allow them to create a tournament
            if (user == null)   
            {
                createTournamentButton.Visibility = Visibility.Hidden;
            }
        }

        public void Load_Tournament(object sender, RoutedEventArgs e)
        {
            //Only load tournament if one is selected
            if (tournamentList.SelectedIndex == -1) return;

            //Look at selected item in tournamentList, pass that id to Tournament
            List<ITournament> tournaments = source.getAllTournaments();
            int selectedTourney = tournamentList.SelectedIndex;
            ITournament selectedTournament = source.getTournamentByName(tournaments[selectedTourney].TournamentName);
            int tournamentId = tournaments[selectedTourney].TournamentId;
            List<ITournamentEntry> entries = source.getTournamentEntriesByTournamentId(tournamentId);
            selectedTournament.Participants = entries;

            selectedTournament = business.createSingleEliminationTournament(selectedTournament);
            for (int i = 0; i < selectedTournament.Rounds.Count; i++) 
            {
                //Get the RoundId for the current round
                IRound currRound = source.getRoundByTournamentIdandRoundNum(selectedTournament.Rounds[i]);
                selectedTournament.Rounds[i].RoundId = currRound.RoundId;

                //Get every RoundMatchup for each round
                List <IRoundMatchup> roundMatchups = source.getRoundMatchupsByRoundId(currRound);
                for (int j = 0; j < roundMatchups.Count; j++)
                {
                    //Look up Score in MatchupEntries by MatchupId
                    List<IMatchupEntry> matchupEntries = source.getMatchupEntriesByMatchupId(roundMatchups[j].MatchupId);
                    for (int k = 0; k < matchupEntries.Count; k++)
                    {
                        if (selectedTournament.Rounds[i].Pairings[j].Teams.Count > 0)
                        {
                            selectedTournament.Rounds[i].Pairings[j].Teams[k % 2].Score = matchupEntries[k].Score;
                            selectedTournament.Rounds[i].Pairings[j].Teams[k % 2].MatchupEntryId = matchupEntries[k].MatchupEntryId;
                            selectedTournament.Rounds[i].Pairings[j].Teams[k % 2].MatchupId = matchupEntries[k].MatchupId;
                            selectedTournament.Rounds[i].Pairings[j].Teams[k % 2].TournamentEntryId = matchupEntries[k].TournamentEntryId;
                            selectedTournament.Rounds[i].Pairings[j].Teams[(k + 1) % 2].Score = matchupEntries[(k + 1) % 2].Score;
                            selectedTournament.Rounds[i].Pairings[j].Teams[(k + 1) % 2].MatchupEntryId = matchupEntries[(k + 1) % 2].MatchupEntryId;
                            selectedTournament.Rounds[i].Pairings[j].Teams[(k + 1) % 2].MatchupId = matchupEntries[(k + 1) % 2].MatchupId;
                            selectedTournament.Rounds[i].Pairings[j].Teams[(k + 1) % 2].TournamentEntryId = matchupEntries[(k + 1) % 2].TournamentEntryId;
                        }
                    }
                }
            }

            
            TournamentViewUI viewUI = new TournamentViewUI(selectedTournament);
            viewUI.Show();
        }

        private void CreateTournament_Click(object sender, RoutedEventArgs e)
        {
            CreateTournament newTournament = new CreateTournament(user);
            newTournament.Show();
            this.Close();
        }
    }
}