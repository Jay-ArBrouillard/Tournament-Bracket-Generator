namespace TBG.Core.Interfaces
{
    public interface IPrize
    {
        int PrizeId { get; set; }
        string PrizeName { get; set; }
        decimal PrizeAmount { get; set; }
        decimal PrizePercent { get; set; }
    }
}
