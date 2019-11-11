using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using TBG.Core.Interfaces;
using TBG.Driver;
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
            String test = null;
            tournament = inTourney;

            //Add all the rounds to the Rounds view
            for (int i = 1; i <= tournament.Rounds.Count; i++)
            {
                roundDropDown.Items.Add(i);
            }

            roundDropDown.SelectedItem = null;
            roundDropDown.SelectedIndex = 0;
            displayRound1();
        }

        private void displayRound1()
        {
            int selectedRound = roundDropDown.SelectedIndex;

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

                bool valid = tournament.RecordResult(tournament.Rounds[selectedRound].Pairings[selectedPairing]);

                if (valid)
                {
                    TournamentViewListBoxItem currItem = (TournamentViewListBoxItem)matchupsListBox.Items[selectedPairing];
                    currItem.imageURL = "Assets/confirm.png";
                    matchupsListBox.Items.Refresh();
                }
            }

        }
    }
}