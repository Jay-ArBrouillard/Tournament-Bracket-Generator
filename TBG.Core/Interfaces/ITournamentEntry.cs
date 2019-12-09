using System.Collections.ObjectModel;

namespace TBG.Core.Interfaces
{
    public interface ITournamentEntry
    {
        int TournamentEntryId { get; set; }
        int TournamentId { get; set; }
        int TeamId { get; set; }
        double Seed { get; set; }
        //For results page
        int Wins { get; set; }
        int Losses { get; set; }
        ObservableCollection<IPerson> Members { get; set; }
    }
}
