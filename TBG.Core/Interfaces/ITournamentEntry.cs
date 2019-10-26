using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITournamentEntry
    {
        int TournamentEntryId { get; set; }
        int TournamentId { get; set; }
        int TeamId { get; set; }
        int Seed { get; set; }
    }
}
