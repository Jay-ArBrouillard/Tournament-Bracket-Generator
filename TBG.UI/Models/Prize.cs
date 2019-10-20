using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Models
{
    public class Prize : IPrize
    {

        public Prize(string PrizeName, decimal PrizePercent = 0, decimal PrizeAmount = 0)
        {
            this.PrizeName = PrizeName;
            this.PrizeAmount = PrizeAmount;
            this.PrizePercent = PrizePercent;
        }

        public int PrizeId { get; set; }
        public string PrizeName { get; set; }
        public decimal PrizeAmount { get; set; }
        public decimal PrizePercent { get; set; }

    }
}
