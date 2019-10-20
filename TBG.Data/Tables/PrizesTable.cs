using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public class PrizesTable
    {
        public static IPrize Create(IPrize entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO Prizes (prize_name, prize_amount, prize_percent) VALUES (@name, @amount, @percent)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@name", entity.PrizeName);
            param.Add("@amount", entity.PrizeAmount.ToString());
            param.Add("@percent", entity.PrizePercent.ToString());

            var results = DatabaseHelper.GetNonQueryCount(query, dbConn, param);

            if (results > 0) { return entity; }
            return null;
        }

        public static IPrize Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Prizes WHERE prize_id = @Id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@Id", Id.ToString());

            using (var reader = DatabaseHelper.GetReader(query, dbConn, param))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return ConvertReader(reader);
                }
            }
            return null;
        }

        public static List<IPrize> GetAll(MySqlConnection dbConn)
        {
            List<IPrize> result = new List<IPrize>();

            string query = "SELECT * FROM Prizes";
            using (var reader = DatabaseHelper.GetReader(query, dbConn))
            {
                while (reader.Read())
                {
                    result.Add(ConvertReader(reader));
                }
            }
            return result;
        }

        public static IPrize Update(IPrize entity, MySqlConnection dbConn)
        {
            string query = "UPDATE Prizes SET prize_name = @name, prize_amount = @amount, prize_percent = @percent WHERE prize_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@name", entity.PrizeName.ToString());
            param.Add("@amount", entity.PrizeAmount.ToString());
            param.Add("@percent", entity.PrizePercent.ToString());
            param.Add("@id", entity.PrizeId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static IPrize Delete(IPrize entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Prizes WHERE prize_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.PrizeId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        private static IPrize ConvertReader(MySqlDataReader reader)
        {
            return new Prize()
            {
                PrizeId = int.Parse(reader["prize_id"].ToString()),
                PrizeName = reader["prize_name"].ToString(),
                PrizeAmount = decimal.Parse(reader["prize_amount"].ToString()),
                PrizePercent = decimal.Parse(reader["prize_percent"].ToString())
            };
        }
    }
}
