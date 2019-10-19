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
        public static ITournament Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM `Tournaments` WHERE `tournament_id` = @Id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@Id", Id.ToString());

            using (var reader = DatabaseHelper.GetReader(query, dbConn, param))
            {
                if (reader.HasRows)
                {
                    Tournament found = new Tournament()
                    {
                        TournamentId = Int32.Parse(reader["tournament_id"].ToString()),
                        UserId = Int32.Parse(reader["tournament_id"].ToString()),
                        TournamentName = reader["tournament_name"].ToString(),
                        EntryFee = Decimal.Parse(reader["entry_fee"].ToString()),
                        TotalPrizePool = Double.Parse(reader["total_prize_pool"].ToString()),
                        TournamentTypeId = Int32.Parse(reader["tournament_type_id"].ToString())
                    };
                    return found;
                }
            }
            return new Tournament();
        }

        public static List<ITournament> GetAll(MySqlConnection dbConn)
        {
            List<ITournament> result = new List<ITournament>();

            string query = "SELECT * FROM Tournaments";
            using (var reader = DatabaseHelper.GetReader(query, dbConn, new Dictionary<string, string>()))
            {
                while (reader.Read())
                {
                    Tournament found = new Tournament()
                    {
                        TournamentId = Int32.Parse(reader["tournament_id"].ToString()),
                        UserId = Int32.Parse(reader["tournament_id"].ToString()),
                        TournamentName = reader["tournament_name"].ToString(),
                        EntryFee = Decimal.Parse(reader["entry_fee"].ToString()),
                        TotalPrizePool = Double.Parse(reader["total_prize_pool"].ToString()),
                        TournamentTypeId = Int32.Parse(reader["tournament_type_id"].ToString())
                    };
                    result.Add(found);
                }
            }
            return result;
        }

        public static ITournament Update(ITournament tournament, MySqlConnection dbConn)
        {
            string query = "UPDATE Tournaments SET user_id = @user, tournament_name = @name, entry_fee = @fee, total_prize_pool = @pool, tournament_type_id = @type  WHERE tournament_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@user", tournament.UserId.ToString());
            param.Add("@name", tournament.TournamentName.ToString());
            param.Add("@fee", tournament.EntryFee.ToString());
            param.Add("@pool", tournament.TotalPrizePool.ToString());
            param.Add("@type", tournament.TournamentTypeId.ToString());
            param.Add("@id", tournament.TournamentId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return tournament;
            }

            return null;
        }

        public static ITournament Delete(ITournament tournament, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Tournaments WHERE tournament_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", tournament.TournamentId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return tournament;
            }

            return null;
        }
    }
}
