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
            //InitializeComponent();
            controller = ApplicationController.getTournamentViewer();
            source = ApplicationController.getProvider();

            tournamentNameLbl.Content = inTourney.TournamentName;

            readMatchups(inTourney);
        }

        /// <summary>
        /// 
        /// </summary>
        private void readMatchups(ITournament inTourney)
        {
            List<IRound> roundList = inTourney.Rounds;
            List<IMatchup> matchupsList = new List<IMatchup>();
            for (int i = 0; i < roundList.Count; i++)
                for (int j = 0; j < roundList[i].Pairings.Count; j++)
                    matchupsList.Add(roundList[i].Pairings[j]);

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
    }
}