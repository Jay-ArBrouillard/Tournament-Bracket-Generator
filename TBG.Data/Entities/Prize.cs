using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Prize : IPrize
    {
        public int PrizeId { get; set; }
        public string PrizeName { get; set; }
        public decimal PrizeAmount { get; set; }
        public decimal PrizePercent { get; set; }
        public int PlaceNumber { get; set; }
    }
}
