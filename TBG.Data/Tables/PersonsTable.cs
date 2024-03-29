﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.Data.Tables
{
    public class PersonsTable
    {
        public static IPerson Create(IPerson entity, MySqlConnection dbConn)
        {
            string query = "INSERT INTO Persons (first_name, last_name, email, phone, wins, losses) VALUES (@first, @last, @email, @phone, @wins,@losses)";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@first", entity.FirstName.ToString());
            param.Add("@last", entity.LastName.ToString());
            param.Add("@email", entity.Email.ToString());
            param.Add("@phone", entity.Phone.ToString());
            param.Add("@wins", entity.Wins.ToString());
            param.Add("@losses", entity.Losses.ToString());

            var resultsPK = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            entity.PersonId = resultsPK;
            return entity;
        }

        public static IPerson Get(int Id, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Persons WHERE person_id = @Id";
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

        public static IPerson getPersonByUniqueIdentifiers(string firstName, string lastName, string email, MySqlConnection dbConn)
        {
            string query = "SELECT * FROM Persons WHERE first_name = @First AND  last_name = @Last AND email = @Email";
            Dictionary<string, string> param = new Dictionary<string, string>();
            if (firstName == null || lastName == null || email == null) { return null; }

            param.Add("@First", firstName.ToString());
            param.Add("@Last", lastName.ToString());
            param.Add("@Email", email.ToString());

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

        public static List<IPerson> GetAll(MySqlConnection dbConn)
        {
            List<IPerson> results = new List<IPerson>();

            string query = "SELECT * FROM Persons";
            using (var reader = DatabaseHelper.GetReader(query, dbConn))
            {
                while (reader.Read())
                {
                    results.Add(ConvertReader(reader));
                }
            }
            return results;
        }

        public static IPerson Update(IPerson entity, MySqlConnection dbConn)
        {
            string query = "UPDATE Persons SET first_name = @first, last_name = @last, email = @email, phone = @phone, wins = @wins, losses = @losses WHERE person_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@first", entity.FirstName.ToString());
            param.Add("@last", entity.LastName.ToString());
            param.Add("@email", entity.Email.ToString());
            param.Add("@phone", entity.Phone.ToString());
            param.Add("@wins", entity.Wins.ToString());
            param.Add("@losses", entity.Losses.ToString());
            param.Add("@id", entity.PersonId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        public static IPerson Delete(IPerson entity, MySqlConnection dbConn)
        {
            string query = "DELETE FROM Persons WHERE person_id = @id";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@id", entity.PersonId.ToString());

            var result = DatabaseHelper.GetNonQueryCount(query, dbConn, param);
            if (result != 0)
            {
                return entity;
            }

            return null;
        }

        private static IPerson ConvertReader(MySqlDataReader reader)
        {
            return new Person()
            {
                PersonId = int.Parse(reader["person_id"].ToString()),
                FirstName = reader["first_name"].ToString(),
                LastName = reader["last_name"].ToString(),
                Email = reader["email"].ToString(),
                Phone = reader["phone"].ToString(),
                Wins = int.Parse(reader["wins"].ToString()),
                Losses = int.Parse(reader["losses"].ToString())
            };
        }
    }
}
