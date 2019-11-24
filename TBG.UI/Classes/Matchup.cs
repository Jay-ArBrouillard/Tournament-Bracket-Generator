using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Classes
{
    public class Matchup : IMatchup
    {
        public int MatchupId { get; set; }
        public List<IMatchupEntry> Teams { get; set; }
        public IMatchup NextRound { get; set; }

        public Matchup()
        {
        }

        public Matchup(int matchupId)
        {
            MatchupId = matchupId;
        }
    }
}
