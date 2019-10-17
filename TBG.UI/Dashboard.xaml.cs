using System.Windows;
using TBG.Core.Interfaces;
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

        public Dashboard(IProvider source, IController business)
        {
            InitializeComponent();
            this.source = source;
            this.business = business;

            for (int i = 0; i < 10; i++)
            {
                this.tournamentList.Items.Add(new TournamentListBoxItem("Item " + i));
                this.typeFilter.Items.Add("Item " + i);
            }
        }

        public void Load_Tournament(object sender, RoutedEventArgs e)
        {

        }

        public void Close_Application(object sender, RoutedEventArgs e)
        {
            WindowHelper.Exit_App(this);
        }
    }
}
