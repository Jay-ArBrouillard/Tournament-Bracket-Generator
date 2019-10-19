using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Place : IPlace
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public int PlaceNumber { get; set; }
    }
}
