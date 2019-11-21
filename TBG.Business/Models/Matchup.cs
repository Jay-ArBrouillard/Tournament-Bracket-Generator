using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class Matchup : IMatchup
    {
        public int MatchupId { get; set; }
        public List<IMatchupEntry> Teams { get; set; }
        public IMatchup NextRound { get; set; }

        public Matchup()
        {
            Teams = new List<IMatchupEntry>();
        }
    }
}
