using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Matchup : IMatchup
    {
        public int MatchupId { get; set; }
        public List<IMatchupEntry> Teams { get; set; }
        public IMatchup NextRound { get; set; }
        public bool Completed { get; set; }
    }
}
