using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Classes
{
    public class RoundMatchup : IRoundMatchup
    {
        public int RoundMatchupId { get; set; }
        public int RoundId { get; set; }
        public int MatchupId { get; set; }

        public RoundMatchup(int roundId)
        {
            RoundId = roundId;
        }
    }
}
