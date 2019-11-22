using System.Collections.Generic;

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
