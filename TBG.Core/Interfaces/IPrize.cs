using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface IPrize
    {
        int PlaceNumber { get; set; }
        string PlaceName { get; set; }
        decimal PrizeAmount { get; set; }
        double PrizePercent { get; set; }
    }
}
