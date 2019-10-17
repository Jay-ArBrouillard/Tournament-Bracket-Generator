using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace TBG.Data.Classes
{
    public class DatabaseLogin : IDatabaseLogin, IDisposable
    {
        private static string connString = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;
        private static MySqlConnection dbConn;

        public DatabaseLogin()
        {
            dbConn = new MySqlConnection(connString);
            dbConn.Open();
        }

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

        public void UpdateLastLogin(string user)
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

        public void Dispose()
        {
            if (dbConn != null)
            {
                dbConn.Close();
            }
        }
    }
}
