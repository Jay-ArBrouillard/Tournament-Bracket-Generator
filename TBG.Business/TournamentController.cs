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
                return thisTournament;
            }

            return null;
        }

        public bool validateEntryFee(string number)
        {
            if (string.IsNullOrEmpty(number)) { return false; }

            if (!int.TryParse(number, out _) && !double.TryParse(number, out _) && !decimal.TryParse(number, out _)) { return false; }
            return true;
        }

        public bool validateTournamentType(ITournamentType type)
        {
            if (type == null) { return false; }

            String tournamentName = type.TournamentTypeName;

            if (string.IsNullOrEmpty(tournamentName)) { return false; }

            if (!tournamentName.Equals("Single Elimination")) { return false; }
            return true;
        }

        public bool validateSingleEliminationTournament(ITournament tournament)
        {
            string tournamentName = tournament.TournamentName;
            if (string.IsNullOrEmpty(tournamentName)) { return false; }

            int numParticipants = tournament.Participants.Count;
            if (numParticipants == 0)
            {
                return false;
            }
            else
            {
                //Validate totalPrizePool calculated correctly
                double entryFee = tournament.EntryFee;
                double totalPrizePool = tournament.TotalPrizePool;
                if (Math.Abs(totalPrizePool - (numParticipants * entryFee)) > 0.01)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
