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

        /// <summary>
        /// Return true if the passed username and password match a row in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if the username doesn't match a username in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ValidateUserName(string user)
        {
            string query = string.Format("SELECT * FROM `Users` WHERE `user_name` LIKE '{0}'", user);
            //Validate user exists
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Adds a user to Users table in database. Returns true if successful.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool CreateUser(string user, string pass)
        {
            string query = string.Format("INSERT INTO `team4`.`Users` (`user_id`, `user_name`, `password`, `active`, `admin`) VALUES (NULL, '{0}', '{1}', '1', '0')", user, pass);
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                int rowsEffected = cmd.ExecuteNonQuery();
                if (rowsEffected > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the last_login column for the passed user.
        /// </summary>
        /// <param name="user"></param>
        public void updateLastLogin(string user)
        {
            string query = string.Format("SELECT * FROM `Users` WHERE `user_name` LIKE '{0}'", user);
            //Validate user exists
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Close();
                        DateTime myDateTime = DateTime.Now;
                        string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string updateQuery = string.Format("UPDATE `team4`.`Users` SET `last_login` = '{0}' WHERE `Users`.`user_name` = '{1}'", sqlFormattedDate, user);
                        cmd.CommandText = updateQuery;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            
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
