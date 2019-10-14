using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public class SingleEliminationTournament : Tournament
    {
        public override string TournamentName { get; set; }

        public override List<Team> Participants { get; set; } = new List<Team>();

        public override int EntryFee { get; set; }

        public override List<Prize> Prizes { get; set; } = new List<Prize>();

        public List<List<Matchup>> Rounds { get; set; } = new List<List<Matchup>>();

    }
}
