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
    public class MatchupEntriesTable
    {
        public static IMatchupEntry Create(IMatchupEntry entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO MatchupEntries (matchup_entry_id, matchup_id, tournament_entry_id, score) VALUES (NULL, @matchId, @entryId, @score)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@matchId", entity.MatchupId.ToString());
            param.Add("@entryId", entity.TournamentEntryId.ToString());
            param.Add("@score", entity.Score.ToString());

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.MatchupEntryId = resultsPK;
            return entity;
        }

        public static IMatchupEntry Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM MatchupEntries WHERE matchup_entry_id = @Id";
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

        public static List<IMatchupEntry> GetByMatchupId(int matchupId, MySqlConnection dbConn)
        {
            List<IMatchupEntry> results = new List<IMatchupEntry>();

            string query = "SELECT * FROM MatchupEntries WHERE matchup_id = @Id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@Id", matchupId.ToString());

            using (var reader = DatabaseHelper.GetReader(query, dbConn, param))
            {
                while (reader.Read())
                {
                    results.Add(ConvertReader(reader));
                }
            }
            return results;
        }

        public static IMatchupEntry GetByMatchupIdAndTournamentEntryId(int matchupId, int tournamentEntryId, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM MatchupEntries WHERE matchup_id = @match AND tournament_entry_id = @entryId";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@match", matchupId.ToString());
            param.Add("@entryId", tournamentEntryId.ToString());

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

        public static IMatchupEntry UpdateScore(int Id, int score, MySqlConnection dbConn)
        {
            string query = "UPDATE MatchupEntries SET score = @score WHERE matchup_entry_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@Id", Id.ToString());
            param.Add("@score", score.ToString());

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

        private static IMatchupEntry ConvertReader(MySqlDataReader reader)
        {
            return new MatchupEntry()
            {
                MatchupEntryId = int.Parse(reader["matchup_entry_id"].ToString()),
                MatchupId = int.Parse(reader["matchup_id"].ToString()),
                TournamentEntryId = int.Parse(reader["tournament_entry_id"].ToString()),
                Score = int.Parse(reader["score"].ToString()),
            };
        }

    }
}
