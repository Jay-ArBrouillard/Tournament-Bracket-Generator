using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Classes
{
    public class Prize : IPrize
    {
        public int PrizeId { get; set; }
        public string PrizeName { get; set; }
        public decimal PrizeAmount { get; set; }
        public decimal PrizePercent { get; set; }

    }
}
