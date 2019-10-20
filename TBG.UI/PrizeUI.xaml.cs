using System;
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

        IPrizeController controller;
        IProvider source;
        List<IPrize> allPrizes;

        public PrizeUI()
        {
            InitializeComponent();
            controller = ApplicationController.GetPrizeController();
            source = ApplicationController.GetProvider();

            readPrizes();
        }

        private void createPrizeBtn_Click(object sender, RoutedEventArgs e)
        {
            IPrize prize = controller.ValidatePrize(placeNameTxtBox.Text,  prizeAmtTxtBox.Text);

            if (prize != null)
            {
                //Creates a new prize with valid information
                source.createPrize(prize);
                readPrizes();
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