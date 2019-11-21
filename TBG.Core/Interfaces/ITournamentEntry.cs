using System.Collections.ObjectModel;

namespace TBG.Core.Interfaces
{
    public interface ITournamentEntry
    {
        int TournamentEntryId { get; set; }
        int TournamentId { get; set; }
        int TeamId { get; set; }
        int Seed { get; set; }
        ObservableCollection<IPerson> Members { get; set; }
    }
}
