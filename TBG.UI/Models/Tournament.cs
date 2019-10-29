using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Models
{
    public class Tournament : ITournament
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public decimal EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public List<ITournamentEntry> Participants { get; set; }
        public List<ITournamentPrize> Prizes { get; set; }
        public List<IRound> Rounds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool BuildTournament()
        {
            throw new NotImplementedException();
        }
    }
}
