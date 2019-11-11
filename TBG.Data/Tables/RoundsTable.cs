using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public class RoundsTable
    {
        public static IRound Create(IRound entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO Rounds (tournament_id, round_num) VALUES (@id, @round)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.TournamentId.ToString());
            param.Add("@round", entity.RoundNum.ToString());

            var results = DatabaseHelper.GetNonQueryCount(query, dbConn, param, out int primaryKey);
            if (results > 0)
            {
                entity.RoundId = primaryKey;
                return entity;
            }
            return null;
        }

        public static IRound Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Rounds WHERE prize_id = @Id";
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

        public static List<IRound> GetAll(MySqlConnection dbConn)
        {
            List<IRound> result = new List<IRound>();

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

        public static IRound Update(IRound entity, MySqlConnection dbConn)
        {/*
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
            }*/

            return null;
        }

        private static IRound ConvertReader(MySqlDataReader reader)
        {
            return new Round()
            {
                TournamentId = int.Parse(reader["tournament_id"].ToString()),
                RoundId = int.Parse(reader["round_num"].ToString())
            };
        }
    }
}
