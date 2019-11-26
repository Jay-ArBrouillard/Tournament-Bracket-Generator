using System.Collections.Generic;

namespace TBG.Core.Interfaces
{
    public interface ITournamentController
    {
        double validateEntryFee(string number);
        bool validateTournamentType(ITournamentType tournamentType);
        bool validateTournamentName(string name);
        bool validateParticipantCount(int count);
        double validateTotalPrizePool(string pool, int numParticipants, double EntryFee);
        int validateScore(string score);
        bool validateRoundCompletion(IRound round);
        ITournament createTournament(
            string tournamentName, 
            ITournamentType tournamentTypeId, 
            int userId, 
            double entryFee, 
            double totalPrizePool,
            List<ITournamentEntry> participants
        );
        ITournament rebuildTournament(ITournament savedTournament);
        bool ScoreMatchup(IMatchup matchup, int team1Score, int team2Score);
        ITournament advanceRound(ITournament tournament);
    }
}
