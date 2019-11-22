namespace TBG.Core.Interfaces
{
    public interface IRoundMatchup
    {
        int RoundMatchupId { get; set; }
        int RoundId { get; set; }
        int MatchupId { get; set; }
        int MatchupNumber { get; set; }
    }
}
