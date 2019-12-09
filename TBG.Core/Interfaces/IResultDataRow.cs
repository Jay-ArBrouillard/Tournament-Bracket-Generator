using System;

namespace TBG.Core.Interfaces
{
    public interface IResultDataRow
    {
        String TeamName { get; set; }
        String Players { get; set; }
        String Opponent { get; set; }
        int Placing { get; set; }
        int Wins { get; set; }
        int Losses { get; set; }
        double WinLoss { get; set; }
        int CareerWins { get; set; }
        int CareerLosses { get; set; }
        double CareerWinLoss { get; set; }
        decimal Winnings { get; set; }
    }
}
