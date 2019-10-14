using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public class Dashboard
    {
        public List<Tournament> Tournaments { get; set; } = new List<Tournament>();

        public List<Tournament> TournamentsCompleted { get; set; } = new List<Tournament>();

        public List<Tournament> TournamentsPending { get; set; } = new List<Tournament>();
    }
}
