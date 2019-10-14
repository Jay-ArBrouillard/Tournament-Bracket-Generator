using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public class SingleEliminationTournament : Tournament
    {
        /// <summary>
        /// Represents the tournament name for this tournament.
        /// </summary>
        public override string TournamentName { get; set; }

        /// <summary>
        /// Represents the list of players in this tournament.
        /// </summary>
        public override List<Team> Participants { get; set; } = new List<Team>();

        /// <summary>
        /// Represents the entry fee each team pays to join this tornament.
        /// </summary>
        public override int EntryFee { get; set; }

        /// <summary>
        /// Represents the list of prizes in this tournament.
        /// </summary>
        public override List<Prize> Prizes { get; set; } = new List<Prize>();

        /// <summary>
        /// Represents the every possible matchup in this tournament.
        /// </summary>
        public List<List<Matchup>> Rounds { get; set; } = new List<List<Matchup>>();

    }
}
