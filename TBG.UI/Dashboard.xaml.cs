using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Classes;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private IProvider source;
        private ITournamentController tournamentControl;
        private IUser user;

        public Dashboard(User user)
        {
            InitializeComponent();
            this.user = user;
            this.source = ApplicationController.getProvider();
            tournamentControl = ApplicationController.getTournamentController();

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
            ITournament selectedTournament = source.getTournamentByName(tournaments[tournamentList.SelectedIndex].TournamentName);

            ITournament newTournament = new Tournament()
            {
                TournamentId = selectedTournament.TournamentId,
                UserId = selectedTournament.UserId,
                TournamentName = selectedTournament.TournamentName,
                EntryFee = selectedTournament.EntryFee,
                TotalPrizePool = selectedTournament.TotalPrizePool,
                TournamentTypeId = selectedTournament.TournamentTypeId
            };
            newTournament.Participants = source.getTournamentEntriesByTournamentId(selectedTournament.TournamentId);
            newTournament = tournamentControl.createTournament(newTournament);
            List<IRound> existingRounds = source.getRoundsByTournamentId(newTournament.TournamentId);
            for (int i = 0; i < existingRounds.Count; i++)
            {
                List<IRoundMatchup> roundMatchups = source.getRoundMatchupsByRoundId(new RoundMatchup(existingRounds[i].RoundId));
                if (roundMatchups.Count > 0)
                {
                    for (int j = 0; j < roundMatchups.Count; j++)
                    {
                        List<IMatchupEntry> matchupEntries = source.getMatchupEntriesByMatchupId(roundMatchups[j].MatchupId);

                        if (i != 0)
                        {
                            IMatchup matchup = source.getMatchup(matchupEntries[j].MatchupId);
                            List<IMatchupEntry> matchEntries = source.getMatchupEntriesByMatchupId(matchup.MatchupId);
                            foreach (IMatchupEntry matchupEntry in matchEntries)
                            {
                                matchupEntry.TheTeam = source.getTournamentEntry(matchupEntry.TournamentEntryId);
                            }
                            newTournament.Rounds[i].Pairings[j].Teams = matchEntries;
                        }

                        newTournament.Rounds[i].Pairings[j].MatchupId = roundMatchups[j].MatchupId;

                        if (matchupEntries.Count == 1)
                        {
                            newTournament.Rounds[i].Pairings[j].Teams[0].Score = matchupEntries[0].Score;
                            newTournament.Rounds[i].Pairings[j].Teams[0].MatchupId = matchupEntries[0].MatchupId;
                            newTournament.Rounds[i].Pairings[j].Teams[0].TournamentEntryId = matchupEntries[0].TournamentEntryId;
                        }
                        else if (matchupEntries.Count == 2)
                        {
                            newTournament.Rounds[i].Pairings[j].Teams[0].Score = matchupEntries[0].Score;
                            newTournament.Rounds[i].Pairings[j].Teams[1].Score = matchupEntries[1].Score;
                            newTournament.Rounds[i].Pairings[j].Teams[0].MatchupId = matchupEntries[0].MatchupId;
                            newTournament.Rounds[i].Pairings[j].Teams[1].MatchupId = matchupEntries[1].MatchupId;
                            newTournament.Rounds[i].Pairings[j].Teams[0].TournamentEntryId = matchupEntries[0].TournamentEntryId;
                            newTournament.Rounds[i].Pairings[j].Teams[1].TournamentEntryId = matchupEntries[1].TournamentEntryId;
                        }
                    }
                }
            }

            TournamentViewUI viewUI = new TournamentViewUI(newTournament);
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