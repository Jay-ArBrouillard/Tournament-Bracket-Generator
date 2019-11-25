using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private ITournamentController tournamentController;

        public TournamentViewUI(ITournament inTourney)
        {
            InitializeComponent();
            source = ApplicationController.getProvider();
            tournamentController = ApplicationController.getTournamentController();
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
                    TeamId = teams[0].TheTeam.TeamId
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
                    TeamId = teams[1].TheTeam.TeamId
                };

                teamTwoDataGrid.Items.Add(dataGridItem);
            }

            firstTeamLabel.Content = theTeam1.TeamName;
            secondTeamLabel.Content = theTeam2.TeamName;
            firstTeamScoreTxtBox.Text = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[0].Score.ToString();
            secondTeamScoreTxtBox.Text = tournament.Rounds[selectedRound].Pairings[selectedPairing].Teams[1].Score.ToString();
        }

        private void RoundDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialization == true) return; //Prevents this method getting called twice when switching round

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
                roundMatchups = roundMatchups.OrderBy(x => x.MatchupNumber).ToList();

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
            var validatedTeam1Score = tournamentController.validateScore(firstTeamScoreTxtBox.Text);
            var validatedTeam2Score = tournamentController.validateScore(secondTeamScoreTxtBox.Text);
            bool invalidTeamScore = false;

            //Validates score before saving
            if (validatedTeam1Score == -1)
            {
                teamOneInvalidScoreLbl.Content = "Score is not valid";
                invalidTeamScore = true;
            }
            else
            {
                teamOneInvalidScoreLbl.Content = "";
            }

            if (validatedTeam2Score == -1)
            {
                teamTwoInvalidScoreLbl.Content = "Score is not valid";
                invalidTeamScore = true;
            }
            else
            {
                teamTwoInvalidScoreLbl.Content = "";
            }

            if(invalidTeamScore) { return; }

            var matchup = tournament.Rounds[roundDropDown.SelectedIndex].Pairings[matchupsListBox.SelectedIndex];
            var scoreValid = tournamentController.ScoreMatchup(matchup, validatedTeam1Score, validatedTeam2Score);

            if(!scoreValid) { return; } //Error Message?

            source.saveMatchupScore(matchup);

            scoreRecordedLbl.Content = "Score recorded successfully";
        }

        private void UnplayedCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(matchupsListBox.Items);
            view.Filter = MatchupFilter;
            view.Refresh();
        }

        private void UnplayedCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(matchupsListBox.Items);
            view.Filter = null;
            view.Refresh();
        }

        private bool MatchupFilter(object item)
        {
            TournamentViewListBoxItem matchup = item as TournamentViewListBoxItem;
            return matchup.imageURL.Contains("x-button");
        }

    }
}