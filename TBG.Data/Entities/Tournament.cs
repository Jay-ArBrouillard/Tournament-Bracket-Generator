using System;
using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class Tournament : ITournament
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public double EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public List<ITeam> Teams { get; set; } = new List<ITeam>();
        public List<ITournamentPrize> Prizes { get; set; } = new List<ITournamentPrize>();
        public List<IRound> Rounds { get; set; } = new List<IRound>();
        public int ActiveRound { get; set; }
        public List<ITournamentEntry> TournamentEntries { get; set; } = new List<ITournamentEntry>();
        public List<ITournamentPrize> TournamentPrizes { get; set; } = new List<ITournamentPrize>();
    }
}
