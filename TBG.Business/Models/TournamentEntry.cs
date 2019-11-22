using System.Collections.ObjectModel;
using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class TournamentEntry : ITournamentEntry
    {
        public int TournamentEntryId { get; set; }
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public double Seed { get; set; }
        public ObservableCollection<IPerson> Members { get; set; }
    }
}
