using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Classes;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private IProvider source;
        private ITournamentController tournamentController;
        private IUser user;
        private List<ITournament> allTournaments;

        public Dashboard(IUser user)
        {
            InitializeComponent();
            this.user = user;
            this.source = ApplicationController.getProvider();
            tournamentController = ApplicationController.getTournamentController();
            var tournamentTypes = source.getTournamentTypes();

            BuildTournamentList();
            foreach (var tournamentType in tournamentTypes)
            {
                this.typeFilter.Items.Add(new ListBoxItem {
                    Content = new Label {
                        Content = tournamentType.TournamentTypeName
                    }
                });
            }

            //Logged in as guest -> don't allow them to create a tournament
            if (user == null)   
            {
                createTournamentButton.Visibility = Visibility.Hidden;
            }
        }

        private void BuildTournamentList()
        {
            tournamentList.Items.Clear();
            allTournaments = source.getAllTournaments();
            foreach (var tournament in allTournaments)
            {
                List<ITeam> teams = source.getTeamsFromTournamentId(tournament.TournamentId);
                var item = new TournamentListBoxItem()
                {
                    Name = tournament.TournamentName,
                    TournamentId = tournament.TournamentId,
                    TournamentTypeId = tournament.TournamentTypeId,
                    Teams = teams,
                    PrizePool = tournament.TotalPrizePool
                };

                tournamentList.Items.Add(item);
            }
        }

        public void Load_Tournament(object sender, RoutedEventArgs e)
        {
            //Only load tournament if one is selected
            if (tournamentList.SelectedIndex == -1) return;
            var selectedItem = tournamentList.SelectedItem as TournamentListBoxItem;
            var selectedTournamentId = allTournaments.Where(x => x.TournamentId == selectedItem.TournamentId).First().TournamentId;
            var selectedTournament = source.getTournament(selectedTournamentId);
            selectedTournament = tournamentController.rebuildTournament(selectedTournament);
            TournamentViewUI viewUI = new TournamentViewUI(selectedTournament);
            viewUI.Show();
        }

        public void Delete_Tournament(object sender, RoutedEventArgs e)
        {
            //Probably should add a confirmation to this
            if (tournamentList.SelectedIndex == -1) return;
            var selectedItem = tournamentList.SelectedItem as TournamentListBoxItem;
            var selectedTournamentId = allTournaments.Where(x => x.TournamentId == selectedItem.TournamentId).First().TournamentId;

            var deletedTournament = source.deleteTournament(selectedTournamentId);

            BuildTournamentList();
            messageBox.Text = "Successfully deleted tournament";
        }

        private void CreateTournament_Click(object sender, RoutedEventArgs e)
        {
            CreateTournament newTournament = new CreateTournament(user);
            newTournament.Show();
        }

        private void NameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(tournamentList.Items);
            view.Filter = TournamentNameFilter;
            view.Refresh();
        }

        private void TypeFilter_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (typeFilter.SelectedIndex != 0)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(tournamentList.Items);
                view.Filter = TournamentTypeFilter;
                view.Refresh();
            }
            else 
            {
                if (tournamentList != null) //Because this method is called once before tournamentList is initialized
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(tournamentList.Items);
                    view.Filter = null;
                    view.Refresh();
                }
            }
        }

        private void PlayerFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(tournamentList.Items);
            if (string.IsNullOrEmpty(playerFilter.Text))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = TeamNameFilter;
            }
            view.Refresh();
        }

        private void PrizeFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(tournamentList.Items);
            if (string.IsNullOrEmpty(prizeFilter.Text))
            {
                view.Filter = null;
            }
            else
            {
                if (Double.TryParse(prizeFilter.Text, out double i))
                {
                    messageBox.Text = "";
                    view.Filter = PrizeFilter;
                }
                else
                {
                    messageBox.Text = "Prize Filter requires a number";
                }
            }
            view.Refresh();
        }

        private bool TournamentNameFilter(object item)
        {
            TournamentListBoxItem tournament = item as TournamentListBoxItem;
            return tournament.Name.ToLower().Contains(nameSearch.Text.ToLower());
        }

        private bool TournamentTypeFilter(object item)
        {
            TournamentListBoxItem tournament = item as TournamentListBoxItem;
            var thisTournament = allTournaments.Find(x => x.TournamentId == tournament.TournamentId);
            if (thisTournament.TournamentTypeId == typeFilter.SelectedIndex)
            {
                return true;
            }
            return false;
        }

        private bool TeamNameFilter(object item)
        {
            TournamentListBoxItem tournament = item as TournamentListBoxItem;
            if (tournament.Teams.Find(x => x.TeamName.ToLower().Contains(playerFilter.Text.ToLower())) != null)
            {
                return true;
            }
            return false;
        }

        private bool PrizeFilter(object item)
        {
            TournamentListBoxItem tournament = item as TournamentListBoxItem;
            if (Double.Parse(prizeFilter.Text) <= tournament.PrizePool)
            {
                return true;
            }
            return false;
        }

    }
}
