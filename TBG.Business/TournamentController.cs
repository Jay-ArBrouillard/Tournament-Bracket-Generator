﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class TournamentController : ITournamentController
    {
        public ITournament createTournament(ITournament tournament)
        {
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

        public bool validateSingleEliminationTournament(ITournament tournament)
        {
            string tournamentName = tournament.TournamentName;
            if (string.IsNullOrEmpty(tournamentName)) { return false; }

            int numParticipants = tournament.Participants.Count;
            if (numParticipants == 0)
            {
                return false;
            }
            else
            {
                //Validate totalPrizePool calculated correctly
                double entryFee = tournament.EntryFee;
                double totalPrizePool = tournament.TotalPrizePool;
                if (Math.Abs(totalPrizePool - (numParticipants * entryFee)) > 0.01)
                {
                    return false;
                }
            }

            return true;
        }

        public List<ITournamentEntry> ConvertITournmentEntries(List<ITournamentEntry> tournmentEntries, ITournament tournament)
        {
            List<ITournamentEntry> results = new List<ITournamentEntry>();
            foreach (ITournamentEntry entry in tournmentEntries)
            {
                ITournamentEntry tournamentEntry = new TournamentEntry()
                {
                    TournamentId = tournament.TournamentId,
                    TeamId = entry.TeamId,
                    Seed = 0    //CHANGE later when seeding is implemented
                };
                results.Add(tournamentEntry);
            }

            return results;
        }
    
    }
}
