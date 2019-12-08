using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class Prize : IPrize
    {
        /// <summary>
        /// Represents the place number for this prize ex: 1, 2, 3...
        /// </summary>
        public int PrizeId { get; set; }

        /// <summary>
        /// Represents the place name for this prize ex: 1st, Champion, RunnerUp...
        /// </summary>
        public string PrizeName { get; set; }

        /// <summary>
        /// Represents the prize amount for this prize
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// Represents the prize amount as a percent of a larger prize pool ex: 1% of 1 million
        /// </summary>
        public decimal PrizePercent { get; set; }

        /// <summary>
        /// Represents the place number
        /// </summary>
        public int PlaceNumber { get; set; }
    }
}
