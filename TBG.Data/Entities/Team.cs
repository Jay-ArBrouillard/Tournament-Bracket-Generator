using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Team : ITeam
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public List<IPerson> TeamMembers { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public Team()
        {

        }

        public Team(string teamName, int wins = 0, int losses = 0)
        {
            TeamName = teamName;
            Wins = wins;
            Losses = losses;
        }
    }
}
