using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBGLibrary
{
    public class SingleEliminationTournament : ITournament
    {
        /// <summary>
        /// Represents the tournament name for this tournament.
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// Represents the list of players in this tournament.
        /// </summary>
        public List<ITeam> Participants { get; set; } = new List<ITeam>();

        /// <summary>
        /// Represents the entry fee each team pays to join this tornament.
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// Represents the list of prizes in this tournament.
        /// </summary>
        public List<IPrize> Prizes { get; set; } = new List<IPrize>();

        /// <summary>
        /// Represents the every possible matchup in this tournament.
        /// </summary>
        public List<List<Matchup>> Rounds { get; set; } = new List<List<Matchup>>();

    }
}
