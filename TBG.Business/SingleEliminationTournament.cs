using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class SingleEliminationTournament : ITournament
    {
        public int TournamentId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int UserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TournamentName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public decimal EntryFee { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double TotalPrizePool { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TournamentTypeId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ITeam> Participants { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ITournamentPrize> Prizes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
