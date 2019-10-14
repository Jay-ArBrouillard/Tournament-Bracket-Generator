using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class Prize : IPrize
    {
        /// <summary>
        /// Represents the place number for this prize ex: 1, 2, 3...
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// Represents the place name for this prize ex: 1st, Champion, RunnerUp...
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Represents the prize amount for this prize
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// Represents the prize amount as a percent of a larger prize pool ex: 1% of 1 million
        /// </summary>
        public double PrizePercent { get; set; }
    }
}
