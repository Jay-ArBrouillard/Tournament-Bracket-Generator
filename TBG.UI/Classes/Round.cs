using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Classes
{
    public class Round : IRound
    {
        public int RoundId { get; set; }
        public int RoundNum { get; set; }
        public int TournamentId { get; set; }
        public List<IMatchup> Pairings { get; set; }
        public Round()
        {
        }

        public Round(int tournamentId, int roundNum)
        {
            TournamentId = tournamentId;
            RoundNum = roundNum;
        }
    }
}
