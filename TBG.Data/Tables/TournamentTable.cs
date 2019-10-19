using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public static class TournamentTable
    {
        private static string connString = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;

        public static Tournament GetTournament(int Id)
        {
            MySqlConnection dbConn = new MySqlConnection(connString);
            dbConn.Open();

            string query = "SELECT * FROM `Tournaments` WHERE `tournament_id` = @Id";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        Tournament found = new Tournament() {
                            TournamentId = Int32.Parse(reader["tournament_id"].ToString()),
                            UserId = Int32.Parse(reader["tournament_id"].ToString()),
                            TournamentName = reader["tournament_name"].ToString(),
                            EntryFee = Decimal.Parse(reader["entry_fee"].ToString()),
                            TotalPrizePool = Double.Parse(reader["total_prize_pool"].ToString()),
                            TournamentTypeId = Int32.Parse(reader["tournament_type_id"].ToString())
                        };
                        return found;
                    }
                }
            }
            return new Tournament();
        }

        public static List<ITournament> GetAll()
        {
            List<ITournament> result = new List<ITournament>();
            MySqlConnection dbConn = new MySqlConnection(connString);
            dbConn.Open();

            string query = "SELECT * FROM Tournaments";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Tournament found = new Tournament()
                        {
                            TournamentId = Int32.Parse(reader["tournament_id"].ToString()),
                            UserId = Int32.Parse(reader["tournament_id"].ToString()),
                            TournamentName = reader["tournament_name"].ToString(),
                            EntryFee = Decimal.Parse(reader["entry_fee"].ToString()),
                            TotalPrizePool = Double.Parse(reader["total_prize_pool"].ToString()),
                            TournamentTypeId = Int32.Parse(reader["tournament_type_id"].ToString())
                        };
                        result.Add(found);
                    }
                }
            }
            return result;
        }
    }
}
