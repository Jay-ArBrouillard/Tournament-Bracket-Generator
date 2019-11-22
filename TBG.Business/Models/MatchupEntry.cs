using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class MatchupEntry : IMatchupEntry
    {
        public int MatchupEntryId { get; set; }
        public int MatchupId { get; set; }
        public int TournamentEntryId { get; set; }
        public ITournamentEntry TheTeam { get; set; }
        public double Score { get; set; }
    }
}
