using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using TBG.Core.Interfaces;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public class TournamentTypeTable
    {
        private static string connString = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;

        public static List<ITournamentType> GetAll()
        {
            List<ITournamentType> result = new List<ITournamentType>();
            MySqlConnection dbConn = new MySqlConnection(connString);
            dbConn.Open();

            string query = "SELECT * FROM TournamentTypes";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TournamentType found = new TournamentType()
                        {
                            TournamentTypeId = Int32.Parse(reader["tournament_type_id"].ToString()),
                            TournamentTypeName = reader["tournament_type_name"].ToString()
                        };
                        result.Add(found);
                    }
                }
            }
            return result;
        }
    }
}
