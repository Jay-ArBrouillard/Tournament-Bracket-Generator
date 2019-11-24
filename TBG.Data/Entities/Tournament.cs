using System;
using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Tournament : ITournament
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public double EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public List<ITeam> Teams { get; set; }
        public List<ITeam> Participants { get; set; }
        public List<ITournamentPrize> Prizes { get; set; }
        public List<IRound> Rounds { get; set; }
        List<ITournamentEntry> ITournament.Participants { get; set; }
        List<IPrize> ITournament.Prizes { get; set; }

        public ITournament BuildTournament()
        {
            throw new NotImplementedException();
        }

        public ITournament RebuildTournament()
        {
            throw new NotImplementedException();
        }

        public bool RecordResult(IMatchup matchup)
        {
            throw new NotImplementedException("This should probably change?");
        }
    }
}
