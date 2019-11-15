using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface IRoundMatchup
    {
        int RoundMatchupId { get; set; }
        int RoundId { get; set; }
        int MatchupId { get; set; }
    }
}
