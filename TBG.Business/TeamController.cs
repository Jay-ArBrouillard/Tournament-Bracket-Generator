using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class TeamController : ITeamController
    {
        public bool validateTeam(ITeam thisTeam, ITeam thatTeam)
        {
            if (thisTeam == null) { return false; }

            if (thisTeam.TeamMembers.Count == 0) { return false; }

            if (thisTeam != null && thatTeam != null)
            {
                if (thisTeam.TeamName.Equals(thatTeam.TeamName)) { return false; }  //Team name duplicate
            }

            return true;
        }
    }
}
