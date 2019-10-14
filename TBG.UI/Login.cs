using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TBG.UI
{
    public static class Login
    {
        //method to check if eligible to be logged in 
        //Default username: "team4", password: "welcome1"
        public static bool Validate(string user, string pass)
        {
            //check user name empty 
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Enter a user name!");
                return false;
            }
            //check password empty
            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Enter a password!");
                return false;
            }

            //Check username
            if (!user.Equals("team4"))
            {
                MessageBox.Show("Username is incorrect!");
                return false;
            }

            //Check password
            if (!pass.Equals("welcome1"))
            {
                MessageBox.Show("Password is correct!");
                return false;
            }

            return true;

        }
    }

}
