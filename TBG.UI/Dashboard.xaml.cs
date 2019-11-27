using System;
using System.Collections.Generic;
using System.Linq;
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

        public Dashboard(IUser user)
        {
            InitializeComponent();
            this.user = user;
            this.source = ApplicationController.getProvider();
            tournamentController = ApplicationController.getTournamentController();

            var tournamentTypes = source.getTournamentTypes();

            List<TournamentListBoxItem> tourneys = new List<TournamentListBoxItem>();

            BuildTournamentList();

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

        private void BuildTournamentList()
        {
            tournamentList.Items.Clear();
            allTournaments = source.getAllTournaments();
            foreach (var tournament in allTournaments)
            {
                var item = new TournamentListBoxItem()
                {
                    Name = tournament.TournamentName,
                    Id = tournament.TournamentId
                };

                tournamentList.Items.Add(item);
            }
        }

        public void Load_Tournament(object sender, RoutedEventArgs e)
        {
            //Only load tournament if one is selected
            if (tournamentList.SelectedIndex == -1) return;
            var selectedItem = tournamentList.SelectedItem as TournamentListBoxItem;
            var selectedTournamentId = allTournaments.Where(x => x.TournamentId == selectedItem.Id).First().TournamentId;
            var selectedTournament = source.getTournament(selectedTournamentId);
            selectedTournament = tournamentController.rebuildTournament(selectedTournament);
            TournamentViewUI viewUI = new TournamentViewUI(selectedTournament);
            viewUI.Show();
        }

        public void Delete_Tournament(object sender, RoutedEventArgs e)
        {
            //Probably should add a confirmation to this
            if (tournamentList.SelectedIndex == -1) return;
            var selectedItem = tournamentList.SelectedItem as TournamentListBoxItem;
            var selectedTournamentId = allTournaments.Where(x => x.TournamentId == selectedItem.Id).First().TournamentId;

            var deletedTournament = source.deleteTournament(selectedTournamentId);

            BuildTournamentList();
            messageBox.Text = "Successfully deleted tournament";
        }

        private void CreateTournament_Click(object sender, RoutedEventArgs e)
        {
            CreateTournament newTournament = new CreateTournament(user);
            newTournament.Show();
        }
    }
}