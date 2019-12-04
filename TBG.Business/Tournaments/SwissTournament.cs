using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Business.Helpers;
using TBG.Business.Interfaces;
using TBG.Business.Models;
using TBG.Core.Interfaces;

namespace TBG.Business.Tournaments
{
    class SwissTournament : ITournamentApplication
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public double EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public int ActiveRound { get; set; }
        public List<ITeam> Teams { get; set; } = new List<ITeam>();
        public List<ITournamentEntry> TournamentEntries { get; set; } = new List<ITournamentEntry>();
        public List<IPrize> TournamentPrizes { get; set; } = new List<IPrize>();
        public List<IPrize> Prizes { get; set; } = new List<IPrize>();
        public List<IRound> Rounds { get; set; } = new List<IRound>();

        public ITournament AdvanceRound()
        {
            var activeRound = Rounds.Where(x => x.RoundNum == ActiveRound).First();

            ReseedTournament();
            TournamentEntries = TournamentEntries.OrderByDescending(x => x.Seed).ToList();
            var teamQueue = TournamentBuilderHelper.GetTeamQueue(TournamentEntries);

            ActiveRound++;

            var nextRound = Rounds.Where(x => x.RoundNum == ActiveRound).First();
            if (nextRound != null)
            {
                TournamentBuilderHelper.AddTeamsToPairings(teamQueue, this, nextRound);
            }

            return this;
        }

        public ITournament BuildTournament()
        {
            TournamentEntries = TournamentBuilderHelper.GetSeededEntries(TournamentEntries);
            var teamQueue = TournamentBuilderHelper.GetTeamQueue(TournamentEntries);
            var roundCount = TournamentBuilderHelper.GetRoundCounts(TournamentEntries.Count, 2);
            Rounds = TournamentBuilderHelper.GetRounds(roundCount);

            ActiveRound = 1;

            foreach (var round in Rounds)
            {
                TournamentBuilderHelper.AddTeamsToPairings(teamQueue, this, round);
            }
            
            return this;
        }

        public ITournament RebuildTournament()
        {
            return this;
        }

        private void ReseedTournament()
        {
            var matchups = Rounds.SelectMany(x => x.Matchups).ToList();
            foreach (var entry in TournamentEntries)
            {
                var wins = 0;
                var losses = 0;
                var entryMatchupEntries = matchups.SelectMany(y => y.MatchupEntries).Where(z => z.TheTeam.TeamId == entry.TeamId);
                foreach(var matchupEntry in entryMatchupEntries)
                {
                    var matchup = matchups.Where(x => x.MatchupId == matchupEntry.MatchupId).First();
                    var entryScore = matchup.MatchupEntries.Where(x => x.TheTeam.TeamId == entry.TeamId).First().Score;
                    var opponentScore = matchup.MatchupEntries.Where(x => x.TheTeam.TeamId != entry.TeamId).First().Score;
                    if (entryScore > opponentScore) { wins++; }
                    else { losses++; }
                }
                if (losses == 0) { entry.Seed = 1; }
                else { entry.Seed = wins / (wins + losses); }
            }
        }
    }
}
