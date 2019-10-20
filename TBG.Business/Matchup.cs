using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Business
{
    public class Matchup
    {
        /// <summary>
        /// Represents two teams playing against each other in a particular tournament.
        /// </summary>
        public List<MatchupEntry> Entries { get; set; } = new List<MatchupEntry>();

        /// <summary>
        /// The winner out of the two teams playing.
        /// </summary>
        public Team Winner { get; set; }

        /// <summary>
        /// The round number in the bracket.
        /// </summary>
        public int MatchupRound { get; set; }
    }
}
