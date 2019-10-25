﻿using System;
using System.Collections.Generic;
using System.Windows;
using TBG.Core.Interfaces;
using TBG.Driver;
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

        private IPrizeController controller;
        private IProvider source;
        private List<IPrize> allPrizes;
        private Tournament tournament;

        public PrizeUI(Tournament tournament)
        {
            InitializeComponent();
            controller = ApplicationController.GetPrizeController();
            source = ApplicationController.GetProvider();
            allPrizes = new List<IPrize>();
            readPrizes();
            this.tournament = tournament;
        }

        private void createPrizeBtn_Click(object sender, RoutedEventArgs e)
        {
            IPrize prize = controller.ValidatePrize(placeNameTxtBox.Text,  prizeAmtTxtBox.Text);

            if (prize != null)
            {
                //Creates a new prize with valid information
                IPrize newPrize = source.createPrize(prize);
                readPrizes();
                List<IPrize> prizesInTournment = tournament.prizesInTournament;
                List<IPrize> allPrizes = tournament.prizes;
                allPrizes.Add(newPrize);
                prizesInTournment.Add(newPrize);
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
            allPrizes = source.GetAllPrizes();
            prizeDisplayListBox.ItemsSource = allPrizes;
        }


    }
}