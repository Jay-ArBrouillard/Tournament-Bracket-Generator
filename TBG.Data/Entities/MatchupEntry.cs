using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class MatchupEntry : IMatchupEntry
    {
        public int MatchupEntryId { get; set; }
        public int MatchupId { get; set; }
        public ITournamentEntry TheTeam { get; set; }
        public double Score { get; set; }
        public int TournamentEntryId { get; set; }

        public MatchupEntry()
        {
        }

        public MatchupEntry(int matchupId, int tournamentEntryId)
        {
            MatchupId = matchupId;
            TournamentEntryId = tournamentEntryId;
        }
    }
}
