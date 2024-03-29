﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace TBG.Data.Classes
{
    public static class DatabaseHelper
    {
        public static MySqlDataReader GetReader(string query, MySqlConnection dbConn)
        {
            return GetReader(query, dbConn, new Dictionary<string, string>());
        }

        public static MySqlDataReader GetReader(string query, MySqlConnection dbConn, Dictionary<string, string> parameters)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                foreach(var entry in parameters)
                {
                    cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                }
                return cmd.ExecuteReader();
            }
        }

        public static int GetNonQueryCount(string query, MySqlConnection dbConn)
        {
            return GetNonQueryCount(query, dbConn, new Dictionary<string, string>());
        }

        public static int GetNonQueryCount(string query, MySqlConnection dbConn, Dictionary<string, string> parameters)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, dbConn))
            {
                foreach (var entry in parameters)
                {
                    cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                }
                var results = cmd.ExecuteNonQuery();

                // If has last inserted id, add a parameter to hold it.
                if (cmd.LastInsertedId > 0)
                {
                    return Convert.ToInt32(cmd.LastInsertedId);
                }

                return -1;
            }
        }

        public static string DateToString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string BoolToString(Boolean boolean)
        {
            if(boolean) { return "1"; }
            return "0";
        }
    }
}
