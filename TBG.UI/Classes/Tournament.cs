using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Classes
{
    public class Tournament : ITournament
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public double EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public List<ITeam> Teams { get; set; }
        public List<ITournamentEntry> Participants { get; set; }
        public List<IPrize> Prizes { get; set; }
        public List<IRound> Rounds { get; set; }

        public Tournament()
        {
            Teams = new List<ITeam>();
            Participants = new List<ITournamentEntry>();
            Prizes = new List<IPrize>();
            Rounds = new List<IRound>();
        }

        public Tournament(int tournamentId)
        {
            TournamentId = tournamentId;
        }

        public ITournament BuildTournament()
        {
            throw new NotImplementedException();
        }

        public bool RecordResult(IMatchup matchup)
        {
            throw new NotImplementedException();
        }

        public ITournament RebuildTournament()
        {
            throw new NotImplementedException();
        }
    }
}
