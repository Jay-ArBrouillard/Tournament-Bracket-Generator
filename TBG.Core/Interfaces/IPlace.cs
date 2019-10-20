using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface IPlace
    {
        int PlaceId { get; set; }
        string PlaceName { get; set; }
        int PlaceNumber { get; set; }
    }
}
