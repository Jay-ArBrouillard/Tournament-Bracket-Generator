using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Models
{
    public class SingleEliminationTournament : ITournament
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public decimal EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public List<ITournamentEntry> Participants { get; set; } = new List<ITournamentEntry>();
        public List<IPrize> Prizes { get; set; } = new List<IPrize>();
        public List<IRound> Rounds { get; set; } = new List<IRound>();

        public bool RecordResult(IMatchup matchup)
        {
            throw new NotImplementedException();
        }
    }
}
