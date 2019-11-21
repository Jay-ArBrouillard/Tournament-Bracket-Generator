using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class TeamController : ITeamController
    {
        public bool validateTeam(ITeam thisTeam, ITeam thatTeam)
        {
            if (thisTeam == null) { return false; }

            if (thisTeam != null && thatTeam != null)
            {
                if (thisTeam.TeamName.Equals(thatTeam.TeamName)) { return false; }  //Team name duplicate
            }

            return true;
        }
    }
}
