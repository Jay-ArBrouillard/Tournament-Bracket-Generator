using System.Collections.Generic;

namespace TBG.Core.Interfaces
{
    public interface IMatchup
    {
        int MatchupId { get; set; }
        int RoundId { get; set; }
        bool Completed { get; set; }
        List<IMatchupEntry> MatchupEntries { get; set; }
    }
}
