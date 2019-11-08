using System;
using System.Collections.Generic;
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

        public TournamentViewUI(ITournament inTourney)
        {
            InitializeComponent();
            controller = ApplicationController.getTournamentViewer();
            source = ApplicationController.getProvider();

            tournamentNameLbl.Content = inTourney.TournamentName;


            readMatchups(inTourney);
            readRounds(inTourney);
            roundDropDown.SelectedIndex = 0;
        }

        private void readRounds(ITournament inTourney)
        {
            throw new NotImplementedException("");
        }

        /// <summary>
        /// 
        /// </summary>
        private void readMatchups(ITournament inTourney)
        {
            List<IRound> roundList = inTourney.Rounds;
            for (int i = 0; i < roundList.Count; i++)
                roundDropDown.Items.Add(roundList[i].RoundNum);
            //roundDropDown.ItemsSource = roundList;
            List<string> matchupsList = new List<string>();
            for (int i = 0; i < roundList.Count / 2; i++)
            {
                for (int j = 0; j < roundList[i].Pairings.Count; j++)
                {
                    //int teamID = roundList[i].Pairings[j].Teams[k].TheTeam.TeamId;
                    int teamID = 58 + 2 * j;
                    string teamName1 = source.getTeamName(teamID);
                    string teamName2 = source.getTeamName(teamID + 1);
                    matchupsList.Add(teamName1 + " VS " + teamName2);
                }
            }

            matchupsListBox.ItemsSource = matchupsList;
        }

        /*
         * Records the final score of the game. 
         */
        private void finalScoreBtn_Click(object sender, RoutedEventArgs e)
        {

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

        }
    }
}