using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class Login : ILogin, IDisposable
    {
        private static string connString = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;
        private static MySqlConnection dbConn;

        public Login()
        {
            dbConn = new MySqlConnection(connString);
            dbConn.Open();
        }

        public bool Validate(string user, string pass)
        {
            //check user name empty 
            if (string.IsNullOrEmpty(user))
            {
                return false;
            }
            //check password empty
            if (string.IsNullOrEmpty(pass))
            {
                return false;
            }
            string query = string.Format("SELECT * FROM `Users` WHERE `user_name` LIKE '{0}' AND `password` LIKE '{1}'", user, pass);
            //Validate user exists and password is correct
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        public void Dispose()
        {
            if (dbConn != null)
            {
                dbConn.Close();
            }
        }
    }

}
