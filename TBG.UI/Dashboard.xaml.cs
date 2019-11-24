using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Classes;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private IProvider source;
        private ITournamentController tournamentController;
        private IUser user;
        private List<ITournament> allTournaments;

        public Dashboard(User user)
        {
            InitializeComponent();
            this.user = user;
            this.source = ApplicationController.getProvider();
            tournamentController = ApplicationController.getTournamentController();

            allTournaments = source.getAllTournaments();
            var tournamentTypes = source.getTournamentTypes();

            foreach (var tournament in allTournaments)
            {
                var item = new TournamentListBoxItem(tournament.TournamentName);
                this.tournamentList.Items.Add(new ListBoxItem {
                    Content = item.Name
                });
            }

            foreach (var tournamentType in tournamentTypes)
            {
                this.typeFilter.Items.Add(new ListBoxItem {
                    Content = new Label {
                        Content = tournamentType.TournamentTypeName
                    }
                });
            }

            //Logged in as guest -> don't allow them to create a tournament
            if (user == null)   
            {
                createTournamentButton.Visibility = Visibility.Hidden;
            }
        }

        public void Load_Tournament(object sender, RoutedEventArgs e)
        {
            //Only load tournament if one is selected
            if (tournamentList.SelectedIndex == -1) return;

            var selectedTournament = source.getTournamentByName(allTournaments[tournamentList.SelectedIndex].TournamentName);
            selectedTournament = tournamentController.rebuildTournament(selectedTournament);
            TournamentViewUI viewUI = new TournamentViewUI(selectedTournament);
            viewUI.Show();
        }

        public void Delete_Tournament(object sender, RoutedEventArgs e)
        {

            Console.WriteLine("Temp: " + tournamentList.SelectedIndex);
            Console.WriteLine(tournamentList.SelectedIndex == -1);
            if (tournamentList.SelectedIndex == -1)
            {
                messageBox.Text = "Please select a tournament";
                return;
            }

            ITournament selectedTournament = source.getTournamentByName(allTournaments[tournamentList.SelectedIndex].TournamentName);
            ITournament tournament = source.deleteTournament(new Tournament(selectedTournament.TournamentId));

            if (tournament != null)
            {
                //Delete from Matchups Table if it doesn't have any matchupEntries since it isn't effected by the cascade from deleting tournament
                foreach (var matchup in source.getAllMatchups())
                {
                    if (source.getMatchupEntriesByMatchupId(matchup.MatchupId) != null)
                    {
                        IMatchup currMatchup = source.deleteMatchup(new Matchup(matchup.MatchupId));
                    }
                }

                tournamentList.Items.Clear();

                allTournaments = source.getAllTournaments();
                foreach (var t in allTournaments)
                {
                    var item = new TournamentListBoxItem(t.TournamentName);
                    this.tournamentList.Items.Add(new ListBoxItem
                    {
                        Content = item.Name
                    });
                }

                var tournamentTypes = source.getTournamentTypes();

                foreach (var tournamentType in tournamentTypes)
                {
                    this.typeFilter.Items.Add(new ListBoxItem
                    {
                        Content = new Label
                        {
                            Content = tournamentType.TournamentTypeName
                        }
                    });
                }

                tournamentList.Items.Refresh();
                messageBox.Text = "Successfully deleted tournament";
            }
            else
            {
                messageBox.Text = "Couldn't delete the selected Tournament";
            }
        }

        private void CreateTournament_Click(object sender, RoutedEventArgs e)
        {
            CreateTournament newTournament = new CreateTournament(user);
            newTournament.Show();
            this.Close();
        }
    }
}