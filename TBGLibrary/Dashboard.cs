using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public class Dashboard
    {

        /// <summary>
        /// Represents all existing tournaments.
        /// </summary>
        public List<Tournament> Tournaments { get; set; } = new List<Tournament>();

        /// <summary>
        /// Represents only the tournaments fully completed.
        /// </summary>
        public List<Tournament> TournamentsCompleted { get; set; } = new List<Tournament>();

        /// <summary>
        /// Represents only the tournaments not completed.
        /// </summary>
        public List<Tournament> TournamentsPending { get; set; } = new List<Tournament>();
    }
}
