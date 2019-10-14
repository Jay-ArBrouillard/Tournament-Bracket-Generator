using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBGLibrary
{
    public class Dashboard
    {

        /// <summary>
        /// Represents all existing tournaments.
        /// </summary>
        public List<ITournament> Tournaments { get; set; } = new List<ITournament>();

        /// <summary>
        /// Represents only the tournaments fully completed.
        /// </summary>
        public List<ITournament> TournamentsCompleted { get; set; } = new List<ITournament>();

        /// <summary>
        /// Represents only the tournaments not completed.
        /// </summary>
        public List<ITournament> TournamentsPending { get; set; } = new List<ITournament>();
    }
}
