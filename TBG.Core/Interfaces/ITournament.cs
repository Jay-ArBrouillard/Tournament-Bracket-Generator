using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITournament
    {
        int TournamentId { get; set; }
        int UserId { get; set; }
        string TournamentName { get; set; }
        decimal EntryFee { get; set; }
        double TotalPrizePool { get; set; }
        int TournamentTypeId { get; set; }
        List<ITournamentEntry> Participants { get; set; }
        List<ITournamentPrize> Prizes { get; set; }
    }
}
