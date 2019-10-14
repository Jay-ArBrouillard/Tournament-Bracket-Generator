using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public abstract class Tournament
    {
        public abstract string TournamentName { get; set; }
        public abstract List<Team> Participants { get; set; }
        public abstract int EntryFee { get; set; }
        public abstract List<Prize> Prizes { get; set; }
    }
}
