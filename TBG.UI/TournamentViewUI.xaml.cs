﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
            }

            //Set tournament Name
            tournamentNameLbl.Content = tournament.TournamentName;

            roundDropDown.SelectedItem = null;
            roundDropDown.SelectedIndex = 0;
            populateMatchupListBox(0);
            initialization = false;
        }

        private void MatchupsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedRound = roundDropDown.SelectedIndex;
            int selectedPairing = matchupsListBox.SelectedIndex;

            if (selectedPairing == -1 || selectedRound == -1) return;   //For some reason this method is getting called when changing the round so we need this check

            List<IMatchupEntry> teams = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams;

            ITeam theTeam1 = source.getTeam(teams[0].TheTeam.TeamId);
            ITeam theTeam2 = source.getTeam(teams[1].TheTeam.TeamId);

            List<ITeamMember> team1Ids = source.getTeamMembersByTeamId(teams[0].TheTeam.TeamId);
            List<ITeamMember> team2Ids = source.getTeamMembersByTeamId(teams[1].TheTeam.TeamId);

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
            IRound thisRound = source.getRoundByTournamentIdandRoundNum(new Round(tournament.TournamentId, selectedRound + 1));

            if (thisRound != null)
            {
                List<IRoundMatchup> roundMatchups = source.getRoundMatchupsByRoundId(new RoundMatchup(thisRound.RoundId));

                foreach (IRoundMatchup roundMatchup in roundMatchups)
                {
                    List<IMatchupEntry> matchupEntries = source.getMatchupEntriesByMatchupId(roundMatchup.MatchupId);

                    if (matchupEntries != null && matchupEntries.Count == 2)
                    {
                        ITournamentEntry tournamentEntry1 = source.getTournamentEntry(matchupEntries[0].TournamentEntryId);
                        ITournamentEntry tournamentEntry2 = source.getTournamentEntry(matchupEntries[1].TournamentEntryId);

                        String imageURLString = "Assets/x-button.png";
                        if (matchupEntries[0].Score != 0 || matchupEntries[1].Score != 0)
                        {
                            imageURLString = "Assets/confirm.png";
                        }

                        TournamentViewListBoxItem matchupsItem = new TournamentViewListBoxItem()
                        {
                            Team1Name = source.getTeam(tournamentEntry1.TeamId).TeamName,
                            Team2Name = source.getTeam(tournamentEntry2.TeamId).TeamName,
                            imageURL = imageURLString
                        };

                        firstTeamScoreTxtBox.Text = matchupEntries[0].Score.ToString();
                        firstTeamScoreTxtBox.Text = matchupEntries[1].Score.ToString();
                        matchupsListBox.Items.Add(matchupsItem);
                    }
                }
            }
            else
            {
                //There are no Teams in the next round yet
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
            int nextRoundNumber = selectedRound + 2;
            int nextPairingNumber = selectedPairing / 2;

            //Set the local Data structure
            tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].Score = score1;
            tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].Score = score2;

            bool valid = tournament.RecordResult(tournament.Rounds[selectedRound].Pairings[selectedPairing]);

            if (valid)
            {
                TournamentViewListBoxItem currItem = (TournamentViewListBoxItem)matchupsListBox.Items[selectedPairing];
                currItem.imageURL = "Assets/confirm.png";
                matchupsListBox.Items.Refresh();
            }

            if (tournament.Rounds[selectedRound].Pairings[selectedPairing].NextRound != null)
            {


                IRound nextRound = source.getRoundByTournamentIdandRoundNum(new Round(tournament.TournamentId, nextRoundNumber));
                if (nextRound == null)
                {
                    nextRound = source.createRound(new Round(tournament.TournamentId, nextRoundNumber));
                }
                //Only create a new matchup if a matchup with the correct round_id and matchup_number doesn't exist
                IRoundMatchup roundMatchup = source.getRoundMatchupByRoundIdAndMatchupNumber(new RoundMatchup(nextRound.RoundId, nextPairingNumber));
                IMatchup newMatchup = null;

                if (roundMatchup == null)
                {
                    newMatchup = source.createMatchup(new Matchup());
                }

                IRoundMatchup newRoundMatchup = null;
                if (roundMatchup == null)
                {
                    newRoundMatchup = source.createRoundMatchup(new RoundMatchup(nextRound.RoundId, newMatchup.MatchupId, nextPairingNumber));
                }

                if (score1 > score2)
                {
                    int temp;
                    if (newMatchup == null)
                    {
                        temp = roundMatchup.MatchupId;
                    }
                    else
                    {
                        temp = newMatchup.MatchupId;
                    }


                    IMatchupEntry newMatchupEntry = source.createMatchupEntry(new MatchupEntry()
                    {
                        MatchupId = temp,
                        TournamentEntryId = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].TheTeam.TournamentEntryId
                    });
                    tournament.Rounds[selectedRound + 1].Pairings[nextPairingNumber].MatchupId = temp;
                    tournament.Rounds[selectedRound + 1].Pairings[nextPairingNumber].Teams[0].TournamentEntryId =  newMatchupEntry.TournamentEntryId;
                    tournament.Rounds[selectedRound + 1].Pairings[nextPairingNumber].Teams[0].MatchupId = temp;
                }
                else
                {
                    int temp;
                    if (newMatchup == null)
                    {
                        temp = roundMatchup.MatchupId;
                    }
                    else
                    {
                        temp = newMatchup.MatchupId;
                    }

                    IMatchupEntry newMatchupEntry = source.createMatchupEntry(new MatchupEntry()
                    {
                        MatchupId = temp,
                        TournamentEntryId = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].TheTeam.TournamentEntryId
                    });

                    tournament.Rounds[selectedRound + 1].Pairings[nextPairingNumber].MatchupId = temp;
                    tournament.Rounds[selectedRound + 1].Pairings[nextPairingNumber].Teams[0].TournamentEntryId = newMatchupEntry.TournamentEntryId;
                    tournament.Rounds[selectedRound + 1].Pairings[nextPairingNumber].Teams[0].MatchupId = temp;
                }
            }
            else //Final Matchup
            {
                /*int matchupId = tournament.Rounds[selectedRound].Pairings[selectedPairing].MatchupId;

                if (score1 > score2)
                {
                    int tournamentEntryId = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].TheTeam.TournamentEntryId;
                    IMatchupEntry winner = source.getMatchupEntryByMatchupIdAndTournamentEntryId(matchupId, tournamentEntryId);
                }
                else
                {
                    int tournamentEntryId = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].TheTeam.TournamentEntryId;

                    IMatchupEntry winner = source.getMatchupEntryByMatchupIdAndTournamentEntryId(matchupId, tournamentEntryId);
                }*/
                tournamentNameLbl.Content += " (Completed)";

                TournamentViewListBoxItem currItem = (TournamentViewListBoxItem)matchupsListBox.Items[selectedPairing];
                currItem.imageURL = "Assets/confirm.png";
                matchupsListBox.Items.Refresh();
            }


        }
    }
}