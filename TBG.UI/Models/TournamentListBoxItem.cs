using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Models
{
    public class TournamentListBoxItem
    {
        public string Name { get; set; }
        public int TournamentId { get; set; }
        public int TournamentTypeId { get; set; }
        public double PrizePool { get; set; }
        public List<ITeam> Teams { get; set; }
    }
}
