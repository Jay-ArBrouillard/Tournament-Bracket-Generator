using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private IProvider source;
        private ITournamentController business;
        private IUser user;

        public Dashboard(User thisUser)
        {
            InitializeComponent();
            user = thisUser;
            this.source = ApplicationController.getProvider();
            business = ApplicationController.getTournamentController();

            var tournaments = source.getAllTournaments();
            var tournamentTypes = source.getTournamentTypes();

            foreach (var tournament in tournaments)
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
            if (thisUser == null)   
            {
                createTournamentButton.Visibility = Visibility.Hidden;
            }
        }

        public void Load_Tournament(object sender, RoutedEventArgs e)
        {
            //Only load tournament if one is selected
            if (tournamentList.SelectedIndex == -1) return;

            //Look at selected item in tournamentList, pass that id to Tournament For
            this.source = ApplicationController.getProvider();
            var tournaments = source.getAllTournaments();
            int selectedTourney = tournamentList.SelectedIndex;
            //Need to get the source
            ITournament selected = tournaments[selectedTourney];
            ITournament selectedTournament = source.getTournamentByName(selected.TournamentName);
            //selectedTournament. source.getTournamentEntriesByTournmentId(selected.TournamentId);

            int tournamentId = source.getTournamentByName("Test Tournament").TournamentId;   //Change

            List<ITournamentEntry> entries = source.getTournamentEntriesByTournamentId(selected.TournamentId);

            selectedTournament.Participants = entries;
            selectedTournament = business.createSingleEliminationTournament(selectedTournament);

            TournamentViewUI viewUI = new TournamentViewUI(selectedTournament);
            viewUI.Show();
        }

        private void CreateTournament_Click(object sender, RoutedEventArgs e)
        {
            CreateTournament newTournament = new CreateTournament();
            newTournament.Show();
            //this.Close();
        }
    }
}