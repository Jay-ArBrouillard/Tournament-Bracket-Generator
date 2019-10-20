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
    public static class UsersTable
    {
        public static IUser Create(IUser user, MySqlConnection dbConn)
        {
            string query = "INSERT INTO Users (user_name, password, active, admin, last_login) VALUES (@username, @password, @active, @admin, @lastLogin)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@username", user.UserName);
            param.Add("@password", user.Password);
            param.Add("@active", DatabaseHelper.BoolToString(user.Active));
            param.Add("@admin", DatabaseHelper.BoolToString(user.Admin));
            param.Add("@lastLogin", DatabaseHelper.DateToString(user.LastLogin));

            var results = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (results > 0) { return user; }
            return null;
        }

        public static IUser Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Users WHERE user_id = @Id";
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

        public static IUser Get(string Username, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Users WHERE user_name = @User";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@User", Username);

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

        public static List<IUser> GetAll(MySqlConnection dbConn)
        {
            List<IUser> results = new List<IUser>();

            string query = "SELECT * FROM Users";
            using (var reader = DatabaseHelper.GetReader(query, dbConn))
            {
                while (reader.Read())
                {
                    results.Add(ConvertReader(reader));
                }
            }
            return results;
        }

        public static IUser Update(IUser user, MySqlConnection dbConn)
        {
            string query = "UPDATE Users SET user_name = @user, password = @password, active = @active, admin = @admin, last_login = @lastLogin  WHERE user_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@user", user.UserName.ToString());
            param.Add("@password", user.Password.ToString());
            param.Add("@active", DatabaseHelper.BoolToString(user.Active));
            param.Add("@admin", DatabaseHelper.BoolToString(user.Admin));
            param.Add("@lastLogin", DatabaseHelper.DateToString(user.LastLogin));
            param.Add("@id", user.UserId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return user;
            }

            return null;
        }

        public static IUser Delete(IUser tournament, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Users WHERE user_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", tournament.UserId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return tournament;
            }

            return null;
        }

        private static IUser ConvertReader(MySqlDataReader reader)
        {
            return new User()
            {
                UserId = int.Parse(reader["user_id"].ToString()),
                UserName = reader["user_name"].ToString(),
                Password = reader["password"].ToString(),
                Active = bool.Parse(reader["active"].ToString()),
                Admin = bool.Parse(reader["admin"].ToString()),
                LastLogin = DateTime.Parse(reader["last_login"].ToString())
            };
        }
    }
}
