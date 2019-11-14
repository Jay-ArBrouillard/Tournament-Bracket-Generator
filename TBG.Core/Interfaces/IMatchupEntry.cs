using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface IMatchupEntry
    {
        int MatchupEntryId { get; set; }
        int MatchupId { get; set; }
        int TournamentEntryId { get; set; }
        ITournamentEntry TheTeam { get; set; }
        double Score { get; set; }
    }
}
