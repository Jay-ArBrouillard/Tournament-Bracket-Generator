using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITournamentController
    {
        bool validateEntryFee(string entryFee);
        ITournament createSingleEliminationTournament(ITournament tournament);
        bool validateSingleEliminationTournament(ITournament tournament);
        bool validateTournamentType(string text);
        bool validateTotalPrizePool(string text);
    }
}
