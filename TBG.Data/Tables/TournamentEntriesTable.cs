using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public static class TournamentEntryTable
    {
        public static ITournamentEntry Create(ITournamentEntry entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO TournamentEntries (tournament_id, team_id, seed) VALUES (@tournament, @team, @seed)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@tournament", entity.TournamentId.ToString());
            param.Add("@team", entity.TeamId.ToString());
            param.Add("@seed", entity.Seed.ToString());

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.TournamentEntryId = resultsPK;
            return entity;
        }

        public static ITournamentEntry Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM `TournamentEntries` WHERE `tournament_entry_id` = @Id";
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

        public static List<ITournamentEntry> GetByTournamentId(int Id, MySqlConnection dbConn)
        {
            List<ITournamentEntry> result = new List<ITournamentEntry>();
            string query = "SELECT * FROM `TournamentEntries` WHERE `tournament_id` = @Id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@Id", Id.ToString());

            using (var reader = DatabaseHelper.GetReader(query, dbConn, param))
            {
                while (reader.Read())
                {
                    result.Add(ConvertReader(reader));
                }

                return result;
            }
        }

        public static List<ITournamentEntry> GetAll(MySqlConnection dbConn)
        {
            List<ITournamentEntry> result = new List<ITournamentEntry>();

            string query = "SELECT * FROM TournamentEntries";
            using (var reader = DatabaseHelper.GetReader(query, dbConn, new Dictionary<string, string>()))
            {
                while (reader.Read())
                {
                    result.Add(ConvertReader(reader));
                }
            }
            return result;
        }

        public static ITournamentEntry ConvertReader(MySqlDataReader reader)
        {
            return new TournamentEntry()
            {
                TournamentEntryId = Int32.Parse(reader["tournament_entry_id"].ToString()),
                TournamentId = Int32.Parse(reader["tournament_id"].ToString()),
                TeamId = Int32.Parse(reader["team_id"].ToString()),
                Seed = Int32.Parse(reader["seed"].ToString())
            };
        }
    }
}
