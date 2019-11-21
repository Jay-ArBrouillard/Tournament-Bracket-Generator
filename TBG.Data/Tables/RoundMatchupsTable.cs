using MySql.Data.MySqlClient;
using System.Collections.Generic;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public class RoundMatchupsTable
    {
        public static IRoundMatchup Create(IRoundMatchup entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO RoundMatchups (round_id, matchup_id, matchup_number) VALUES (@round, @matchup, @number)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@round", entity.RoundId.ToString());
            param.Add("@matchup", entity.MatchupId.ToString());
            param.Add("@number", entity.MatchupNumber.ToString());

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.RoundId = resultsPK;
            return entity;
        }

        public static IRoundMatchup Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM RoundMatchups WHERE round_matchup_id = @Id";
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

        public static List<IRoundMatchup> GetAll(MySqlConnection dbConn)
        {
            List<IRoundMatchup> result = new List<IRoundMatchup>();

            string query = "SELECT * FROM RoundMatchups";
            using (var reader = DatabaseHelper.GetReader(query, dbConn))
            {
                while (reader.Read())
                {
                    result.Add(ConvertReader(reader));
                }
            }
            return result;
        }

        public static List<IRoundMatchup> GetByRoundId(IRoundMatchup entity, MySqlConnection dbConn)
        {
            List<IRoundMatchup> result = new List<IRoundMatchup>();
            string query = "SELECT * FROM RoundMatchups WHERE round_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@Id", entity.RoundId.ToString());

            using (var reader = DatabaseHelper.GetReader(query, dbConn, param))
            {
                while (reader.Read())
                {
                    result.Add(ConvertReader(reader));
                }
            }
            return result;
        }

        private static IRoundMatchup ConvertReader(MySqlDataReader reader)
        {
            return new RoundMatchup()
            {
                RoundMatchupId = int.Parse(reader["round_matchup_id"].ToString()),
                RoundId = int.Parse(reader["round_id"].ToString()),
                MatchupId = int.Parse(reader["matchup_id"].ToString()),
                MatchupNumber = int.Parse(reader["matchup_number"].ToString())
            };
        }
    }
}
