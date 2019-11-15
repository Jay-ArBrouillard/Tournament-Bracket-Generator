using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Classes;
using TBG.UI.Models;

namespace TBG.UI
{

    public partial class TournamentViewUI : Window
    {
        private IProvider source;
        private ITournament tournament;
        private Boolean initialization = true;

        public TournamentViewUI(ITournament inTourney)
        {
            InitializeComponent();
            source = ApplicationController.getProvider();
            tournament = inTourney;

            //Add all the rounds to the Rounds view
            for (int i = 1; i <= tournament.Rounds.Count; i++)
            {
                roundDropDown.Items.Add(i);
                /*source.createRound(new Round()
                {
                    TournamentId = tournament.TournamentId,
                    RoundNum = i,
                    Pairings = tournament.Rounds[i].Pairings
                });*/
            }

            //Set tournament Name
            tournamentNameLbl.Content = tournament.TournamentName;

            roundDropDown.SelectedItem = null;
            roundDropDown.SelectedIndex = 0;
            displayRound1();
        }
        private void displayRound1()
        {
            int selectedRound = roundDropDown.SelectedIndex;

            if (selectedRound == -1) return; 

            foreach (IMatchup matchup in tournament.Rounds[selectedRound].Pairings)
            {
                IMatchupEntry team1 = matchup.Teams[0];
                IMatchupEntry team2 = matchup.Teams[1];

                //IMatchupEntry has a ITournamentEntry which has a teamId
                //Will call database to get their teamName
                String team1Name = source.getTeam(team1.TheTeam.TeamId).TeamName;
                String team2Name = source.getTeam(team2.TheTeam.TeamId).TeamName;

                TournamentViewListBoxItem matchupsItem = new TournamentViewListBoxItem()
                {
                    Team1Name = team1Name,
                    Team2Name = team2Name,
                    imageURL = "Assets/x-button.png"
                };

                matchupsListBox.Items.Add(matchupsItem);

                //Add the matchups to roundMatchups
            }

            initialization = false;
        }

        private void MatchupsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int selectedRound = roundDropDown.SelectedIndex;
            int selectedPairing = matchupsListBox.SelectedIndex;

            if (selectedPairing == -1 || selectedRound == -1) return;   //For some reason this method is getting called when changing the round so we need this check

            List<IMatchupEntry> teams = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams;

            IMatchupEntry team1 = teams[0];
            IMatchupEntry team2 = teams[1];

            ITeam theTeam1 = source.getTeam(team1.TheTeam.TeamId);
            ITeam theTeam2 = source.getTeam(team2.TheTeam.TeamId);

            List<ITeamMember> team1Ids = source.getTeamMembersByTeamId(team1.TheTeam.TeamId);
            List<ITeamMember> team2Ids = source.getTeamMembersByTeamId(team2.TheTeam.TeamId);

            List<IPerson> team1TeamMembers = new List<IPerson>();
            List<IPerson> team2TeamMembers = new List<IPerson>();

            foreach (ITeamMember m in team1Ids)
            {
                team1TeamMembers.Add(source.getPerson(m.PersonId));
            }

            foreach (ITeamMember m in team2Ids)
            {
                team2TeamMembers.Add(source.getPerson(m.PersonId));
            }

            theTeam1.TeamMembers = team1TeamMembers;
            theTeam2.TeamMembers = team2TeamMembers;

            teamOneDataGrid.Items.Clear();
            foreach (IPerson p in theTeam1.TeamMembers)
            {
                TournamentViewDataGridItem dataGridItem = new TournamentViewDataGridItem()
                {
                    PlayerName = p.FirstName + " " + p.LastName,
                    Wins = p.Wins,
                    Losses = p.Losses,
                    TeamId = team1.TheTeam.TeamId
                };

                teamOneDataGrid.Items.Add(dataGridItem);
            }

            teamTwoDataGrid.Items.Clear();
            foreach (IPerson p in theTeam2.TeamMembers)
            {
                TournamentViewDataGridItem dataGridItem = new TournamentViewDataGridItem()
                {
                    PlayerName = p.FirstName + " " + p.LastName,
                    Wins = p.Wins,
                    Losses = p.Losses,
                    TeamId = team2.TheTeam.TeamId
                };

                teamTwoDataGrid.Items.Add(dataGridItem);
            }

            firstTeamLabel.Content = theTeam1.TeamName;
            secondTeamLabel.Content = theTeam2.TeamName;
            //Score
            firstTeamScoreTxtBox.Text = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].Score.ToString();
            secondTeamScoreTxtBox.Text = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].Score.ToString();
        }

        private void RoundDropDown_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (initialization == true) return; //Don't do anything

            int selectedRound = roundDropDown.SelectedIndex;
            matchupsListBox.SelectedItem = null;
            matchupsListBox.Items.Clear();
            teamTwoDataGrid.Items.Clear();
            teamOneDataGrid.Items.Clear();

            populateMatchupListBox(selectedRound);
        }

        private void populateMatchupListBox(int selectedRound)
        {
            IRound thisRound = tournament.Rounds[selectedRound];

            foreach (IMatchup m in thisRound.Pairings)
            {
                if (m.Teams != null && m.Teams.Count != 2) continue;

                IMatchupEntry team1 = m.Teams[0];
                IMatchupEntry team2 = m.Teams[1];

                //IMatchupEntry has a ITournamentEntry which has a teamId
                //Will call database to get their teamName
                String team1Name = source.getTeam(team1.TheTeam.TeamId).TeamName;
                String team2Name = source.getTeam(team2.TheTeam.TeamId).TeamName;

                TournamentViewListBoxItem matchupsItem = new TournamentViewListBoxItem()
                {
                    Team1Name = team1Name,
                    Team2Name = team2Name,
                    imageURL = "Assets/x-button.png"
                };

                matchupsListBox.Items.Add(matchupsItem);
            }
        }

        private void FinalScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            String teamScore1 = firstTeamScoreTxtBox.Text;
            String teamScore2 = secondTeamScoreTxtBox.Text;

            int score1 = int.Parse(teamScore1);
            int score2 = int.Parse(teamScore2);

            int selectedRound = roundDropDown.SelectedIndex;
            int selectedPairing = matchupsListBox.SelectedIndex;

            if (tournament.Rounds[selectedRound].Pairings[selectedPairing].NextRound != null)
            {
                tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].Score = score1;
                tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].Score = score2;

                //Create a Matchup and a single MatchupEntry if a it doesn't already exist
                //IDEA=> get the matchup id from RoundMatchups then check if MatchupEntries has 0 or 1 rows matching that matchupId
                int tournamentId = tournament.TournamentId;
                IRound currRound = source.getRoundByTournamentIdandRoundNum(new Round()
                {
                    TournamentId = tournamentId,
                    RoundNum = selectedRound+1,
                });

                List<IRoundMatchup> roundMatchups = source.getRoundMatchupsByRoundId(new Round()
                {
                    RoundId = currRound.RoundId,
                });
                //tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].MatchupId;
                List<IMatchupEntry> entries = source.getMatchupEntriesByMatchupId(roundMatchups[roundMatchups.Count-1].MatchupId);

                if (entries.Count == 2)
                {
                    IMatchup matchup = source.createMatchup(new Matchup());

                    source.createRoundMatchup(new RoundMatchup()
                    {
                        MatchupId = matchup.MatchupId,
                        RoundId = currRound.RoundId,
                    });

                    if (score1 > score2)
                    {
                        IMatchupEntry newMatchupEntry = source.createMatchupEntry(new MatchupEntry()
                        {
                            MatchupId = matchup.MatchupId,
                            TournamentEntryId = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].TournamentEntryId,
                            Score = 0
                        });
                    }
                    else
                    {
                        IMatchupEntry newMatchupEntry = source.createMatchupEntry(new MatchupEntry()
                        {
                            MatchupId = matchup.MatchupId,
                            TournamentEntryId = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].TournamentEntryId,
                            Score = 0
                        });
                    }
                }
                else
                {
                    if (score1 > score2)
                    {
                        List<IMatchupEntry> matchupEntries = source.getTournamentEntryIdFromPreviousMatchup(new MatchupEntry()
                        {
                            MatchupId = entries[0].MatchupId,
                        });

                        IMatchupEntry newMatchupEntry = source.createMatchupEntry(new MatchupEntry()
                        {
                            MatchupId = entries[0].MatchupId,
                            //TournamentEntryId = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].TournamentEntryId,
                            TournamentEntryId = matchupEntries[0].TournamentEntryId,
                            Score = 0
                        });
                    }
                    else
                    {
                        List<IMatchupEntry> matchupEntries = source.getTournamentEntryIdFromPreviousMatchup(new MatchupEntry()
                        {
                            MatchupId = entries[0].MatchupId,
                        });

                        IMatchupEntry newMatchupEntry = source.createMatchupEntry(new MatchupEntry()
                        {
                            MatchupId = entries[0].MatchupId,
                            //TournamentEntryId = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].TournamentEntryId,
                            TournamentEntryId = matchupEntries[matchupEntries.Count - 1].TournamentEntryId,
                            Score = 0
                        });
                    }
                }

                bool valid = tournament.RecordResult(tournament.Rounds[selectedRound].Pairings[selectedPairing]);

                if (valid)
                {
                    TournamentViewListBoxItem currItem = (TournamentViewListBoxItem)matchupsListBox.Items[selectedPairing];
                    currItem.imageURL = "Assets/confirm.png";
                    matchupsListBox.Items.Refresh();
                }
            }
            else //Last Matchup
            {
                tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].Score = score1;
                tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].Score = score2;

                TournamentViewListBoxItem currItem = (TournamentViewListBoxItem)matchupsListBox.Items[selectedPairing];
                currItem.imageURL = "Assets/trophy.png";
                matchupsListBox.Items.Refresh();

                if (score1 > score2)
                {
                    String team1Name = source.getTeam(tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].TheTeam.TeamId).TeamName;

                    firstTeamLabel.Content = team1Name;
                    versusLabel.Content = " is the ";
                    secondTeamLabel.Content = "winner!";

                }
                else
                {
                    String team2Name = source.getTeam(tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].TheTeam.TeamId).TeamName;

                    firstTeamLabel.Content = team2Name;
                    versusLabel.Content = " is the ";
                    secondTeamLabel.Content = "winner!";
                }
            }

            //Save score to database
            int matchupEntryId1 = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].MatchupEntryId;
            int matchupEntryId2 = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].MatchupEntryId;

            IMatchupEntry matchupEntry1 = source.updateMatchupEntryScore(matchupEntryId1, score1);
            IMatchupEntry matchupEntry2 = source.updateMatchupEntryScore(matchupEntryId2, score2);

        }
    }
}