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
        public static IUser Create(IUser entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO Users (user_name, password, active, admin, last_login) VALUES (@username, @password, @active, @admin, @lastLogin)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@username", entity.UserName);
            param.Add("@password", entity.Password);
            param.Add("@active", DatabaseHelper.BoolToString(entity.Active));
            param.Add("@admin", DatabaseHelper.BoolToString(entity.Admin));
            param.Add("@lastLogin", DatabaseHelper.DateToString(entity.LastLogin));

            var results = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (results > 0) { return entity; }
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

        public static IUser Get(string value, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Users WHERE user_name = @User";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@User", value);

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

        public static IUser Update(IUser entity, MySqlConnection dbConn)
        {
            string query = "UPDATE Users SET user_name = @user, password = @password, active = @active, admin = @admin, last_login = @lastLogin  WHERE user_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@user", entity.UserName.ToString());
            param.Add("@password", entity.Password.ToString());
            param.Add("@active", DatabaseHelper.BoolToString(entity.Active));
            param.Add("@admin", DatabaseHelper.BoolToString(entity.Admin));
            param.Add("@lastLogin", DatabaseHelper.DateToString(entity.LastLogin));
            param.Add("@id", entity.UserId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static IUser Delete(IUser entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Users WHERE user_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.UserId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
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
