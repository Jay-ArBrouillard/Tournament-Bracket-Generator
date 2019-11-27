using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class Matchup : IMatchup
    {
        public int MatchupId { get; set; }
        public List<IMatchupEntry> MatchupEntries { get; set; } = new List<IMatchupEntry>();
        public IMatchup NextRound { get; set; }
        public bool Completed { get; set; }
        public int RoundId { get; set; }
    }
}
