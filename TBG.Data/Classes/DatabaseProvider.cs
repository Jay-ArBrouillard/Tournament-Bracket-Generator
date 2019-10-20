using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using TBG.Core.Interfaces;
using TBG.Data.Entities;
using TBG.Data.Tables;

namespace TBG.Data.Classes
{
    public class DatabaseProvider : IProvider, IDisposable
    {
        private static string connString = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;
        private static MySqlConnection dbConn;

        #region CLASS SETUP METHODS
        public DatabaseProvider()
        {
            dbConn = new MySqlConnection(connString);
            dbConn.Open();
        }

        public void Dispose()
        {
            if (dbConn != null) { dbConn.Close(); }
        }
        #endregion

        public bool createTournament(ITournament entry)
        {
            //Create Tournament Record
            //Create Cross Reference Records For Tournament/Team/Player
            return true;
        }

        #region TEAM METHODS
        public ITeam createTeam(ITeam entry)
        {
            var team = TeamsTable.Create(entry, dbConn);
            if (team == null) { return null; }

            team = TeamsTable.Get(team.TeamName, dbConn);
            if (team == null) { return null; }

            foreach (var participant in entry.TeamMembers)
            {
                TeamMembersTable.Create(new TeamMember()
                {
                    PersonId = participant.PersonId,
                    TeamId = team.TeamId
                }, dbConn);
            }
            return entry;
        }

        public ITeam getTeam(string teamName)
        {
            return TeamsTable.Get(teamName, dbConn);
        }
        #endregion

        #region PERSON METHODS
        public IPerson createPerson(IPerson entry)
        {
            return PersonsTable.Create(entry, dbConn);
        }

        public List<IPerson> getPeople()
        {
            return PersonsTable.GetAll(dbConn);
        }
        #endregion
        
        #region LOGIN METHODS
        public IUser createUser(IUser thisUser)
        {
            return UsersTable.Create(thisUser, dbConn);
        }

        public IUser getUser(string userName)
        {
            return UsersTable.Get(userName, dbConn);
        }

        public IUser updateLoginTime(IUser thisUser)
        {
            thisUser.LastLogin = DateTime.Now;
            return UsersTable.Update(thisUser, dbConn);
        }
        #endregion

        #region DASHBOARD METHODS
        public List<ITournament> GetAllTournaments()
        {
            return TournamentTable.GetAll(dbConn); ;
        }

        public List<ITournamentType> GetTournamentTypes()
        {
            return TournamentTypeTable.GetAll();
        }
        #endregion
    }
}
