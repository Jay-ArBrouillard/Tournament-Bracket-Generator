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
        public string TeamName { get; set;}
        public List<IPerson> TeamMembers { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
