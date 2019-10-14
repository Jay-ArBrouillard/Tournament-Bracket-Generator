using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public class Team
    {
        public string TeamName { get; set; }

        public List<Person> TeamMembers { get; set; } = new List<Person>();

        public int Seed { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }
    }
}
