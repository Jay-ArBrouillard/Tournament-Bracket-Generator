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
            //var teams = convertToTeam(source.getAllTeams());
            List<TeamTreeView> teams = new List<TeamTreeView>();

            TeamTreeView team1 = new TeamTreeView()
            {
                Name = "TankTop Team"
            };
            team1.Members.Add(new TeamMemberTreeview()
            {
                Name = "Johnny Smith",
                Age = 21
            });
            team1.Members.Add(new TeamMemberTreeview()
            {
                Name = "Gerald Smith",
                Age = 31
            });
            teams.Add(team1);
            TeamTreeView team2 = new TeamTreeView()
            {
                Name = "Team4"
            };
            team2.Members.Add(new TeamMemberTreeview()
            {
                Name = "Jayar",
                Age = 50
            });
            team2.Members.Add(new TeamMemberTreeview()
            {
                Name = "Drew",
                Age = 20
            });
            teams.Add(team2);

            participantsTreeView.ItemsSource = teams;
        }

        private List<Team> convertToTeam(List<ITeam> list)
        {
            List<Team> result = new List<Team>();

            foreach(ITeam team in list)
            {
                List<TeamMember> teamMembers = new List<TeamMember>();
                int teamId = team.TeamId;
                List<ITeamMember> iTeamMembers = source.getTeamMembersByTeamId(teamId);
                foreach (ITeamMember teamMember in iTeamMembers)
                {
                    IPerson person = source.getPerson(teamMember.PersonId);
                    teamMembers.Add(new TeamMember(person.FirstName, 0));
                }

                Team newTeam = new Team(team.TeamName, teamMembers);
                result.Add(newTeam);
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

    public class TeamTreeView
    {
        public TeamTreeView()
        {
            this.Members = new ObservableCollection<TeamMemberTreeview>();
        }

        public string Name { get; set; }

        public ObservableCollection<TeamMemberTreeview> Members { get; set; }
    }

    public class TeamMemberTreeview
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
