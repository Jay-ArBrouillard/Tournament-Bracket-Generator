using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Team : ITeam
    {
        public string TeamName { get; set; }
        public List<IPerson> TeamMembers { get; set; }
        public int Wins { get; }
        public int Losses { get; }

        public Team(string teamName, int wins = 0, int losses = 0)
        {
            TeamName = teamName;
            Wins = wins;
            Losses = losses;
        }
    }
}
