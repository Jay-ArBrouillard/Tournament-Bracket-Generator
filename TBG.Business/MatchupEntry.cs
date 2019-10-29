using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class MatchupEntry : IMatchupEntry
    {
        public ITournamentEntry TheTeam { get; set; }
        public double Score { get; set; }
    }
}
