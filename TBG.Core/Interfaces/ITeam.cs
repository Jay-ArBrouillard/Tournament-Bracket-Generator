using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITeam
    {
        int TeamId { get; set; }
        string TeamName { get; set; }
        List<IPerson> TeamMembers { get; set; }
        int Wins { get; set; }
        int Losses { get; set; }
    }
}
