using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class Round : IRound
    {
        public int RoundId { get; set; }
        public int RoundNum { get; set; }
        public int TournamentId { get; set; }
        public List<IMatchup> Matchups { get; set; }

        public Round()
        {
            Matchups = new List<IMatchup>();
        }
    }
}
