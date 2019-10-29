using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    class TournamentEntry : ITournamentEntry
    {
        public int TournamentEntryId { get; set; }
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public int Seed { get; set; }
    }
}
