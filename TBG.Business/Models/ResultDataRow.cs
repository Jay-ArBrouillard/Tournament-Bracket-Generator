using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class ResultDataRow : IResultDataRow
    {
        public String TeamName { get; set; }
        public String Players { get; set; }
        public String Opponent { get; set; }
        public int Placing { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double WinLoss { get; set; }
        public int CareerWins { get; set; }
        public int CareerLosses { get; set; }
        public double CareerWinLoss { get; set; }
        public decimal Winnings { get; set; }
    }
}
