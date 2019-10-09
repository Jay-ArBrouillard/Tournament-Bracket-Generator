using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Classes.Participants
{
    public class Team
    {
        public string name { get; set; }
        public int size { get; set; }
        public List<Player> players { get; set; }

        public Team (string name, int size, int publicSeed)
        {
            this.name = name;
            this.size = size;
        }

    }
}
