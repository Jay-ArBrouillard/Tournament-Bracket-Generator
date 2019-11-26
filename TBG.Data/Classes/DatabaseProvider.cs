﻿using MySql.Data.MySqlClient;
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

        #region TOURNAMENT METHODS
        public ITournament createTournament(ITournament entry)
        {
            ITournament tournament = TournamentTable.Create(entry, dbConn);

            if (tournament == null) { return null; }

            entry.Participants.ForEach(x => x.TournamentId = tournament.TournamentId);
            entry.Rounds.ForEach(x => x.TournamentId = tournament.TournamentId);
            setupTournamentData(tournament);
            
            return tournament;
        }

        public ITournament saveActiveRound (ITournament entry)
        {
            //Update tournamentId for Rounds
            TournamentTable.Update(entry, dbConn);
            entry.Rounds.ForEach(x => x.TournamentId = entry.TournamentId);
            var activeRound = entry.Rounds.Where(x => x.RoundNum == entry.ActiveRound).First();

            IRound newRound = createRound(new Round(entry.TournamentId, entry.ActiveRound));
            //Create Matchups
            var matchupCount = 1;
            foreach (var matchup in activeRound.Pairings)
            {
                IMatchup newMatchup = createMatchup(new Matchup());
                IRoundMatchup newRoundMatchup = createRoundMatchup(new RoundMatchup(newRound.RoundId, newMatchup.MatchupId, matchupCount));
                createMatchupEntry(new MatchupEntry(newRoundMatchup.MatchupId, matchup.Teams[0].TheTeam.TournamentEntryId));
                createMatchupEntry(new MatchupEntry(newRoundMatchup.MatchupId, matchup.Teams[1].TheTeam.TournamentEntryId));
            }

            return entry;
        }

        public ITournament updateTournamentName(ITournament entry)
        {
            ITournament tournament = TournamentTable.UpdateName(entry, dbConn);
            if (tournament == null) { return null; }

            return tournament;
        }

          public void setupTournamentData(ITournament tournament)
        {
            if (tournament.TournamentTypeId == 1)
            {
                List<ITournamentEntry> tournamentEntries = createTournamentEntries(tournament.Participants);
                IRound roundOne = createRound(new Round(tournament.TournamentId, tournament.ActiveRound));

                int numMatchupsRoundOne = tournament.Participants.Count / 2;
                int roundOneId = roundOne.RoundId;

                for (int i = 0; i < numMatchupsRoundOne; i++)
                {
                    IMatchup newMatchup = createMatchup(new Matchup());
                    IRoundMatchup newRoundMatchup = createRoundMatchup(new RoundMatchup(roundOneId, newMatchup.MatchupId, i));
                    createMatchupEntry(new MatchupEntry(newRoundMatchup.MatchupId, tournamentEntries[0].TournamentEntryId));
                    createMatchupEntry(new MatchupEntry(newRoundMatchup.MatchupId, tournamentEntries[1].TournamentEntryId));
                    tournamentEntries.RemoveAt(0);
                    tournamentEntries.RemoveAt(0);
                }
            }
        }

        public ITournament deleteTournament (ITournament entry)
        {
            ITournament tournament = TournamentTable.Delete(entry, dbConn);
            if (tournament == null) { return null; }

            return tournament;
        }

        public ITournament getTournamentByName(string entry)
        {
            ITournament tournament = TournamentTable.GetTournamentByName(entry, dbConn);
            if (tournament == null) { return null; }

            var tournamentEntries = TournamentEntryTable.GetAll(dbConn);
            var rounds = RoundsTable.GetAll(dbConn);
            var roundMatchups = RoundMatchupsTable.GetAll(dbConn);
            var matchups = MatchupsTable.GetAll(dbConn);
            var matchupEntries = MatchupEntriesTable.GetAll(dbConn);


            tournament.Participants = tournamentEntries.Where(x => x.TournamentId == tournament.TournamentId).ToList();
            tournament.Rounds = rounds.Where(x => x.TournamentId == tournament.TournamentId).ToList();

            foreach(var round in tournament.Rounds)
            {
                var xref = roundMatchups.Where(x => x.RoundId == round.RoundId).ToList();
                round.Pairings = matchups.Where(x => xref.Select(y => y.MatchupId).ToList().Contains(x.MatchupId)).ToList();
                foreach(var matchup in round.Pairings)
                {
                    matchup.Teams = matchupEntries.Where(x => x.MatchupId == matchup.MatchupId).ToList();
                    foreach(var team in matchup.Teams)
                    {
                        team.TheTeam = tournamentEntries.Where(x => x.TournamentEntryId == team.TournamentEntryId).First();
                    }
                }
            }

            return tournament;
        }
        #endregion

        #region TOURNAMENT ENTRY METHODS
        public List<ITournamentEntry> createTournamentEntries(List<ITournamentEntry> entries)
        {
            List<ITournamentEntry> results = new List<ITournamentEntry>();
            foreach (ITournamentEntry tournamentEntry in entries)
            {
                ITournamentEntry tournament = TournamentEntryTable.Create(tournamentEntry, dbConn);
                results.Add(tournament);
            }

            return results;
        }

        public ITournamentEntry getTournamentEntry(int id)
        {
            ITournamentEntry tournament = TournamentEntryTable.Get(id, dbConn);
            if (tournament == null) { return null; }

            return tournament;
        }

        public List<ITournamentEntry> getTournamentEntriesByTournamentId (int id)
        {
            List<ITournamentEntry> tournamentEntries = TournamentEntryTable.GetByTournamentId(id, dbConn);
            if (tournamentEntries == null) { return null; }

            return tournamentEntries;
        }
        #endregion

        #region ROUND METHODS
        public IRound createRound(IRound entry)
        {
            IRound round = RoundsTable.Create(entry, dbConn);
            if (round == null) { return null; }

            return round;
        }

        public List<IRound> getRoundsByTournamentId(int tournamentId)
        {
            List<IRound> rounds = RoundsTable.GetByTournamentId(tournamentId, dbConn);
            if (rounds == null) { return null; }

            return rounds;
        }

        public IRound getRoundByTournamentIdandRoundNum(IRound entry)
        {
            IRound round = RoundsTable.GetByTournamentIdAndRoundNum(entry, dbConn);
            if (round == null) { return null; }

            return round;
        }
        #endregion

        #region ROUND MATCHUP METHODS
        public IRoundMatchup createRoundMatchup(IRoundMatchup entry)
        {
            IRoundMatchup roundMatchup = RoundMatchupsTable.Create(entry, dbConn);
            if (roundMatchup == null) { return null; }

            return roundMatchup;
        }

        public List<IRoundMatchup> getRoundMatchupsByRoundId(IRoundMatchup entry)
        {
            List<IRoundMatchup> roundMatchups = RoundMatchupsTable.GetByRoundId(entry, dbConn);
            if (roundMatchups == null) { return null; }

            return roundMatchups;
        }

        public IRoundMatchup getRoundMatchupByRoundIdAndMatchupNumber(IRoundMatchup entry)
        {
            IRoundMatchup roundMatchup = RoundMatchupsTable.GetByRoundIdAndMatchupId(entry, dbConn);
            if (roundMatchup == null) { return null; }

            return roundMatchup;
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

        public IUser updateLoginTime(IUser thisUser)
        {
            thisUser.LastLogin = DateTime.Now;
            return UsersTable.Update(thisUser, dbConn);
        }

        public IUser deleteUser(IUser entry)
        {
            return UsersTable.Delete(entry, dbConn);
        }
        #endregion

        #region DASHBOARD METHODS
        public List<ITournament> getAllTournaments()
        {
            return TournamentTable.GetAll(dbConn); ;
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
            foreach(var entity in matchup.Teams)
            {
                MatchupEntriesTable.Update(entity, dbConn);
            }

            return matchup;
        }

        public IMatchup getMatchup(int matchupId)
        {
            IMatchup matchup = MatchupsTable.Get(matchupId, dbConn);
            if (matchup == null) { return null; }

            return matchup;
        }

        public List<IMatchup> getAllMatchups()
        {
            List<IMatchup> matchup = MatchupsTable.GetAll(dbConn);
            if (matchup == null) { return null; }

            return matchup;
        }
        public IMatchup createMatchup(IMatchup entry)
        {
            IMatchup matchup = MatchupsTable.Create(entry, dbConn);
            if (matchup == null) return null;

            return matchup;
        }

        public IMatchup deleteMatchup(IMatchup entry)
        {
            IMatchup matchup = MatchupsTable.Delete(entry, dbConn);
            if (matchup == null) return null;

            return matchup;
        }
        #endregion

        #region MATCHUP ENTRY METHODS
        public IMatchupEntry createMatchupEntry(IMatchupEntry entry)
        {
            IMatchupEntry matchupEntry = MatchupEntriesTable.Create(entry, dbConn);
            if (matchupEntry == null) return null;

            return matchupEntry;
        }

        public List<IMatchupEntry> getMatchupEntriesByMatchupId(int matchupId)
        {
            List<IMatchupEntry> matchupEntries = MatchupEntriesTable.GetByMatchupId(matchupId, dbConn);
            if (matchupEntries == null) return null;

            return matchupEntries;
        }

        public IMatchupEntry updateMatchupEntryScore(int matchupId, int tournamentEntryId, int score)
        {
            IMatchupEntry matchupEntry = MatchupEntriesTable.UpdateScore(matchupId, tournamentEntryId, score, dbConn);
            if (matchupEntry == null) return null;

            return matchupEntry;
        }
        #endregion
    }
}
