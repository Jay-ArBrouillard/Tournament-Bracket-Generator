using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TBG.Core.Interfaces;
using TBG.UI.Models;

namespace TBG.UI
{
    public partial class ResultWindow : Window
    {
        private ITournament tournament;
        private List<KeyValuePair<int, int>> placings;

        public ResultWindow(ITournament tournament)
        {
            InitializeComponent();
            this.tournament = tournament;

            foreach (var round in tournament.Rounds)
            {
                roundSelectComboBox.Items.Add("Round " + round.RoundNum);
            }

            calculatePlacing();
            populateDataGrid(tournament.Rounds[0].Matchups);
        }

        private void calculatePlacing()
        {/*
            placings = new List<KeyValuePair<int, int>>();
            foreach (var matchups in tournament.Rounds[0].Matchups)
            {
                foreach (var matchup in matchups.MatchupEntries)
                {
                    placings.Add(new KeyValuePair<int, int>(matchup.TheTeam.TeamId, matchup.TheTeam.Wins));
                }
            }

            placings.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));*/
            ReseedTournament();
            tournament.TournamentEntries.OrderByDescending(x => x.Seed).ToList();
        }

        private void populateDataGrid(List<IRound> rounds)
        {
            foreach (var round in rounds)
            {
                populateDataGrid(round.Matchups);
            }
        }

        private void populateDataGrid(List<IMatchup> matchups)
        {
            foreach (var matchup in matchups)
            {
                for (int i = 0; i < matchup.MatchupEntries.Count; i++) //var matchupEntry in matchup.MatchupEntries)
                {
                    ResultItem item = new ResultItem();
                    StringBuilder playerBuilder = new StringBuilder();
                    int numTeamMembers = matchup.MatchupEntries[i].TheTeam.Members.Count;
                    for (int j = 0; j < numTeamMembers; j++) //var player in matchup.TheTeam.Members)
                    {
                        IPerson player = matchup.MatchupEntries[i].TheTeam.Members[j];
                        playerBuilder.Append(player.FirstName).Append(" ").Append(player.LastName);
                        if (j != numTeamMembers - 1)
                        {
                            playerBuilder.Append(" + ");
                        }
                    }
                    ITeam thisTeam = tournament.Teams.Find(x => x.TeamId == matchup.MatchupEntries[i].TheTeam.TeamId);
                    item.Players = playerBuilder.ToString();
                    item.TeamName = thisTeam.TeamName;
                    /*int placeValue = placings.Find(x => x.Key == thisTeam.TeamId).Value;
                    if (placeValue != 0)
                    {
                        item.Placing = placeValue;
                    }*/
                    item.Placing = matchup.MatchupEntries.Seed //Here
                    item.Wins = matchup.MatchupEntries[i].TheTeam.Wins;
                    item.Losses = matchup.MatchupEntries[i].TheTeam.Losses;
                    item.WinLoss = Math.Round(calculateWinPercentage(item.Wins, item.Losses), 3);
                    item.CareerWins = thisTeam.Wins;
                    item.CareerLosses = thisTeam.Losses;
                    item.CareerWinLoss = Math.Round(calculateWinPercentage(item.CareerWins, item.CareerLosses), 3);

                    resultDataGrid.Items.Add(item);
                }
            }
        }

        private double calculateWinPercentage(int wins, int losses)
        {
            if (wins + losses == 0)
            {
                return 1;
            }

            return (double)wins / (wins + losses);
        }

        private void RoundSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedRound = roundSelectComboBox.SelectedIndex;
            resultDataGrid.Items.Clear();
            if (selectedRound == -1)
            {
                return;
            }
            else if (selectedRound == tournament.Rounds.Count)
            {
                populateDataGrid(tournament.Rounds);
            }
            else
            {
                populateDataGrid(tournament.Rounds[selectedRound].Matchups);
            }
        }

        private void ReseedTournament()
        {
            var matchups = tournament.Rounds.SelectMany(x => x.Matchups).ToList();
            foreach (var entry in tournament.TournamentEntries)
            {
                var wins = 0;
                var losses = 0;
                var entryMatchupEntries = matchups.SelectMany(y => y.MatchupEntries).Where(z => z.TheTeam.TeamId == entry.TeamId);
                foreach (var matchupEntry in entryMatchupEntries)
                {
                    var matchup = matchups.Where(x => x.MatchupId == matchupEntry.MatchupId).First();
                    var entryScore = matchup.MatchupEntries.Where(x => x.TheTeam.TeamId == entry.TeamId).First().Score;
                    var opponentScore = matchup.MatchupEntries.Where(x => x.TheTeam.TeamId != entry.TeamId).First().Score;
                    if (entryScore > opponentScore) { wins++; }
                    else { losses++; }
                }
                if (losses == 0) { entry.Seed = 1; }
                else { entry.Seed = wins / (wins + losses); }
            }
        }


    }
}
