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
    public class TeamsTable
    {
        public static ITeam Create(ITeam entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO Teams (team_name, wins, losses) VALUES (@name, @wins, @losses)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@name", entity.TeamName);
            param.Add("@wins", entity.Wins.ToString());
            param.Add("@losses", entity.Losses.ToString());

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.TeamId = resultsPK;
            return entity;
        }

        public static ITeam Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Teams WHERE team_id = @Id";
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

        public static ITeam Get(string value, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Teams WHERE team_name = @team";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@team", value);

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

        public static List<ITeam> GetAll(MySqlConnection dbConn)
        {
            List<ITeam> results = new List<ITeam>();

            string query = "SELECT * FROM Teams";
            using (var reader = DatabaseHelper.GetReader(query, dbConn))
            {
                while (reader.Read())
                {
                    results.Add(ConvertReader(reader));
                }
            }
            return results;
        }

        public static ITeam Update(ITeam entity, MySqlConnection dbConn)
        {
            string query = "UPDATE Teams SET team_name = @name, wins = @wins, losses = @losses WHERE team_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@name", entity.TeamName.ToString());
            param.Add("@wins", entity.Wins.ToString());
            param.Add("@losses", entity.Losses.ToString());
            param.Add("@id", entity.TeamId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static ITeam Delete(ITeam entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Users WHERE team_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.TeamId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        private static ITeam ConvertReader(MySqlDataReader reader)
        {
            return new Team()
            {
                TeamId = int.Parse(reader["team_id"].ToString()),
                TeamName = reader["team_name"].ToString(),
                Wins = int.Parse(reader["wins"].ToString()),
                Losses = int.Parse(reader["losses"].ToString())
            };
        }
    }
}
