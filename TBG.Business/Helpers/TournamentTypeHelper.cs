﻿using TBG.Business.Interfaces;
using TBG.Business.Tournaments;
using TBG.Core.Interfaces;

namespace TBG.Business.Helpers
{
    public static class TournamentTypeHelper
    {
        public static ITournamentApplication GetNewTournament(ITournamentType type)
        {
            ITournamentApplication newTournament;
            switch (type?.TournamentTypeId)
            {
                case 1:
                    newTournament = NewSingleEliminationTournament();
                    break;
                default:
                    newTournament = null;
                    break;
            }

            if (newTournament != null)
            {
                newTournament.TournamentTypeId = type.TournamentTypeId;
            }

            return newTournament;
        }
        public static ITournamentApplication ConvertTournamentType(ITournament tournament)
        {
            switch (tournament.TournamentTypeId)
            {
                case 1: return ConvertToSingleElimination(tournament);
                default: return null;
            }
        }

        private static SingleEliminationTournament ConvertToSingleElimination(ITournament tournament)
        {
            SingleEliminationTournament converted = new SingleEliminationTournament()
            {
                TournamentId = tournament.TournamentId,
                UserId = tournament.UserId,
                TournamentName = tournament.TournamentName,
                EntryFee = tournament.EntryFee,
                TotalPrizePool = tournament.TotalPrizePool,
                TournamentTypeId = tournament.TournamentTypeId,
                TournamentEntries = tournament.TournamentEntries,
                Prizes = tournament.TournamentPrizes,
                Rounds = tournament.Rounds,
                ActiveRound = tournament.ActiveRound,
                Teams = tournament.Teams
            };

            return converted;
        }

        private static SingleEliminationTournament NewSingleEliminationTournament()
        {
            return new SingleEliminationTournament();
        }
    }
}
