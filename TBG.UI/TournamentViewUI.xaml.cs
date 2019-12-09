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

        public TournamentViewUI(ITournament inTourney, bool fullAccess = true)
        {
            InitializeComponent();
            source = ApplicationController.getProvider();
            tournamentController = ApplicationController.getTournamentController();
            tournament = inTourney;

            //Add all the rounds to the Rounds view
            populateRoundList();

            //Set tournament Name
            tournamentNameLbl.Content = tournament.TournamentName;

            //Results Button
            var activeRound = tournament.Rounds.Where(x => x.RoundNum == tournament.ActiveRound).First();
            var isActiveRoundValid = tournamentController.validateActiveRound(tournament);

            if (!isActiveRoundValid)
            {
                resultsBtn.Visibility = Visibility.Visible;
            }

            matchupGrid.IsEnabled = fullAccess;
            finalizeRoundBtn.IsEnabled = fullAccess;

            populateMatchupListBox(0);
            initialization = false;
        }

        private void MatchupsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedRound = roundDropDown.SelectedIndex;
            int selectedPairing = matchupsListBox.SelectedIndex;

            if (selectedPairing == -1 || selectedRound == -1)
            {
                matchupGrid.Visibility = Visibility.Hidden;
                return;   //For some reason this method is getting called when changing the round so we need this check
            }

            List<IMatchupEntry> teams = tournament.Rounds[selectedRound].Matchups[selectedPairing].MatchupEntries;

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

            populateMatchupDetails(tournament.Rounds[selectedRound].Matchups[selectedPairing]);

            firstTeamLabel.Content = theTeam1.TeamName;
            secondTeamLabel.Content = theTeam2.TeamName;
            firstTeamScoreTxtBox.Text = tournament.Rounds[selectedRound].Matchups[selectedPairing].MatchupEntries[0].Score.ToString();
            secondTeamScoreTxtBox.Text = tournament.Rounds[selectedRound].Matchups[selectedPairing].MatchupEntries[1].Score.ToString();
            matchupGrid.Visibility = Visibility.Visible;
        }

        private void RoundDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialization == true) return; //Prevents this method getting called twice when switching round

            int selectedRound = roundDropDown.SelectedIndex;
            teamTwoDataGrid.Items.Clear();
            teamOneDataGrid.Items.Clear();

            populateMatchupListBox(selectedRound);
            lblFinalized.Content = "";
        }

        private void populateRoundList()
        {
            for (int i = 1; i <= tournament.Rounds.Count; i++)
            {
                roundDropDown.Items.Add(i);
            }

            roundDropDown.SelectedItem = null;
            roundDropDown.SelectedIndex = 0;
        }

        private void populateMatchupListBox(int selectedIndex)
        {
            var matchupIndex = matchupsListBox.SelectedIndex;
            matchupsListBox.SelectedItem = null;
            matchupsListBox.Items.Clear();
            var selectedRound = tournament.Rounds[selectedIndex];
            foreach (var matchup in selectedRound.Matchups)
            {
                if (matchup != null && matchup.MatchupEntries.Count == 2)
                {

                    String imageURLString = "Assets/x-button.png";
                    if (matchup.Completed == true) { imageURLString = "Assets/confirm.png"; }

                    var team1Name = tournament.Teams.Find(x => x.TeamId == matchup.MatchupEntries[0].TheTeam.TeamId).TeamName;
                    var team2Name = tournament.Teams.Find(x => x.TeamId == matchup.MatchupEntries[1].TheTeam.TeamId).TeamName;

                    TournamentViewListBoxItem matchupsItem = new TournamentViewListBoxItem()
                    {
                        Team1Name = team1Name,
                        Team2Name = team2Name,
                        imageURL = imageURLString,
                        Completed = matchup.Completed
                    };
                    matchupsListBox.Items.Add(matchupsItem);
                }
            }
            if (matchupIndex != -1)
            {
                matchupsListBox.SelectedIndex = matchupIndex;
            }
        }

        private void FinalScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            var validatedTeam1Score = tournamentController.validateScore(firstTeamScoreTxtBox.Text);
            var validatedTeam2Score = tournamentController.validateScore(secondTeamScoreTxtBox.Text);
            bool invalidTeamScore = false;

            //Validates score before saving
            if (validatedTeam1Score == 0 && validatedTeam2Score == 0 || validatedTeam1Score == validatedTeam2Score)
            {
                scoreRecordedLbl.Content = "Scores are not valid";
                return;
            }

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

            if (invalidTeamScore) { return; }

            var matchup = tournament.Rounds[roundDropDown.SelectedIndex].Matchups[matchupsListBox.SelectedIndex];
            var scoreValid = tournamentController.ScoreMatchup(matchup, validatedTeam1Score, validatedTeam2Score);

            if (!scoreValid) { return; } //Error Message?

            source.saveTournamentEntry(matchup);
            source.saveMatchupScore(matchup);
            source.savePersonStats(matchup);
            source.saveTeamScore(matchup);

            populateMatchupDetails(matchup);
            
            populateMatchupListBox(roundDropDown.SelectedIndex);

            scoreRecordedLbl.Content = "Score recorded successfully";
        }

        private void populateMatchupDetails(IMatchup matchup)
        {
            teamOneDataGrid.Items.Clear();
            foreach (IPerson p in matchup.MatchupEntries[0].TheTeam.Members)
            {
                TournamentViewDataGridItem dataGridItem = new TournamentViewDataGridItem()
                {
                    PlayerName = p.FirstName + " " + p.LastName,
                    Wins = p.Wins,
                    Losses = p.Losses,
                    TeamId = matchup.MatchupEntries[0].TheTeam.TeamId
                };

                teamOneDataGrid.Items.Add(dataGridItem);
            }

            teamTwoDataGrid.Items.Clear();
            foreach (IPerson p in matchup.MatchupEntries[1].TheTeam.Members)
            {
                TournamentViewDataGridItem dataGridItem = new TournamentViewDataGridItem()
                {
                    PlayerName = p.FirstName + " " + p.LastName,
                    Wins = p.Wins,
                    Losses = p.Losses,
                    TeamId = matchup.MatchupEntries[1].TheTeam.TeamId
                };

                teamTwoDataGrid.Items.Add(dataGridItem);
            }
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
            return !matchup.Completed;
        }

        private void FinalizeRoundBtn_Click(object sender, RoutedEventArgs e)
        {
            //Confirm that all matchups in active round are complete
            //Build matchups for next round
            var activeRound = tournament.Rounds.Where(x => x.RoundNum == tournament.ActiveRound).First();
            var isRoundComplete = tournamentController.validateRoundCompletion(activeRound);
            if (!isRoundComplete)
            {
                lblFinalized.Content = "Active round not finished";
                return; //Error Message somewhere
            }

            var isActiveRoundValid = tournamentController.validateActiveRound(tournament);

            if (isActiveRoundValid)
            {
                tournament = tournamentController.advanceRound(tournament);
                source.saveActiveRound(tournament);
                lblFinalized.Content = "Round finalized";
            }
            else
            {
                lblFinalized.Content = "Tournament is over";
                resultsBtn.Visibility = Visibility.Visible;
            }

            
        }
        private void ResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow resultWindow = new ResultWindow(tournament);
            resultWindow.Show();
        }
    }
}