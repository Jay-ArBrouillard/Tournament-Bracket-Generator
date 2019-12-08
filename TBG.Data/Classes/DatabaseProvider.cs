using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TBG.Core.Interfaces;
using TBG.Data.Entities;
using TBG.Data.Tables;

namespace TBG.Data.Classes
{
    public class DatabaseProvider : IProvider, IDisposable
    {
        private static string connString = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;
        private static MySqlConnection dbConn;

        public bool sourceActive { get; set; }

        #region CLASS SETUP METHODS
        public DatabaseProvider()
        {
            try
            {
                dbConn = new MySqlConnection(connString);
                dbConn.Open();
                sourceActive = true;
            }
            catch
            {
                sourceActive = false;
            }
        }

        public void Dispose()
        {
            if (dbConn != null) { dbConn.Close(); }
        }
        #endregion

        #region TOURNAMENT METHODS
        public ITournament createTournament(ITournament entry)
        {
            ITournament tournament = TournamentTable.Create(entry, dbConn);

            if (tournament == null) { return null; }

            entry.TournamentEntries.ForEach(x => x.TournamentId = tournament.TournamentId);
            entry.TournamentEntries.ForEach(x => TournamentEntryTable.Create(x, dbConn));

            entry.Rounds.ForEach(x => x.TournamentId = tournament.TournamentId);

            foreach (var round in entry.Rounds)
            {
                round.RoundId = RoundsTable.Create(round, dbConn).RoundId;
                CreateRoundLinks(round, entry.TournamentEntries);
            }
            
            return tournament;
        }

        public ITournament saveActiveRound (ITournament entry)
        {
            TournamentTable.Update(entry, dbConn);
            entry.Rounds.ForEach(x => x.TournamentId = entry.TournamentId);
            var activeRound = entry.Rounds.Where(x => x.RoundNum == entry.ActiveRound).First();

            activeRound.Matchups.ForEach(x => x.RoundId = activeRound.RoundId);
            CreateRoundLinks(activeRound, entry.TournamentEntries);

            return entry;
        }

        private void CreateRoundLinks(IRound round, List<ITournamentEntry> tournamentEntries)
        {
            round.Matchups.ForEach(x => x.RoundId = round.RoundId);
            foreach (var matchup in round.Matchups)
            {
                matchup.MatchupId = MatchupsTable.Create(matchup, dbConn).MatchupId;
                matchup.MatchupEntries.ForEach(x => x.MatchupId = matchup.MatchupId);
                foreach (var matchupEntry in matchup.MatchupEntries)
                {
                    matchupEntry.TournamentEntryId = tournamentEntries.Find(x => x.TeamId == matchupEntry.TheTeam.TeamId).TournamentEntryId;
                    matchupEntry.MatchupEntryId = MatchupEntriesTable.Create(matchupEntry, dbConn).MatchupEntryId;
                }
            }
        }

        public ITournament deleteTournament (int id)
        {
            var tournament = getTournament(id);
            foreach(var round in tournament.Rounds)
            {
                foreach(var matchup in round.Matchups)
                {
                    foreach(var matchupEntry in matchup.MatchupEntries)
                    {
                        MatchupEntriesTable.Delete(matchupEntry, dbConn);
                    }
                    MatchupsTable.Delete(matchup, dbConn);
                }
                RoundsTable.Delete(round, dbConn);
            }

            foreach(var tournamentEntry in tournament.TournamentEntries)
            {
                TournamentEntryTable.Delete(tournamentEntry, dbConn);
            }

            TournamentTable.Delete(tournament, dbConn);
            return tournament;
        }

        public ITournament getTournament(int id)
        {
            var tournament = TournamentTable.Get(id, dbConn);
            var allRounds = RoundsTable.GetAll(dbConn);
            var allMatchups = MatchupsTable.GetAll(dbConn);
            var allMatchupEntries = MatchupEntriesTable.GetAll(dbConn);
            var allTournamentEntries = TournamentEntryTable.GetAll(dbConn);
            var allTeams = TeamsTable.GetAll(dbConn);
            var allTeamMembers = TeamMembersTable.GetAll(dbConn);
            var allPersons = PersonsTable.GetAll(dbConn);
            tournament.TournamentEntries = allTournamentEntries.Where(x => x.TournamentId == tournament.TournamentId).ToList();
            foreach(var entry in tournament.TournamentEntries)
            {
                var entryMembers = allTeamMembers.Where(x => x.TeamId == entry.TeamId).ToList();
                foreach(var member in entryMembers)
                {
                    entry.Members.Add(allPersons.Find(x => x.PersonId == member.PersonId));
                }
                var theTeam = allTeams.Where(x => x.TeamId == entry.TeamId).First();
                tournament.Teams.Add(theTeam);
            }
            tournament.Rounds = allRounds.Where(x => x.TournamentId == tournament.TournamentId).ToList();
            foreach(var round in tournament.Rounds)
            {
                round.Matchups = allMatchups.Where(x => x.RoundId == round.RoundId).ToList();
                foreach(var matchup in round.Matchups)
                {
                    matchup.MatchupEntries = allMatchupEntries.Where(x => x.MatchupId == matchup.MatchupId).ToList();
                    foreach(var team in matchup.MatchupEntries)
                    {
                        team.TheTeam = tournament.TournamentEntries.Where(x => x.TournamentEntryId == team.TournamentEntryId).First();
                    }
                }
            }

            return tournament;
        }
        #endregion

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

        public ITeam getTeam(int teamId)
        {
            return TeamsTable.Get(teamId, dbConn);
        }

        public List<ITeam> getTeamsFromTournamentId(int tournamentId)
        {
            var results = new List<ITeam>();
            var tournamentEntries = TournamentEntryTable.GetAll(dbConn);
            var teams = TeamsTable.GetAll(dbConn);
            var teamHashTable = teams.ToDictionary(x => x.TeamId, x => x);

            for (int i = 0; i < tournamentEntries.Count; i++)
            {
                if (tournamentEntries[i].TournamentId == tournamentId)
                {
                    do
                    {
                        results.Add(teamHashTable[tournamentEntries[i].TeamId]);
                        i++;
                    }
                    while (i < tournamentEntries.Count && tournamentEntries[i].TournamentId == tournamentId);
                    break;
                }
            }

            return results;
        }

        public List<ITeam> getAllTeams()
        {
            var allTeams = TeamsTable.GetAll(dbConn);
            var allTeamMembers = TeamMembersTable.GetAll(dbConn);
            var allPersons = PersonsTable.GetAll(dbConn);
            foreach (var team in allTeams)
            {
                var members = allTeamMembers.Where(x => x.TeamId == team.TeamId);
                foreach(var person in members)
                {
                    var thePersonRecord = allPersons.Where(x => x.PersonId == person.PersonId).First();
                    team.TeamMembers.Add(thePersonRecord);
                }
            }

            return allTeams;
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

        public IPerson deletePerson(IPerson entry)
        {
            return PersonsTable.Delete(entry, dbConn);
        }
        #endregion

        #region LOGIN(USER) METHODS 
        public IUser createUser(IUser entry)
        {
            IUser user = UsersTable.Create(entry, dbConn);
            if (user == null) { return null; }

            user = UsersTable.Get(user.UserName, dbConn);
            if (user == null) { return null; }

            if (user.UserId == 0)
            {
                Console.WriteLine("Error userid == 0");
            }

            return user;
        }

        public IUser getUser(string userName)
        {
            return UsersTable.Get(userName, dbConn);
        }

        public IUser updateUser(IUser entity)
        {
            UsersTable.Update(entity, dbConn);
            return entity;
        }

        public IUser deleteUser(IUser entry)
        {
            return UsersTable.Delete(entry, dbConn);
        }
        #endregion

        #region DASHBOARD METHODS
        public List<ITournament> getAllTournaments()
        {
            return TournamentTable.GetAll(dbConn);
        }

        public List<ITournamentType> getTournamentTypes()
        {
            return TournamentTypeTable.GetAll();
        }
        #endregion

        #region PRIZE METHODS
        public IPrize createPrize(IPrize prize)
        {
            return PrizesTable.Create(prize, dbConn);
        }

        public List<IPrize> getAllPrizes()
        {
            return PrizesTable.GetAll(dbConn);
        }
        #endregion

        #region MATCHUP METHODS
        public IMatchup saveMatchupScore(IMatchup matchup)
        {
            MatchupsTable.Update(matchup, dbConn);
            foreach(var entity in matchup.MatchupEntries)
            {
                MatchupEntriesTable.Update(entity, dbConn);
            }

            return matchup;
        }
        #endregion

        #region TOURNMENT ENTRY METHODS
        public List<ITournamentEntry> saveTournamentEntry(IMatchup matchup)
        {
            ITournamentEntry entry1 = TournamentEntryTable.Get(matchup.MatchupEntries[0].TournamentEntryId, dbConn);
            ITournamentEntry entry2 = TournamentEntryTable.Get(matchup.MatchupEntries[1].TournamentEntryId, dbConn);

            if (entry1 == null || entry2 == null) { return null; }

            entry1.Wins = matchup.MatchupEntries[0].TheTeam.Wins;
            entry1.Losses = matchup.MatchupEntries[0].TheTeam.Losses;
            entry2.Wins = matchup.MatchupEntries[1].TheTeam.Wins;
            entry2.Losses = matchup.MatchupEntries[1].TheTeam.Losses;

            List<ITournamentEntry> results = new List<ITournamentEntry>();

            if (TournamentEntryTable.Update(entry1, dbConn) == null) { return null; }
            if (TournamentEntryTable.Update(entry2, dbConn) == null) { return null; }

            results.Add(entry1);
            results.Add(entry2);
            return results;
        }
        #endregion
    }
}
