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
    public class PrizesTable
    {

        private static string connString = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;

        public static List<IPrize> GetAll()
        {
            List<IPrize> result = new List<IPrize>();
            MySqlConnection dbConn = new MySqlConnection(connString);
            dbConn.Open();

            string query = "SELECT * FROM Prizes";
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Prize found = new Prize() {
                            PrizeId = Int32.Parse(reader["prize_id"].ToString()),
                            PrizeName = reader["prize_name"].ToString(),
                            PrizeAmount = Decimal.Parse(reader["prize_amount"].ToString()),
                            PrizePercent = Decimal.Parse(reader["prize_percent"].ToString()),
                        };
                        result.Add(found);
                    }
                }
            }
            return result;
        }

    }
}
