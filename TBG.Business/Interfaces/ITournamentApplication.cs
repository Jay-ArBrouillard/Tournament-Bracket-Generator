using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business.Interfaces
{
    public interface ITournamentApplication : ITournament
    {
        ITournament BuildTournament();
        ITournament RebuildTournament();
        ITournament AdvanceRound();
    }
}
