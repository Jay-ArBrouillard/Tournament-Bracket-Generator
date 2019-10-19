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
        private IController business;

        public Dashboard()
        {
            InitializeComponent();
            this.source = ApplicationController.GetProvider();
            business = ApplicationController.GetController();

            var tournaments = source.GetAllTournaments();
            var tournamentTypes = source.GetTournamentTypes();

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
        }

        public void Load_Tournament(object sender, RoutedEventArgs e)
        {
            //Look at selected item in tournamentList, pass that id to Tournament Form
        }

        private void CreateTournament_Click(object sender, RoutedEventArgs e)
        {
            Tournament newTournament = new Tournament();
            newTournament.Show();
            this.Close();
        }
    }
}