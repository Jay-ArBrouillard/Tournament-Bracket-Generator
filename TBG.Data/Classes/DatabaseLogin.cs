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
            string query = "INSERT INTO `team4`.`Users` (`user_id`, `user_name`, `password`) VALUES (NULL, @USER, @PASS)";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@USER", user);
                cmd.Parameters.AddWithValue("@PASS", pass);

                int rowsEffected = cmd.ExecuteNonQuery();
                if (rowsEffected > 0) { return true; }
            }

            return false;
        }

        public void UpdateLastLogin(string user)
        {
            string query = "SELECT * FROM `Users` WHERE `user_name` LIKE @USER1";
            //Validate user exists
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@USER1", user);
                cmd.ExecuteNonQuery();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Close();
                        DateTime myDateTime = DateTime.Now;
                        string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string updateQuery = "UPDATE `team4`.`Users` SET `last_login` = @DATETIME WHERE `Users`.`user_name` = @USER";
                        cmd.CommandText = updateQuery;

                        cmd.Parameters.AddWithValue("@USER", user);
                        cmd.Parameters.AddWithValue("@DATETIME", sqlFormattedDate);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public bool Validate(string user, string pass)
        {
            //check user name empty 
            if (string.IsNullOrEmpty(user)) { return false; }
            //check password empty
            if (string.IsNullOrEmpty(pass)) { return false; }

            string query = "SELECT * FROM `Users` WHERE `user_name` LIKE @USER AND `password` LIKE @PASS";
            //Validate user exists and password is correct
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@USER", user);
                cmd.Parameters.AddWithValue("@PASS", pass);

                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) { return true; }
                }
            }

            return false;
        }

        public bool ValidateUserName(string user)
        {
            //check user name empty 
            if (string.IsNullOrEmpty(user)) { return false; }

            string query = "SELECT * FROM `Users` WHERE `user_name` LIKE @USER";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@USER", user);
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) { return false; }
                }
            }

            return true;
        }

        public void Dispose()
        {
            if (dbConn != null) { dbConn.Close(); }
        }
    }
}
