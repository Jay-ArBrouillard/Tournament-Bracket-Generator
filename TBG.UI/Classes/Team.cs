using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Classes
{ 
    public class Team : ITeam
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public List<IPerson> TeamMembers { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public Team(string teamName, List<IPerson> teamMembers, int wins = 0, int losses = 0)
        {
            TeamName = teamName;
            TeamMembers = teamMembers;
            Wins = wins;
            Losses = losses;
        }
    }

}
