﻿using TBG.Business.Tournaments;
using TBG.Core.Interfaces;

namespace TBG.Business.Helpers
{
    public static class TournamentTypeHelper
    {
        public static ITournament ConvertTournamentType(ITournament tournament)
        {
            switch (tournament.TournamentTypeId)
            {
                case 1: return ConvertToSingleElimination(tournament);
                default: return null;
            }
        }

        public static SingleEliminationTournament ConvertToSingleElimination(ITournament tournament)
        {
            SingleEliminationTournament converted = new SingleEliminationTournament()
            {
                TournamentId = tournament.TournamentId,
                UserId = tournament.UserId,
                TournamentName = tournament.TournamentName,
                EntryFee = tournament.EntryFee,
                TotalPrizePool = tournament.TotalPrizePool,
                TournamentTypeId = tournament.TournamentTypeId,
                Participants = tournament.Participants,
                Prizes = tournament.Prizes,
                Rounds = tournament.Rounds
            };

            return converted;
        }

    }
}