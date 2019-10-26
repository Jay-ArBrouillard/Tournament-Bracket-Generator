using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Classes;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for Team.xaml
    /// </summary>
    public partial class TeamWindow : Window
    {
        private IProvider source;
        private IPersonController personController;
        private ITeamController teamController;
        private List<IPerson> personList;   //List of people in database
        private List<IPerson> selectedPersons; //List of people to create a new team with
        private CreateTournament tournament;

        public TeamWindow(CreateTournament tournament)
        {
            InitializeComponent();
            source = ApplicationController.getProvider();
            personController = ApplicationController.getPersonController();
            teamController = ApplicationController.getTeamController();
            this.tournament = tournament;
            personList = source.getPeople();
            personList.Sort((x,y) =>x.FirstName.CompareTo(y.FirstName));
            selectedPersons = new List<IPerson>();
            selectionListBox.ItemsSource = personList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Currently empty
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            selectedPersons.Clear();
            if (string.IsNullOrEmpty(searchBox.Text) == false)
            {
                //Maintain already selected items

                foreach (IPerson p in selectionListBox.SelectedItems)
                {
                    selectedPersons.Add(p);
                }

                foreach (IPerson p in personList)
                {
                    if (p.FirstName.Contains(searchBox.Text) && !selectionListBox.SelectedItems.Contains(p))
                    {
                        selectedPersons.Add(p);
                    }
                }

                selectedPersons.Sort();
                selectionListBox.ItemsSource = selectedPersons;
            }
            else if (searchBox.Text == "")
            {
                selectionListBox.ItemsSource = personList;
            }

            selectionListBox.Items.Refresh();

        }

        private void ConfirmSelection_Click(object sender, RoutedEventArgs e)
        {
            foreach (IPerson item in selectionListBox.SelectedItems)
            {
                if (!displayListBox.Items.Contains(item))
                {
                    displayListBox.Items.Add(item);
                    selectedPersons.Add(item);
                }
            }

            displayListBox.Items.Refresh();
        }

        private void Deletion_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < displayListBox.SelectedItems.Count; i++)
            {
                selectedPersons.Remove((IPerson)displayListBox.SelectedItem);
                displayListBox.Items.Remove(displayListBox.SelectedItem);
                i--;
            }
        }

        private void Remove_All_Click(object sender, RoutedEventArgs e)
        {
            displayListBox.Items.Clear();
            selectedPersons.Clear();
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstNameText.Text;
            string lastName = lastNameText.Text;
            string email = emailText.Text;
            string phone = phoneNumberText.Text;
            bool valid = personController.validateWinLoss(winsText.Text, lossesText.Text);

            if (valid)
            {
                IPerson newPerson = new Person(firstName, lastName, email, phone, int.Parse(winsText.Text), int.Parse(lossesText.Text));
                bool validate = personController.validatePerson(newPerson);

                if (validate)
                {
                    if (source.createPerson(newPerson) != null)
                    {
                        SetDisplayColors(new SolidColorBrush(Colors.Green));

                        //Update views on TeamWindow
                        personList.Add(newPerson);
                        displayListBox.Items.Add(newPerson);
                        selectedPersons.Add(newPerson);
                        selectionListBox.ItemsSource = personList;
                        selectionListBox.Items.Refresh();
                        displayListBox.Items.Refresh();
                    }
                    else
                    {
                        SetDisplayColors(new SolidColorBrush(Colors.Red));
                    }
                }
                else
                {
                    SetDisplayColors(new SolidColorBrush(Colors.Red));
                }
            }
            else
            {
                SetDisplayColors(new SolidColorBrush(Colors.Red));
            }


        }
        private void SetDisplayColors(SolidColorBrush pColor)
        {
            firstNameText.BorderBrush = pColor;
            lastNameText.BorderBrush = pColor;
            emailText.BorderBrush = pColor;
            phoneNumberText.BorderBrush = pColor;
            winsText.BorderBrush = pColor;
            lossesText.BorderBrush = pColor;
        }

        private void CreateTeam_Click(object sender, RoutedEventArgs e)
        {
            string teamName = teamNameTextBox.Text;

            ITeam newTeam = new Team(teamName, selectedPersons);
            ITeam existingTeam = source.getTeam(teamName);
            bool validate = teamController.validateTeam(newTeam, existingTeam);

            if (validate)
            {
                ITeam createdTeam = source.createTeam(newTeam);

                //Update Teams/Players on tournament Screen
                List<TournamentEntryView> teams = tournament.teamsInTournament;
                TournamentEntryView convertedTeam = tournament.convertToTeam(new List<ITeam>() { createdTeam })[0];
                ObservableCollection<TeamMember> teamMembers = new ObservableCollection<TeamMember>();
                foreach (ITeamMember teamMember in source.getTeamMembersByTeamId(createdTeam.TeamId))
                {
                    IPerson person = source.getPerson(teamMember.PersonId);
                    teamMembers.Add(new TeamMember()
                    {
                        PersonId = person.PersonId,
                        TeamName = newTeam.TeamName,
                        FirstName = person.FirstName,
                        LastName = person.LastName
                    });
                }

                teams.Add(convertedTeam);
                teams[teams.Count - 1].Members = teamMembers;
                
                tournament.participantsTreeView.ItemsSource = teams;
                tournament.participantsTreeView.Items.Refresh();

                //Update Select Teams
                List<ITeam> allTeams = tournament.teams;
                allTeams.Add(createdTeam);
                tournament.selectionListBox.ItemsSource = allTeams;
                tournament.selectionListBox.Items.Refresh();
            }

        }


    }

}
