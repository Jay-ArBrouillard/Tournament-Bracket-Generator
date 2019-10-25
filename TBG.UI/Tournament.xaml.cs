using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
        public List<ITeam> teams;
        public List<TeamTreeView> teamsInTournament;
        public List<IPrize> prizes;
        public List<IPrize> prizesInTournament;

        public Tournament()
        {
            InitializeComponent();
            source = ApplicationController.GetProvider();
            teams = source.getAllTeams();
            teamsInTournament = new List<TeamTreeView>();
            selectionListBox.ItemsSource = teams;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            prizesInTournament = new List<IPrize>();
            prizes = source.GetAllPrizes();
            prizeComboBox.ItemsSource = prizes;
        }

        public List<TeamTreeView> convertToTeam(List<ITeam> list)
        {
            List<TeamTreeView> result = new List<TeamTreeView>();

            foreach(ITeam team in list)
            {
                ObservableCollection<TeamMemberTreeview> teamMembers = new ObservableCollection<TeamMemberTreeview>();
                int teamId = team.TeamId;
                foreach (ITeamMember teamMember in source.getTeamMembersByTeamId(teamId))
                {
                    IPerson person = source.getPerson(teamMember.PersonId);
                    teamMembers.Add(new TeamMemberTreeview()
                    {
                        PersonId = person.PersonId,
                        TeamName = team.TeamName,
                        FirstName = person.FirstName,
                        LastName = person.LastName
                    }); 
                }

                result.Add(new TeamTreeView() {
                    TeamId = teamId,
                    TeamName = team.TeamName,
                    Members = teamMembers
                });
            }

            return result;
        }

        private void ConfirmSelection_Click(object sender, RoutedEventArgs e)
        {
            List<ITeam> selectedTeams = new List<ITeam>();
            foreach (ITeam team in selectionListBox.SelectedItems)
            {
                bool duplicateTeam = false;
                string teamName = team.TeamName;
                foreach (TeamTreeView view in teamsInTournament)
                {
                    if (view.TeamName.Equals(teamName))
                    {
                        duplicateTeam = true;
                        break;
                    }
                }

                if (duplicateTeam == false)
                {
                    selectedTeams.Add(team);
                }
            }

            teamsInTournament.AddRange(convertToTeam(selectedTeams));
            participantsTreeView.ItemsSource = teamsInTournament;
            participantsTreeView.Items.Refresh();

        }

        private void Create_New_Team_Click(object sender, RoutedEventArgs e)
        {
            TeamWindow teamWindow = new TeamWindow(this);
            teamWindow.Show();
        }

        private void Create_New_Prize_Click(object sender, RoutedEventArgs e)
        {
            PrizeUI prizes = new PrizeUI(this);
            prizes.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (participantsTreeView.SelectedItem is TeamTreeView selectedTeamItem)
            {
                teamsInTournament.Remove(selectedTeamItem);
                participantsTreeView.Items.Refresh();
            }
            else if (participantsTreeView.SelectedItem is TeamMemberTreeview selectedMemberItem)
            {
                //Iterate each team in the tournament
                foreach (TeamTreeView view in teamsInTournament)
                {
                    if (!selectedMemberItem.TeamName.Equals(view.TeamName))
                    {
                        continue;   //Saves unnecessary iterations
                    }

                    //Iterate each teamMember and check if their Id matches
                    foreach (TeamMemberTreeview member in view.Members)
                    {
                        if (member.PersonId == selectedMemberItem.PersonId)
                        {
                            view.Members.Remove(member);
                            break;
                        }
                    }
                }
            }

        }

        private void DeletePrizeButton_Click(object sender, RoutedEventArgs e)
        {
            List<IPrize> remove = new List<IPrize>();
            foreach (var prize in prizesListBox.SelectedItems)
            {
                remove.Add((IPrize)prize);
            }

            foreach (IPrize p in remove)
            {
                prizesInTournament.Remove(p);
            }

            prizesListBox.Items.Refresh();
        }

        private void PrizeComboBox_Selected(object sender, RoutedEventArgs e)
        {
            object selectedItem = ((ComboBox)sender).SelectedItem;
            IPrize selectedPrize = (IPrize)selectedItem;
            prizesInTournament.Add(selectedPrize);
            prizesListBox.ItemsSource = prizesInTournament;
            prizesListBox.Items.Refresh();
        }

        private void Create_Tournament_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }

    public class TeamTreeView
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public ObservableCollection<TeamMemberTreeview> Members { get; set; }
        public TeamTreeView()
        {
            this.Members = new ObservableCollection<TeamMemberTreeview>();
        }

        public override bool Equals(object obj)
        {
            return obj is TeamTreeView view &&
                   TeamId == view.TeamId;
        }

        public override int GetHashCode()
        {
            return -1532736471 + TeamId.GetHashCode();
        }
    }

    public class TeamMemberTreeview
    {
        public int PersonId { get; set; }
        public string TeamName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
