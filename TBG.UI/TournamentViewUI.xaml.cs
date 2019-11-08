using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Models;

namespace TBG.UI
{

    public partial class TournamentViewUI : Window
    {

        ITournamentViewer controller;
        IProvider source;
        List<string> matchupsList = new List<string>();
        ITournament thisTournament;

        public TournamentViewUI(ITournament inTourney)
        {
            thisTournament = inTourney;
            InitializeComponent();
            controller = ApplicationController.getTournamentViewer();
            source = ApplicationController.getProvider();

            tournamentNameLbl.Content = inTourney.TournamentName;

            for (int i = 0; i < inTourney.Rounds.Count; i++)
            {
                roundDropDown.Items.Add(inTourney.Rounds[i].RoundNum);
            }
            readMatchups(inTourney.Rounds[0]);
            //readRounds(inTourney);
            roundDropDown.SelectedIndex = 0;
        }

        private void readRounds(ITournament inTourney)
        {
            throw new NotImplementedException("");
        }

        /// <summary>
        /// 
        /// </summary>
        private void readMatchups(IRound round)
        {
            //List<IRound> roundList = inTourney.Rounds;
            //roundDropDown.ItemsSource = roundList;
            for (int i = 0; i < round.Pairings.Count; i++)
            {
                //int teamID = roundList[i].Pairings[j].Teams[k].TheTeam.TeamId;
                int teamID = 58 + 2 * i;
                string teamName1 = source.getTeamName(teamID);
                string teamName2 = source.getTeamName(teamID + 1);
                matchupsListBox.Items.Add(teamName1 + " VS " + teamName2);
            }

            //matchupsListBox.ItemsSource = matchupsList;
            matchupsList.Clear();
        }

        /*
         * Records the final score of the game. 
         */
        private void finalScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedRound = roundDropDown.SelectedIndex;
            int selectedMatchup = matchupsListBox.SelectedIndex;
            int team1Score = -1;
            int team2Score = -1;
            if (validateScore(firstTeamScoreTxtBox.Text) && validateScore(secondTeamScoreTxtBox.Text))
            {
                team1Score = int.Parse(firstTeamScoreTxtBox.Text);
                team2Score = int.Parse(secondTeamScoreTxtBox.Text);
            }
            IRound thisRound = thisTournament.Rounds[roundDropDown.SelectedIndex];
            thisRound.Pairings[selectedMatchup].Teams[0].Score = team1Score;
            thisRound.Pairings[selectedMatchup].Teams[1].Score = team2Score;
            thisTournament.RecordResult(thisRound.Pairings[matchupsListBox.SelectedIndex]);
        }

        /*
         * This will show info in the listbox about the selected match. Things like wins, losses, team members, etc.
         * 
         * (May want more than one box?)
         */
        private void showMatchupDetails()
        {

        }

        private void roundDropDown_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = roundDropDown.SelectedIndex;
            matchupsListBox.Items.Clear();
            readMatchups(thisTournament.Rounds[index]);
        }

        public bool validateScore(string score)
        {
            return Regex.Match(score, @"\d").Success;
        }
    }
}