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
        private IUser user;

        public Dashboard(User thisUser)
        {
            InitializeComponent();
            user = thisUser;
            this.source = ApplicationController.getProvider();
            business = ApplicationController.getController();

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
            //Look at selected item in tournamentList, pass that id to Tournament For
            this.source = ApplicationController.getProvider();
            var tournaments = source.getAllTournaments();
            int selectedTounrey = tournamentList.SelectedIndex;
            TournamentViewUI viewUI = new TournamentViewUI(tournaments[selectedTounrey]);
            viewUI.Show();
        }

        private void CreateTournament_Click(object sender, RoutedEventArgs e)
        {
            CreateTournament newTournament = new CreateTournament();
            newTournament.Show();
            this.Close();
        }
    }
}