namespace TBG.Core.Interfaces
{
    public interface ITournamentPrize
    {
        int TournamentPrizeId { get; set; }
        int TournamentId { get; set; }
        int PrizeId { get; set; }
        int PlaceId { get; set; }
    }
}
