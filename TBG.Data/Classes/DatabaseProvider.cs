using MySql.Data.MySqlClient;
using System;
using System.Collections;
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

        ////////////////////////////TEAM METHODS///////////////////////////////////////
        public bool createTeam(ITeam entry)
        {
            string query = "INSERT INTO `team4`.`Teams` (`team_id`, `team_name`, `wins`, `losses`) VALUES (NULL, @TEAMNAME, @WINS, @LOSSES)";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@TEAMNAME", entry.TeamName);
                cmd.Parameters.AddWithValue("@WINS", entry.Wins);
                cmd.Parameters.AddWithValue("@LOSSES", entry.Losses);
                cmd.ExecuteNonQuery();
            }

            query = "SELECT * FROM `Teams` WHERE `team_name` LIKE @TEAMNAME";

            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@TEAMNAME", entry.TeamName);
                cmd.ExecuteNonQuery();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    int teamId = int.Parse(reader["team_id"].ToString());
                    reader.Close();

                    query = "INSERT INTO `team4`.`TeamMembers` (`person_team_id`, `team_id`, `person_id`) VALUES (NULL, @TEAMID, @PERSONID)";
                    int rowsEffected = 0;
                    //Create Row in TeamMembers for each person in the Team
                    foreach (IPerson p in entry.TeamMembers)
                    {
                        using (MySqlCommand cmd2 = new MySqlCommand(query, dbConn))
                        {
                            cmd2.Parameters.AddWithValue("@TEAMID", teamId);
                            cmd2.Parameters.AddWithValue("@PERSONID", p.PersonId);
                            rowsEffected = cmd2.ExecuteNonQuery();
                        }

                    }

                    if (rowsEffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }

        public ITeam getTeam(string teamName)
        {
            if (string.IsNullOrEmpty(teamName)) { return null; }

            string query = "SELECT * FROM `Teams` WHERE `team_name` LIKE @TEAMNAME";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@TEAMNAME", teamName);
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        ITeam found = new Team(reader["team_name"].ToString(),
                                               int.Parse(reader["wins"].ToString()),
                                               int.Parse(reader["losses"].ToString()));
                        return found;
                    }
                }
            }

            return null;
        }

        //////////////////////////END TEAM METHODS//////////////////////////////////////

        //////////////////////////PERSON METHODS////////////////////////////////////////
        public bool createPerson(IPerson entry)
        {
            string query = "INSERT INTO `team4`.`Persons` (`person_id`, `first_name`, `last_name`, `email`, `phone`, `wins`, `losses`) VALUES (NULL, @FIRST, @LAST, @EMAIL, @PHONE, '0', '0')";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@FIRST", entry.FirstName);
                cmd.Parameters.AddWithValue("@LAST", entry.LastName);
                cmd.Parameters.AddWithValue("@EMAIL", entry.Email);
                cmd.Parameters.AddWithValue("@PHONE", entry.Phone);

                int rowsEffected = cmd.ExecuteNonQuery();
                if (rowsEffected > 0) { return true; }
            }

            return false;
        }

        public List<IPerson> getPeople()
        {
            List<IPerson> personList = new List<IPerson>();

            string query = "SELECT * FROM `Persons`";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IPerson person = new Person(int.Parse(reader["person_id"].ToString()), reader["first_name"].ToString(), reader["last_name"].ToString(),
                            reader["email"].ToString(), reader["phone"].ToString(), int.Parse(reader["wins"].ToString()), int.Parse(reader["losses"].ToString()));
                        personList.Add(person);
                    }
                }
            }

            return personList;
        }


        ///////////////////////// END PERSON METHODS////////////////////////////


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

        ////////////////////////////END USER METHODS////////////////////////////////////////

        ////////////////////////////PRIZE METHODS////////////////////////////////////////

        public bool createPrize(IPrize prize)
        {
            string query = "INSERT INTO `team4`.`Prizes` (`prize_id`, `prize_name`, `prize_amount`, `prize_percent`) VALUES (NULL, @PR_NAME, @PRAMT, @PRPERC)";

            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@PR_NAME", prize.PrizeName);
                cmd.Parameters.AddWithValue("@PRAMT", prize.PrizeAmount);
                cmd.Parameters.AddWithValue("@PRPERC", prize.PrizePercent);

                int rowsEffected = cmd.ExecuteNonQuery();
                if (rowsEffected > 0) { return true; }
            }

            return false;
        }

        public List<IPrize> GetAllPrizes()
        {
            return PrizesTable.GetAll();
        }
    }
}
