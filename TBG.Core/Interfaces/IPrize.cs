using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
