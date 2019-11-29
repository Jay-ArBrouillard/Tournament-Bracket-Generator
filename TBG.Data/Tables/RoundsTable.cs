using MySql.Data.MySqlClient;
using System.Collections.Generic;
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

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.RoundId = resultsPK;
            return entity;
        }

        public static IRound Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Rounds WHERE round_id = @Id";
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

            string query = "SELECT * FROM Rounds";
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
        {
            string query = "UPDATE Rounds SET tournament_id = @tId, round_num = @roundNum WHERE round_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@tId", entity.TournamentId.ToString());
            param.Add("@roundNum", entity.RoundNum.ToString());
            param.Add("@id", entity.RoundId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static IRound Delete(IRound entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Rounds WHERE round_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.RoundId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        private static IRound ConvertReader(MySqlDataReader reader)
        {
            return new Round()
            { 
                RoundId = int.Parse(reader["round_id"].ToString()),
                TournamentId = int.Parse(reader["tournament_id"].ToString()),
                RoundNum = int.Parse(reader["round_num"].ToString()),
            };
        }
    }
}
