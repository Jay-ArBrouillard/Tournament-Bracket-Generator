using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Tournament : ITournament
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public decimal EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public List<ITeam> Participants { get; set; }
        public List<ITournamentPrize> Prizes { get; set; }
        public List<IRound> Rounds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<ITournamentEntry> ITournament.Participants { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<IPrize> ITournament.Prizes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool BuildTournament()
        {
            throw new NotImplementedException();
        }
        public bool RecordResult(IMatchup matchup)
        {
            throw new NotImplementedException("This should probably change?");
        }
    }
}
