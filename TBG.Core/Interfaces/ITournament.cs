using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITournament
    {
        string TournamentName { get; set; }
        List<ITeam> Participants { get; set; }
        decimal EntryFee { get; set; }
        List<IPrize> Prizes { get; set; }
    }
}
