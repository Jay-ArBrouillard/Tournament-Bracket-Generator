using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class TournamentController : ITournamentController
    {
        public ITournament createSingleEliminationTournament(ITournament tournament)
        {
            ITournament newTournament = new SingleEliminationTournament();
            newTournament.Participants = tournament.Participants;
            //TOdo add all properties

            bool valid = newTournament.BuildTournament();

            if (valid)
            {
                return newTournament;
            }

            return null;
        }

        public bool validateEntryFee(string entryFee)
        {
            if (!int.TryParse(entryFee, out _)) { return false; }

            return true;
        }
    }
}
