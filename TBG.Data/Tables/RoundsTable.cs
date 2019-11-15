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

        public static IRound GetByTournamentIdAndRoundNum(IRound entity, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Rounds WHERE tournament_id = @tournamentId AND round_num = @roundNum";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@tournamentId", entity.TournamentId.ToString());
            param.Add("@roundNum", entity.RoundNum.ToString());

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
