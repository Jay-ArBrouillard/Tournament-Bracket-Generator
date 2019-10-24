using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for Tournament.xaml
    /// </summary>
    public partial class Tournament : Window
    {
        private IProvider source;

        public Tournament()
        {
            InitializeComponent();
            source = ApplicationController.GetProvider();
            List<Team> teams = convertToTeam(source.getAllTeams());
            participantsTreeView.ItemsSource = teams;
        }

        private List<Team> convertToTeam(List<ITeam> list)
        {
            List<Team> result = new List<Team>();

            foreach(ITeam team in list)
            {
                result.Add(new Team(team.TeamName, team.TeamMembers));
            }

            return result;
        }

        private void Create_New_Team_Click(object sender, RoutedEventArgs e)
        {
            TeamWindow teamWindow = new TeamWindow();
            teamWindow.Show();
            this.Close();
        }

        private void Create_New_Prize_Click(object sender, RoutedEventArgs e)
        {
            PrizeUI prizes = new PrizeUI();
            prizes.Show();
        }

    }
}
