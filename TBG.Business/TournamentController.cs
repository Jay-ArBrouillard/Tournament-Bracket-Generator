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
            SingleEliminationTournament thisTournament = new SingleEliminationTournament(tournament);

            bool valid = thisTournament.BuildTournament();

            if (valid)
            {
                return tournament;
            }

            return null;
        }

        public bool validateEntryFee(string entryFee)
        {
            if (string.IsNullOrEmpty(entryFee)) { return true; }

            if (!int.TryParse(entryFee, out _)) { return false; }
            return true;
        }


        public bool validateTotalPrizePool(string prizePool)
        {
            if (!int.TryParse(prizePool, out _)) { return false; }
            return true;
        }

        public bool validateTournamentType(string type)
        {
            if (string.IsNullOrEmpty(type)) { return false; }

            if (!type.Equals("Single Elimination")) { return false; }
            return true;
        }

        public bool validateSingleEliminationTournament(ITournament tournament)
        {
            return false;
        }

    }
}
