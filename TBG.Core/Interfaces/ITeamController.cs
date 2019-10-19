using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITeamController
    {
        bool validateTeam(ITeam thisTeam, ITeam thatTeam);
    }
}
