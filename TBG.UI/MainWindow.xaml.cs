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
using TBG.UI.Classes;
using TBG.UI.Models;

namespace TBG.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IProvider source;
        private ILoginController loginController;

        public MainWindow()
        {
            InitializeComponent();
            source = ApplicationController.getProvider();

            if (!source.sourceActive)
            {
                MessageBox.Show("Error Connecting to Data", "TBG", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            loginController = ApplicationController.getLoginController();
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

            var userExisting = source.getUser(user);
            var validatedUser = loginController.validateLogin(user, pass, userExisting);

            if (validatedUser != null)
            {
                source.updateUser(validatedUser);
                //Visuals
                displayMessage.Text = String.Empty;
                SetDisplayColors(new SolidColorBrush(Colors.Green));
                
                //Start Application 
                Dashboard dB = new Dashboard(validatedUser);
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

            if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(pass))
            {
                SetDisplayColors(new SolidColorBrush(Colors.Red));
                displayMessage.Text = "Username and Password\ncan't be empty";
                return;
            }

            var userExisting = source.getUser(user);
            var validatedUser = loginController.validateRegister(user, pass, userExisting);

            if (validatedUser != null)
            {
                SetDisplayColors(new SolidColorBrush(Colors.Green));

                if (source.createUser(validatedUser) != null)
                {
                    displayMessage.Text = "Created new user " + user;
                }
                else
                {
                    displayMessage.Text = "Error creating new user";
                }

            }
            else
            {
                displayMessage.Text = "Username already exists";
                SetDisplayColors(new SolidColorBrush(Colors.Red));
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
            Dashboard dB = new Dashboard(null);
            dB.Show();
            this.Close();
        }
        
        private void UserNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (userNameTextBox.Text.Equals("Username"))
            {
                userNameTextBox.Text = "";
            }
        }

        private void UserNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(userNameTextBox.Text))
            {
                userNameTextBox.Text = "Username";
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passwordTextBox.Password.Equals("Password"))
            {
                passwordTextBox.Password = "";
            }
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordTextBox.Password))
            {
                passwordTextBox.Password = "Password";
            }
        }
    }
}
