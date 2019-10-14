using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public class MatchupEntry
    {
        /// <summary>
        /// Represents one team in a matchup.
        /// </summary>
        public Team TeamCompeting { get; set; }

        /// <summary>
        /// Represents this teams score.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Represents the matchup that this team came from as the winner.
        /// </summary>
        public Matchup ParentMatchup { get; set; }
    }
}
