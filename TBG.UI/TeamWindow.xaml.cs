using System;
using System.Collections.Generic;
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

        public TeamWindow()
        {
            InitializeComponent();
            source = ApplicationController.GetProvider();
            personController = ApplicationController.getPersonController();
            teamController = ApplicationController.getTeamController();

            personList = source.getPeople();
            selectedPersons = new List<IPerson>();
            selectionListBox.ItemsSource = personList;
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchBox.Text) == false)
            {
                foreach (IPerson p in personList)
                {
                    if (p.FirstName.Contains(searchBox.Text))
                    {
                        selectedPersons.Add(p);
                    }
                    else
                    {
                        selectedPersons.Remove(p);
                    }
                }

                selectionListBox.ItemsSource = selectedPersons;
            }
            else if (searchBox.Text == "")
            {
                selectionListBox.ItemsSource = personList;
            }

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

            IPerson newPerson = new Person(firstName, lastName, email, phone);
            bool validate = personController.validatePerson(newPerson);

            if (validate)
            { 
                if (source.createPerson(newPerson) != null)
                {
                    SetDisplayColors(new SolidColorBrush(Colors.Green));

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
        private void SetDisplayColors(SolidColorBrush pColor)
        {
            firstNameText.BorderBrush = pColor;
            lastNameText.BorderBrush = pColor;
            emailText.BorderBrush = pColor;
            phoneNumberText.BorderBrush = pColor;
        }

        private void CreateTeam_Click(object sender, RoutedEventArgs e)
        {
            string teamName = teamNameTextBox.Text;

            ITeam newTeam = new Team(teamName, selectedPersons);
            ITeam existingTeam = source.getTeam(teamName);
            bool validate =  teamController.validateTeam(newTeam, existingTeam);

            if (validate)
            {
                //ITeam newTeam = new Team(teamName, selectedPersons);
                source.createTeam(newTeam);
                /*if (success)
                {
                    //Message box for now. UI visuals will handle later
                    MessageBox.Show(this, "Successfully created new team: " + teamName);
                }
                else
                {
                    //Message box for now. UI visuals will handle later
                    MessageBox.Show(this, "Error creating new team");
                }*/
            }

            //Go back to tournament screen after creating team
            Tournament tournament = new Tournament();
            tournament.Show();
            this.Close();
        }
    }

}
