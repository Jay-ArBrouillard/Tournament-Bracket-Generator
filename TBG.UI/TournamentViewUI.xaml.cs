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

        ITournamentViewer controller;
        IProvider source;
        ITournament thisTournament;
        DataTable dtOne = new DataTable();
        DataTable dtTwo = new DataTable();
        public TournamentViewUI(ITournament inTourney)
        {
            thisTournament = inTourney;
                /*new SingleEliminationTournament() {
                TournamentId = inTourney.TournamentId,
                UserId = inTourney.UserId,
                TournamentName = inTourney.TournamentName,
                EntryFee = inTourney.EntryFee,
                TotalPrizePool = inTourney.TotalPrizePool,
                TournamentTypeId = inTourney.TournamentTypeId,
                Participants = inTourney.Participants,
                Prizes = inTourney.Prizes,
                Rounds = inTourney.Rounds
            };*/

            InitializeComponent();
            controller = ApplicationController.getTournamentViewer();
            source = ApplicationController.getProvider();

            tournamentNameLbl.Content = inTourney.TournamentName;

            //Adds each round in the tourney to the drop down
            /*for (int i = 0; i < inTourney.Rounds.Count; i++)
            {
                roundDropDown.Items.Add(inTourney.Rounds[i].RoundNum);
            }*/

            roundDropDown.Items.Add(1);

            //Creates the columns needed in data table
            prepareDatatable();

            //Loads the matchups found in the first round
            readMatchups(inTourney.Rounds[0]);
            roundDropDown.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void readMatchups(IRound round)
        {

            //Displays each matchup to the user
            for (int i = 0; i < round.Pairings.Count; i++)
            {
                int teamID = 58 + 2 * i;
                Matchup matchup = new Matchup() {
                    TeamOneName = source.getTeamName(teamID),
                    TeamTwoName = source.getTeamName(teamID + 1),
                    imageURL = "Assets/x-button.png"
                }; 

                
                //string teamName1 = source.getTeamName(teamID);
                //string teamName2 = source.getTeamName(teamID + 1);
                matchupsListBox.Items.Add(matchup);
                
            }
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

            //TODO put inside business rules
            if(selectedMatchup < 0)
            {
                noMatchupSelectedLbl.Content = "No matchup is selected.";
                return;
            }

            //Validates score before saving
            if (validateScore(firstTeamScoreTxtBox.Text) && validateScore(secondTeamScoreTxtBox.Text))
            {
                team1Score = int.Parse(firstTeamScoreTxtBox.Text);
                team2Score = int.Parse(secondTeamScoreTxtBox.Text);
            } else
            {
                //Gives appropriate error message if one score isn't valid.
                if (!validateScore(firstTeamScoreTxtBox.Text))
                {
                    teamOneInvalidScoreLbl.Content = "Score is not valid";
                    return;
                } else
                {
                    teamTwoInvalidScoreLbl.Content = "Score is not valid";
                    return;
                }
            }

            IRound thisRound = thisTournament.Rounds[roundDropDown.SelectedIndex];
            thisRound.Pairings[selectedMatchup].Teams[0].Score = team1Score;
            thisRound.Pairings[selectedMatchup].Teams[1].Score = team2Score;
            bool valid = thisTournament.RecordResult(thisRound.Pairings[matchupsListBox.SelectedIndex]);

            if (valid)
            {
                scoreRecordedLbl.Content = "Score recorded successfully";
                Matchup thisMatchup = (Matchup)(matchupsListBox.SelectedItem);
                thisMatchup.imageURL = "Assets/confirm.png";
                matchupsListBox.Items.Refresh();
                bool roundComplete = true;

                foreach (Matchup matchup in matchupsListBox.Items)
                {
                    if (matchup.imageURL.Equals("Assets/x-button.png"))
                    {
                        roundComplete = false;
                        break;
                    }
                }

                //Add the current matchups winner to the next round
                /*
                IMatchup iMatchup = thisRound.Pairings[selectedMatchup];
                IRound nextRound = null;

                IMatchupEntry team1 = iMatchup.Teams[0];
                IMatchupEntry team2 = iMatchup.Teams[1];
                IMatchupEntry winner = null;


                if (team1.Score > team2.Score)
                {
                    winner = team1;
                }
                else
                {
                    winner = team2;
                }

                iMatchup.NextRound.Teams.Add(winner);*/

                //Check all matchups complete in the round
                if (roundComplete)
                {
                    int lastRound = roundDropDown.Items.Count;
                    lastRound++;

                    if (lastRound < thisTournament.Rounds.Count)
                    {
                        roundDropDown.Items.Add(lastRound);
                    }
                }

            }
            else
            {
                scoreRecordedLbl.Content = "Score recorded error";
            }
        }

        private void roundDropDown_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int selected = roundDropDown.SelectedIndex;
            int index = selected;
            matchupsListBox.Items.Clear();
            readMatchups(thisTournament.Rounds[index]);
        }

        private bool validateScore(string score)
        {
            return Regex.Match(score, @"^\d+$").Success;
        }

        private void changeSelectedMatchDetails(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            dtOne.Clear();
            dtTwo.Clear();
            int currMatchup = matchupsListBox.SelectedIndex;
            /*
            System.Console.WriteLine("currMatchup: " + currMatchup);

            if (currMatchup == -1 || currMatchup == 0) currMatchup = 0;
            if (roundDropDown.SelectedIndex == -1 || roundDropDown.SelectedIndex == 0) currMatchup = 0;*/
            ITournament test = thisTournament;

            int teamOneID = thisTournament.Rounds[roundDropDown.SelectedIndex].Pairings[currMatchup].Teams[0].TheTeam.TeamId;
            int teamTwoID = thisTournament.Rounds[roundDropDown.SelectedIndex].Pairings[currMatchup].Teams[1].TheTeam.TeamId;

            //Changes teamOneID/teamTwoID in lines 127 and 130 to 58 + 2 * currMatchup to test with the testTournament on the main screen.
            string teamName = source.getTeamName(58 + 2 * currMatchup);
            firstTeamLabel.Content = teamName;
            ITeam teamOne = source.getTeam(teamName);
            teamName = source.getTeamName(59 + 2 * currMatchup);
            secondTeamLabel.Content = teamName;
            ITeam teamTwo = source.getTeam(teamName);

            displayTeamOneInfo(teamOne);
            teamOneDataGrid.DataContext = dtOne.DefaultView;

            displayTeamTwoInfo(teamTwo);
            teamTwoDataGrid.DataContext = dtTwo.DefaultView;
        }
        
        /// <summary>
        /// Prepares both datatables with the correct columns
        /// </summary>
        private void prepareDatatable()
        {
            dtOne.Columns.Add("Name", typeof(string));
            dtOne.Columns.Add("Wins", typeof(int));
            dtOne.Columns.Add("Losses", typeof(int));
            dtOne.Columns.Add("W-L Ratio", typeof(double));

            dtTwo.Columns.Add("Name", typeof(string));
            dtTwo.Columns.Add("Wins", typeof(int));
            dtTwo.Columns.Add("Losses", typeof(int));
            dtTwo.Columns.Add("W-L Ratio", typeof(double));
        }

        private void displayTeamOneInfo(ITeam teamOne)
        {
            List<ITeamMember> team = source.getTeamMembersByTeamId(teamOne.TeamId);
            displayTeamInfo(teamOne, dtOne);
            displayTeammates(team, dtOne);
        }

        private void displayTeamTwoInfo(ITeam teamTwo)
        {
            List<ITeamMember> team = source.getTeamMembersByTeamId(teamTwo.TeamId);
            displayTeamInfo(teamTwo, dtTwo);
            displayTeammates(team, dtTwo);
        }

        private void displayTeammates(List<ITeamMember> inTeam, DataTable inDT)
        {

            DataRow row = inDT.NewRow();
            
            for (int i = 0; i < inTeam.Count; i++)
            {
                IPerson person = source.getPerson(inTeam[i].PersonId);
                row = inDT.NewRow();
                row["Name"] = person.FirstName + " " + person.LastName;
                row["Wins"] = person.Wins;
                row["Losses"] = person.Losses;
                if (person.Losses == 0)
                    row["W-L Ratio"] = person.Wins;
                else
                    row["W-L Ratio"] = ((double)person.Wins) / person.Losses;

                inDT.Rows.Add(row);
            }
        }

        private void displayTeamInfo(ITeam inTeam, DataTable inDT)
        {

            DataRow row = inDT.NewRow();
            row["Name"] = inTeam.TeamName;
            row["Wins"] = inTeam.Wins;
            row["Losses"] = inTeam.Losses;
            if (inTeam.Losses == 0)
                row["W-L Ratio"] = 0;
            else
                row["W-L Ratio"] = ((double)inTeam.Wins) / inTeam.Losses;
            inDT.Rows.Add(row);
        }
    }
}