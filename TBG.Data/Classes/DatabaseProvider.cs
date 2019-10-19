using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using TBG.Core.Interfaces;
using TBG.Data.Entities;
using TBG.Data.Tables;

namespace TBG.Data.Classes
{
    public class DatabaseProvider : IProvider, IDisposable
    {
        private static string connString = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;
        private static MySqlConnection dbConn;

        public DatabaseProvider()
        {
            dbConn = new MySqlConnection(connString);
            dbConn.Open();
        }

        public void Dispose()
        {
            if (dbConn != null) { dbConn.Close(); }
        }

        public bool createTournament(ITournament entry)
        {
            //Create Tournament Record
            //Create Cross Reference Records For Tournament/Team/Player
            return true;
        }

        public bool createTeam(ITeam entry)
        {
            //Create Team Record
            return true;
        }

        public bool createPerson(IPerson entry)
        {
            //Create Person Record
            return true;
        }

        ////////////////////////////USER METHODS////////////////////////////////////////
        public bool createUser(IUser thisUser)
        {
            string userName = thisUser.UserName;
            string password = thisUser.Password; 

            string query = "INSERT INTO `team4`.`Users` (`user_id`, `user_name`, `password`) VALUES (NULL, @USER, @PASS)";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@USER", userName);
                cmd.Parameters.AddWithValue("@PASS", MD5.Encrypt(password, true));

                int rowsEffected = cmd.ExecuteNonQuery();
                if (rowsEffected > 0) { return true; }
            }

            return false;
        }

        public IUser getUser(string userName)
        {
            if (string.IsNullOrEmpty(userName)) { return null; }

            string query = "SELECT * FROM `Users` WHERE `user_name` LIKE @USER";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@USER", userName);
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        User found = new User(reader["user_name"].ToString(), MD5.Decrypt(reader["password"].ToString(), true));
                        return found;
                    }
                }
            }

            return null;
        }

        public bool updateLoginTime(IUser thisUser)
        {
            string userName = thisUser.UserName;
            string password = thisUser.Password;
            if (string.IsNullOrEmpty(userName)) { return false; }

            string query = "SELECT * FROM `Users` WHERE `user_name` LIKE @USER AND `password` LIKE @PASS";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@USER", userName);
                cmd.Parameters.AddWithValue("@PASS", password);
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        string PK = reader["user_id"].ToString();
                        reader.Close();
                        DateTime myDateTime = DateTime.Now;
                        string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string updateQuery = "UPDATE `team4`.`Users` SET `last_login` = @DATETIME WHERE `Users`.`user_id` = @ID";
                        cmd.CommandText = updateQuery;

                        cmd.Parameters.AddWithValue("@ID", PK);
                        cmd.Parameters.AddWithValue("@DATETIME", sqlFormattedDate);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }

            return false;

        }

        public List<ITournament> GetAllTournaments()
        {
            return TournamentTable.GetAll(); ;
        }

        public List<ITournamentType> GetTournamentTypes()
        {
            return TournamentTypeTable.GetAll();
        }
    }
}
