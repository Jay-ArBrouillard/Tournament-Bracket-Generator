using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.UI.Models
{
    public class TournamentViewDataGridItem
    {
        public String PlayerName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TeamId { get; set; }
    }
}
