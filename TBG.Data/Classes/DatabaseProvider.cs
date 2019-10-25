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
            ITeam team = TeamsTable.Create(entry, dbConn);
            if (team == null) { return null; }

            team = TeamsTable.Get(team.TeamName, dbConn);
            if (team == null) { return null; }

            entry.TeamId = team.TeamId;  //Quickfix for TeamsTable.Create() not setting the TeamId Correctly
            
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

        public List<ITeam> getAllTeams()
        {
            return TeamsTable.GetAll(dbConn);
        }
        #endregion

        #region TEAMMEMBER METHODS

        public List<ITeamMember> getTeamMembersByTeamId(int teamId)
        {
            return TeamMembersTable.GetTeamMembersByTeamId(teamId, dbConn);
        }

        #endregion

        #region PERSON METHODS
        public IPerson createPerson(IPerson entry)
        {
            IPerson person = PersonsTable.Create(entry, dbConn);
            if (person == null) { return null; }

            person = PersonsTable.getPersonByUniqueIdentifiers(entry.FirstName, entry.LastName, entry.Email, dbConn);
            if (person == null) { return null; }

            entry.PersonId = person.PersonId;

            return entry;
        }
        public IPerson getPerson(int personId)
        {
            return PersonsTable.Get(personId, dbConn);
        }

        public IPerson getPersonByUniqueIdentifiers(string firstName, string lastName, string email)
        {
            return PersonsTable.getPersonByUniqueIdentifiers(firstName, lastName, email, dbConn);
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

        #region PRIZE METHODS
        public IPrize createPrize(IPrize prize)
        {
            return PrizesTable.Create(prize, dbConn);
        }

        public List<IPrize> GetAllPrizes()
        {
            return PrizesTable.GetAll(dbConn);
        }
        #endregion
    }
}
