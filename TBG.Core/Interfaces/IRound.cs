using System.Collections.Generic;

namespace TBG.Core.Interfaces
{
    public interface IRound
    {
        int RoundId { get; set; }
        int RoundNum { get; set; }
        int TournamentId { get; set; }
        List<IMatchup> Pairings { get; set; }
    }
}
