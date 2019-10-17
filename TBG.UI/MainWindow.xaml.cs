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
        private IProvider source;
        private IController business;
        private IDatabaseLogin dbLogin;
        public MainWindow()
        {
            InitializeComponent();
            source = ApplicationController.GetProvider();
            business = ApplicationController.GetController();
            dbLogin = ApplicationController.GetDatabaseLogin();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Log a user in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string user = userNameTextBox.Text;
            string pass = passwordTextBox.Password;
            //check if eligible to be logged in 
            if (dbLogin.Validate(user, pass))
            {
                displayMessage.Text = String.Empty;
                SetDisplayColors(new SolidColorBrush(Colors.Green));
                dbLogin.UpdateLastLogin(user);
                //Start Application 
                Dashboard dB = new Dashboard(source, business);
                dB.Show();
                this.Close();
            }
            else
            {
                displayMessage.Text = "Invalid username or password";
                SetDisplayColors(new SolidColorBrush(Colors.Red));
            }
        }

        /// <summary>
        /// Add a user to the database..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string user = userNameTextBox.Text;
            string pass = passwordTextBox.Password;
            if (!dbLogin.ValidateUserName(user))
            {
                displayMessage.Text = "Username already exists";
                SetDisplayColors(new SolidColorBrush(Colors.Red));
            }
            else
            {
                SetDisplayColors(new SolidColorBrush(Colors.Green));

                bool success = dbLogin.CreateUser(user, pass);
                if (success)
                {
                    displayMessage.Text = "Created new user " + user;
                } 
                else
                {
                    displayMessage.Text = "Error creating new user";
                }
            }

        }

        private void SetDisplayColors(SolidColorBrush pColor)
        {
            userBorder.Background = pColor;
            passwordBorder.Background = pColor;
            loginIcon.Foreground = pColor;
            passwordIcon.Foreground = pColor;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GuestLogin_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Start Application but with read only permission
            Dashboard dB = new Dashboard(source, business);
            dB.Show();
            this.Close();
        }
    }
}
