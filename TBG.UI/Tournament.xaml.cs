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
            List<TeamTreeView> teams = convertToTeam(source.getAllTeams());
            /*List<TeamTreeView> teams = new List<TeamTreeView>();
            
            TeamTreeView team1 = new TeamTreeView()
            {
                TeamName = "TankTop Team"
            };
            team1.Members.Add(new TeamMemberTreeview()
            {
                FirstName = "Johnny Smith",
                LastName = 21
            });
            team1.Members.Add(new TeamMemberTreeview()
            {
                FirstName = "Gerald Smith",
                LastName = 31
            });
            teams.Add(team1);
            TeamTreeView team2 = new TeamTreeView()
            {
                TeamName = "Team4"
            };
            team2.Members.Add(new TeamMemberTreeview()
            {
                FirstName = "Jayar",
                LastName = 50
            });
            team2.Members.Add(new TeamMemberTreeview()
            {
                FirstName = "Drew",
                LastName = 20
            });
            teams.Add(team2);
            */

            participantsTreeView.ItemsSource = teams;
        }

        private List<TeamTreeView> convertToTeam(List<ITeam> list)
        {
            List<TeamTreeView> result = new List<TeamTreeView>();

            foreach(ITeam team in list)
            {
                ObservableCollection<TeamMemberTreeview> teamMembers = new ObservableCollection<TeamMemberTreeview>();
                int teamId = team.TeamId;
                List<ITeamMember> iTeamMembers = source.getTeamMembersByTeamId(teamId);
                foreach (ITeamMember teamMember in iTeamMembers)
                {
                    IPerson person = source.getPerson(teamMember.PersonId);
                    teamMembers.Add(new TeamMemberTreeview()
                    {
                        FirstName = person.FirstName,
                        LastName = person.LastName
                    });
                }

                result.Add(new TeamTreeView(){
                    TeamName = team.TeamName,
                    Members = teamMembers
                });
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

        public string TeamName { get; set; }

        public ObservableCollection<TeamMemberTreeview> Members { get; set; }
    }

    public class TeamMemberTreeview
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
