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
            ITournament newTournament = new SingleEliminationTournament()
            {
                TournamentName = tournament.TournamentName,
                UserId = tournament.UserId,
                EntryFee = tournament.EntryFee,
                TotalPrizePool = tournament.TotalPrizePool,
                TournamentTypeId = tournament.TournamentTypeId,
                Participants = tournament.Participants
        };

            /*
             * 
             *         int TournamentId { get; set; }
        int UserId { get; set; }
        string TournamentName { get; set; }
        decimal EntryFee { get; set; }
        double TotalPrizePool { get; set; }
        int TournamentTypeId { get; set; }
        List<ITournamentEntry> Participants { get; set; }
        List<IPrize> Prizes { get; set; }
        List<IRound> Rounds { get; set; }*/

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
