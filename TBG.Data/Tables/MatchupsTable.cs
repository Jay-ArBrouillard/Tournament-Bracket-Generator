using MySql.Data.MySqlClient;
using System.Collections.Generic;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public class MatchupsTable
    {
        public static IMatchup Create(IMatchup entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO Matchups (round_id, completed) VALUES (@round, @completed)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@round", entity.RoundId.ToString());
            param.Add("@completed", DatabaseHelper.BoolToString(entity.Completed));

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.MatchupId = resultsPK;
            return entity;
        }

        public static IMatchup Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Matchups WHERE matchup_id = @Id";
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

        public static IMatchup Update(IMatchup entity, MySqlConnection dbConn)
        {
            string query = "UPDATE Matchups SET completed = @status WHERE matchup_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@status", DatabaseHelper.BoolToString(entity.Completed));
            param.Add("@id", entity.MatchupId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static IMatchup Delete(IMatchup entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Matchups WHERE matchup_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.MatchupId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static List<IMatchup> GetAll(MySqlConnection dbConn)
        {
            List<IMatchup> result = new List<IMatchup>();

            string query = "SELECT * FROM Matchups";
            using (var reader = DatabaseHelper.GetReader(query, dbConn))
            {
                while (reader.Read())
                {
                    result.Add(ConvertReader(reader));
                }
            }
            return result;
        }

        private static IMatchup ConvertReader(MySqlDataReader reader)
        {
            return new Matchup()
            {
                MatchupId = int.Parse(reader["matchup_id"].ToString()),
                RoundId = int.Parse(reader["round_id"].ToString()),
                Completed = bool.Parse(reader["completed"].ToString())
            };
        }
    }
}
