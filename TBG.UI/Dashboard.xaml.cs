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

        public Dashboard(IProvider source)
        {
            InitializeComponent();
            this.source = source;
            business = ApplicationController.getController();

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

        }
    }
}