using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class TournamentPrize : ITournamentPrize
    {
        public int TournamentPrizeId { get; set; }
        public int TournamentId { get; set; }
        public int PrizeId { get; set; }
        public int PlaceId { get; set; }
    }
}
