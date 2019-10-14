using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITeam
    {
        string TeamName { get; set; }
        List<IPerson> TeamMembers { get; set; }
        int Wins { get; }
        int Losses { get; }
    }
}
