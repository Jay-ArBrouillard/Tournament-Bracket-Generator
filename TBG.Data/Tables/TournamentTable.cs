using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public static class TournamentTable
    {
        public static ITournament Create(ITournament entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO Tournaments (user_id, tournament_name, entry_fee, total_prize_pool, tournament_type_id) VALUES (@user, @name, @fee, @pool, @type)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@user", entity.UserId.ToString());
            param.Add("@name", entity.TournamentName);
            param.Add("@fee", entity.EntryFee.ToString());
            param.Add("@pool", entity.TotalPrizePool.ToString());
            param.Add("@type", entity.TournamentTypeId.ToString());


            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.TournamentId = resultsPK;
            return entity;
        }

        public static ITournament Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM `Tournaments` WHERE `tournament_id` = @Id";
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

        public static ITournament GetTournamentByName(string name, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM `Tournaments` WHERE `tournament_name` = @name";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@name", name);

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

        public static List<ITournament> GetAll(MySqlConnection dbConn)
        {
            List<ITournament> result = new List<ITournament>();

            string query = "SELECT * FROM Tournaments";
            using (var reader = DatabaseHelper.GetReader(query, dbConn, new Dictionary<string, string>()))
            {
                while (reader.Read())
                {
                    result.Add(ConvertReader(reader));
                }
            }
            return result;
        }

        public static ITournament UpdateName(ITournament entity, MySqlConnection dbConn)
        {
            string query = "UPDATE Tournaments SET tournament_name = @name WHERE tournament_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@name", entity.TournamentName.ToString());
            param.Add("@id", entity.TournamentId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static ITournament Update(ITournament entity, MySqlConnection dbConn)
        {
            string query = "UPDATE Tournaments SET user_id = @user, tournament_name = @name, entry_fee = @fee, total_prize_pool = @pool, tournament_type_id = @type  WHERE tournament_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@user", entity.UserId.ToString());
            param.Add("@name", entity.TournamentName.ToString());
            param.Add("@fee", entity.EntryFee.ToString());
            param.Add("@pool", entity.TotalPrizePool.ToString());
            param.Add("@type", entity.TournamentTypeId.ToString());
            param.Add("@id", entity.TournamentId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static ITournament Delete(ITournament entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Tournaments WHERE tournament_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.TournamentId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static ITournament ConvertReader(MySqlDataReader reader)
        {
            return new Tournament()
            {
                TournamentId = Int32.Parse(reader["tournament_id"].ToString()),
                UserId = Int32.Parse(reader["user_id"].ToString()),
                TournamentName = reader["tournament_name"].ToString(),
                EntryFee = Double.Parse(reader["entry_fee"].ToString()),
                TotalPrizePool = Double.Parse(reader["total_prize_pool"].ToString()),
                TournamentTypeId = Int32.Parse(reader["tournament_type_id"].ToString())
            };
        }
    }
}