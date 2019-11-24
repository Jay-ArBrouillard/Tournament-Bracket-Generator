using System.Collections.Generic;

namespace TBG.Core.Interfaces
{
    public interface ITournament
    {
        int TournamentId { get; set; }
        int UserId { get; set; }
        string TournamentName { get; set; }
        double EntryFee { get; set; }
        double TotalPrizePool { get; set; }
        int TournamentTypeId { get; set; }
        List<ITeam> Teams { get; set; }
        List<ITournamentEntry> Participants { get; set; }
        List<IPrize> Prizes { get; set; }
        List<IRound> Rounds { get; set; }
        bool RecordResult(IMatchup matchup);
        ITournament BuildTournament();
        ITournament RebuildTournament();
    }
}
