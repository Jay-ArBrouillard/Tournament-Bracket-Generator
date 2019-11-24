using System;
using System.Collections.Generic;
using TBG.Business.Helpers;
using TBG.Business.Models;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class TournamentController : ITournamentController
    {
        public ITournament createTournament(
            string tournamentName, 
            ITournamentType tournamentType, 
            int userId, 
            double entryFee, 
            double totalPrizePool,
            List<ITournamentEntry> participants)
        {
            var tournament = TournamentTypeHelper.GetNewTournament(tournamentType);
            tournament.TournamentName = tournamentName;
            tournament.EntryFee = entryFee;
            tournament.TotalPrizePool = totalPrizePool;
            tournament.UserId = userId;
            tournament.Participants = participants;

            return tournament.BuildTournament();
        }

        public bool validateTournamentName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return false; }
            return true;
        }

        public double validateEntryFee(string number)
        {
            double result;
            if (string.IsNullOrEmpty(number) || !double.TryParse(number, out result)) { return -1; }
            return result;
        }

        public bool validateTournamentType(ITournamentType type)
        {
            if (type == null) { return false; }
            return true;
        }

        public bool validateParticipantCount(int count)
        {
            if (count <= 0) { return false; }
            return true;
        }

        public double validateTotalPrizePool(string pool, int numParticipants, double entryFee)
        {
            double poolDouble;
            if (!double.TryParse(pool, out poolDouble))
            {
                return -1;
            }

            if (Math.Abs(poolDouble - (numParticipants * entryFee)) > 0.01)
            {
                return -1;
            }
            return poolDouble;
        }

        public ITournament rebuildTournament(ITournament savedTournament)
        {
            var tournament = TournamentTypeHelper.ConvertTournamentType(savedTournament);
            tournament.RebuildTournament();
            return tournament;
        }
    }
}
