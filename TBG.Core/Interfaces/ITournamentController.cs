using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITournamentController
    {
        bool validateEntryFee(string number);
        ITournament createTournament(ITournament tournament);
        bool validateTournamentType(ITournamentType tournamentType);
        List<ITournamentEntry> ConvertITournmentEntries(List<ITournamentEntry> tournmentEntries, ITournament tournament);
    }
}
