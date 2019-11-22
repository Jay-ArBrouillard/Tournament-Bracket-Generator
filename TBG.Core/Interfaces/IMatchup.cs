using System.Collections.Generic;

namespace TBG.Core.Interfaces
{
    public interface IMatchup
    {
        int MatchupId { get; set; }
        List<IMatchupEntry> Teams { get; set;}
        IMatchup NextRound { get; set; }
    }
}
