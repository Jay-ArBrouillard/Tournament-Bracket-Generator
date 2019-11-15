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
    public static class TeamMembersTable
    {
        public static ITeamMember Create(ITeamMember entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO TeamMembers (team_id, person_id) VALUES (@team, @person)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@team", entity.TeamId.ToString());
            param.Add("@person", entity.PersonId.ToString());

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.PersonTeamId = resultsPK;
            return entity;
        }

        public static ITeamMember Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM TeamMembers WHERE person_team_id = @Id";
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

        public static List<ITeamMember> GetTeamMembersByTeamId(int Id, MySqlConnection dbConn)
        {
            List<ITeamMember> results = new List<ITeamMember>();
            string query = "SELECT * FROM TeamMembers WHERE team_id = @Id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@Id", Id.ToString());

            using (var reader = DatabaseHelper.GetReader(query, dbConn, param))
            {
                while (reader.Read())
                {
                    results.Add(ConvertReader(reader));
                }
            }
            return results;
        }

        public static List<ITeamMember> GetAll(MySqlConnection dbConn)
        {
            List<ITeamMember> results = new List<ITeamMember>();

            string query = "SELECT * FROM TeamMembers";
            using (var reader = DatabaseHelper.GetReader(query, dbConn))
            {
                while (reader.Read())
                {
                    results.Add(ConvertReader(reader));
                }
            }
            return results;
        }

        public static ITeamMember Update(ITeamMember entity, MySqlConnection dbConn)
        {
            string query = "UPDATE TeamMembers SET team_id = @team, person_id = @person WHERE person_team_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@team", entity.TeamId.ToString());
            param.Add("@person", entity.PersonId.ToString());
            param.Add("@id", entity.PersonTeamId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static ITeamMember Delete(ITeamMember entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM TeamMembers WHERE person_team_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.PersonTeamId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        private static ITeamMember ConvertReader(MySqlDataReader reader)
        {
            return new TeamMember()
            {
                PersonTeamId = int.Parse(reader["person_team_id"].ToString()),
                TeamId = int.Parse(reader["team_id"].ToString()),
                PersonId = int.Parse(reader["person_id"].ToString())
            };
        }
    }
}
