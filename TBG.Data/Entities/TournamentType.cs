using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class TournamentType :ITournamentType
    {
        public int TournamentTypeId { get; set; }
        public string TournamentTypeName { get; set; }
    }
}
