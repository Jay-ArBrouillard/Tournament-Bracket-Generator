using System;
using System.Collections.Generic;
using System.Linq;
using TBG.Business.Helpers;
using TBG.Business.Interfaces;
using TBG.Business.Models;
using TBG.Core.Interfaces;

namespace TBG.Business.Tournaments
{
    public class SingleEliminationTournament : ITournamentApplication
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public double EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public List<ITeam> Teams { get; set; } = new List<ITeam>();
        public List<ITournamentEntry> TournamentEntries { get; set; } = new List<ITournamentEntry>();
        public List<IPrize> Prizes { get; set; } = new List<IPrize>();
        public List<IRound> Rounds { get; set; } = new List<IRound>();
        public int ActiveRound { get; set; }
        public List<IPrize> TournamentPrizes { get; set; }

        public ITournament BuildTournament()
        {
            TournamentEntries = TournamentBuilderHelper.GetSeededEntries(TournamentEntries);

            var teamQueue = TournamentBuilderHelper.GetTeamQueue(TournamentEntries);

            Rounds = TournamentBuilderHelper.GetRounds(CalculateRoundTotal(TournamentEntries.Count));

            foreach (var round in Rounds)
            {
                AddTeamsToPairings(teamQueue, round);
            }

            ActiveRound = 1;
            return this;
        }

        public ITournament RebuildTournament()
        {
            return this;
        }

        public ITournament AdvanceRound()
        {
            Queue<ITournamentEntry> teamQueue = new Queue<ITournamentEntry>();
            var activeRound = Rounds.Where(x => x.RoundNum == ActiveRound).First();
            foreach (var pairing in activeRound.Matchups)
            {
                var winner = pairing.MatchupEntries.OrderByDescending(x => x.Score).First().TheTeam;
                teamQueue.Enqueue(winner);
            }

            var nextRound = Rounds.Where(x => x.RoundNum == ActiveRound + 1).First();
            if (nextRound != null)
            {
                for (int i = 0; i < teamQueue.Count / 2; i++)
                {
                    nextRound.Matchups.Add(new Matchup()
                    {
                        MatchupId = i,
                        MatchupEntries = new List<IMatchupEntry>()
                    });
                }

                foreach (var matchup in nextRound.Matchups)
                {
                    matchup.MatchupEntries.Add(new MatchupEntry()
                    {
                        TheTeam = teamQueue.Dequeue(),
                        Score = 0
                    });

                    matchup.MatchupEntries.Add(new MatchupEntry()
                    {
                        TheTeam = teamQueue.Dequeue(),
                        Score = 0
                    });
                }
            }
            ActiveRound++;
            return this;
        }

        private void BuildPairings(IRound round)
        {
            for (int i = 0; i < TournamentEntries.Count / 2; i++)
            {
                round.Matchups.Add(new Matchup()
                {
                    MatchupId = i,
                    MatchupEntries = new List<IMatchupEntry>()
                });
            }
        }

        private void AddTeamsToPairings(Queue<ITournamentEntry> teamQueue, IRound round)
        {
            if (round.RoundNum == 1)
            {
                BuildPairings(round);

                foreach (var matchup in round.Matchups)
                {
                    matchup.MatchupEntries.Add(new MatchupEntry()
                    {
                        TheTeam = teamQueue.Dequeue(),
                        Score = 0
                    });

                    matchup.MatchupEntries.Add(new MatchupEntry()
                    {
                        TheTeam = teamQueue.Dequeue(),
                        Score = 0
                    });
                }
            }
        }

        private IMatchupEntry CreateWinnerMatchupEntry(IMatchupEntry winner)
        {
            var nextRound = new MatchupEntry()
            {
                TheTeam = winner.TheTeam,
                Score = 0
            };

            return nextRound;
        }

        /// <summary>
        /// Assuming there are 2 teams per matchup
        /// </summary>
        /// <param name="FieldSize"></param>
        /// <returns></returns>
        private int CalculateRoundTotal(int FieldSize)
        {
            return (int) Math.Log(FieldSize, 2);
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

    }
}
