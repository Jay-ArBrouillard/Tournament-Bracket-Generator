using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Matchup : IMatchup
    {
        public int MatchupId { get; set; }
        public bool Completed { get; set; }
        public int RoundId { get; set; }
        public List<IMatchupEntry> MatchupEntries { get; set; }
    }
}
