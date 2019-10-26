using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Models
{
    public class TournamentEntryView : ITournamentEntry
    {
        public int TournamentEntryId { get; set; }
        public int TournamentId { get; set; }
        public int Seed { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public ObservableCollection<TeamMember> Members { get; set; }
        public TournamentEntryView()
        {
            this.Members = new ObservableCollection<TeamMember>();
        }

        public override bool Equals(object obj)
        {
            return obj is TournamentEntryView view &&
                   TeamId == view.TeamId;
        }

        public override int GetHashCode()
        {
            return -1532736471 + TeamId.GetHashCode();
        }
    }
}
