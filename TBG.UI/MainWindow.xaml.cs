using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TBG.Core.Interfaces;
using TBG.Driver;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProvider source;
        private readonly IController business;
        private readonly ILogin login;
        public MainWindow()
        {
            InitializeComponent();
            source = ApplicationController.getProvider();
            business = ApplicationController.getController();
            login = ApplicationController.getLogin();
        }

        private void Login_Event_Handler(object sender, RoutedEventArgs e)
        {
            string user = userNameTextBox.Text;
            string pass = passwordTextBox.Password;
            //check if eligible to be logged in 
            if (login.Validate(user, pass))
            {
                MessageBox.Show("You are logged in successfully");
                //Do something
            }
        }
    }
}
