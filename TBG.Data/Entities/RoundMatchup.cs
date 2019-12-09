using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class RoundMatchup : IRoundMatchup
    {
        public int RoundMatchupId { get; set; }
        public int RoundId { get; set; }
        public int MatchupId { get; set; }
        public int MatchupNumber { get; set; }
        public RoundMatchup()
        {
        }
        public RoundMatchup(int roundId, int matchupId, int i)
        {
            RoundId = roundId;
            MatchupId = matchupId;
            MatchupNumber = i;
        }
    }
}
