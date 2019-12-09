using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public class TournamentPrizesTable
    {
        public static ITournamentPrize Create(ITournamentPrize entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO TournamentPrizes (tournament_id, prize_id, place_id) VALUES (@tournament, @prize, @place)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@tournament", entity.TournamentId.ToString());
            param.Add("@prize", entity.PrizeId.ToString());
            param.Add("@place", entity.PlaceId.ToString());

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.TournamentPrizeId = resultsPK;
            return entity;
        }

        public static ITournamentPrize Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM TournamentPrizes WHERE tournament_prize_id = @Id";
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

        public static List<ITournamentPrize> GetAll(MySqlConnection dbConn)
        {
            List<ITournamentPrize> result = new List<ITournamentPrize>();

            string query = "SELECT * FROM TournamentPrizes";
            using (var reader = DatabaseHelper.GetReader(query, dbConn, new Dictionary<string, string>()))
            {
                while (reader.Read())
                {
                    result.Add(ConvertReader(reader));
                }
            }
            return result;
        }

        public static ITournamentPrize Update(ITournamentPrize entity, MySqlConnection dbConn)
        {
            string query = "UPDATE TournamentPrizes SET prize_id = @prize, tournament_id = @tourney, place_id = @place WHERE tournament_prize_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.TournamentPrizeId.ToString());
            param.Add("@prize", entity.PrizeId.ToString());
            param.Add("@tourney", entity.TournamentId.ToString());
            param.Add("@place", entity.PlaceId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static ITournamentPrize Delete(ITournamentPrize entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM TournamentPrizes WHERE tournament_prize_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.TournamentPrizeId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static ITournamentPrize ConvertReader(MySqlDataReader reader)
        {
            return new TournamentPrize()
            {
                TournamentPrizeId = Int32.Parse(reader["tournament_prize_id"].ToString()),
                PrizeId = Int32.Parse(reader["prize_id"].ToString()),
                PlaceId = Int32.Parse(reader["place_id"].ToString()),
                TournamentId = Int32.Parse(reader["tournament_id"].ToString())
            };
        }
    }
}
