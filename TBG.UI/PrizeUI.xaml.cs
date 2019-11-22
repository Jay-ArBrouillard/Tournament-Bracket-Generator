using System;
using System.Collections.Generic;
using System.Windows;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Classes;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for PrizeUI.xaml
    /// 
    /// ListBox for displaying all prizes ***
    /// 
    /// </summary>
    public partial class PrizeUI : Window
    {

        private IPrizeController prizeController;
        private IProvider source;
        private List<IPrize> allPrizes;
        private CreateTournament tournament;

        public PrizeUI(CreateTournament tournament)
        {
            InitializeComponent();
            prizeController = ApplicationController.getPrizeController();
            source = ApplicationController.getProvider();
            allPrizes = new List<IPrize>();
            readPrizes();
            this.tournament = tournament;
        }

        private void createPrizeBtn_Click(object sender, RoutedEventArgs e)
        {
            IPrize prize = prizeController.ValidatePrize(placeNameTxtBox.Text, prizeAmtTxtBox.Text);

            if (prize != null)
            {
                //Creates a new prize with valid information
                IPrize newPrize = source.createPrize(prize);
                readPrizes();
                List<ITournamentPrize> prizesInTournment = tournament.prizesInTournament;
                List<IPrize> allPrizes = tournament.prizes;
                allPrizes.Add(newPrize);
                prizesInTournment.Add(new TournamentPrize()
                {
                    
                });
                tournament.prizeComboBox.ItemsSource = allPrizes;
                tournament.prizesListBox.ItemsSource = prizesInTournment;
                tournament.prizeComboBox.Items.Refresh();
                tournament.prizesListBox.Items.Refresh();
                errorMsgLbl.Visibility = Visibility.Hidden;
            }
            else
            {
                errorMsgLbl.Visibility = Visibility.Visible;
            }
        }

        private void readPrizes()
        {
            allPrizes = source.getAllPrizes();
            prizeDisplayListBox.ItemsSource = allPrizes;
        }


    }
}