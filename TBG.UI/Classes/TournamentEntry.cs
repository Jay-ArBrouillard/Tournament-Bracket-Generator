using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Classes
{
    public class TournamentEntry : ITournamentEntry
    {
        public int TournamentEntryId { get; set; }
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public double Seed { get; set; }
        public ObservableCollection<IPerson> Members { get; set; }
        //Need for XAML
        public string TeamName { get; set; }

        public TournamentEntry()
        {
        }

        public TournamentEntry(int tournamentId, int teamId)
        {
            TournamentId = tournamentId;
            TeamId = teamId;
        }
    }
}
