using System;
using System.Collections.Generic;
using TBG.Business.Helpers;
using TBG.Business.Models;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class TournamentController : ITournamentController
    {
        public ITournament createTournament(ITournament tournament)
        {
            string tournamentName = tournament.TournamentName;
            if (string.IsNullOrEmpty(tournamentName)) { return null; }

            int numParticipants = tournament.Participants.Count;
            if (numParticipants == 0)
            {
                return null;
            }
            else
            {
                //Validate totalPrizePool calculated correctly
                double entryFee = tournament.EntryFee;
                double totalPrizePool = tournament.TotalPrizePool;
                if (Math.Abs(totalPrizePool - (numParticipants * entryFee)) > 0.01)
                {
                    return null;
                }
            }

            var convertedTournament = TournamentTypeHelper.ConvertTournamentType(tournament);
            return convertedTournament.BuildTournament();
        }

        public bool validateEntryFee(string number)
        {
            if (string.IsNullOrEmpty(number)) { return false; }

            if (!int.TryParse(number, out _) && !double.TryParse(number, out _) && !decimal.TryParse(number, out _)) { return false; }
            return true;
        }

        public bool validateTournamentType(ITournamentType type)
        {
            if (type == null) { return false; }

            String tournamentName = type.TournamentTypeName;

            if (string.IsNullOrEmpty(tournamentName)) { return false; }

            if (!tournamentName.Equals("Single Elimination")) { return false; }
            return true;
        }
    
    }
}
