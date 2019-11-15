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
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public string TournamentName { get; set; }
        public double EntryFee { get; set; }
        public double TotalPrizePool { get; set; }
        public int TournamentTypeId { get; set; }
        public List<ITournamentEntry> Participants { get; set; } 
        public List<IPrize> Prizes { get; set; }
        public List<IRound> Rounds { get; set; } 
        
        public SingleEliminationTournament()
        {

        }

        public ITournament BuildTournament()
        {
            Queue<ITournamentEntry> teamQueue = new Queue<ITournamentEntry>();
            foreach (var team in Participants)
            {
                teamQueue.Enqueue(team);
            }

            for (int i = 1; i <= CalculateRoundTotal(Participants.Count); i++)
            {
                Rounds.Add(new Round()
                {
                    RoundNum = i,
                    TournamentId = TournamentId
                });
            }

            foreach (var round in Rounds)
            {
                Matchup nextRoundMatchup = new Matchup();

                if (round.RoundNum == 1)
                {
                    for (int i = 0; i < Participants.Count / 2; i++)
                    {
                        round.Pairings.Add(new Matchup()
                        {
                            MatchupId = i,
                            Teams = new List<IMatchupEntry>()
                        });
                    }

                    foreach (var matchup in round.Pairings)
                    {
                        matchup.Teams.Add(new MatchupEntry()
                        {
                            TheTeam = teamQueue.Dequeue(),
                            Score = 0
                        });

                        matchup.Teams.Add(new MatchupEntry()
                        {
                            TheTeam = teamQueue.Dequeue(),
                            Score = 0
                        });
                    }
                }

                if (round.Pairings.Count > 1)
                {
                    int count = 1;
                    foreach (var matchup in round.Pairings)
                    {
                        if (IsOdd(count))
                        {
                            nextRoundMatchup = new Matchup();
                            var nextRound = Rounds.Find(x => x.RoundNum == round.RoundNum + 1);
                            if (nextRound != null) { nextRound.Pairings.Add(nextRoundMatchup); } //Safety, but there should be a next round if Pairings.count > 1.
                        }
                        matchup.NextRound = nextRoundMatchup;

                        count++;
                    }
                }
            }

            return this;
        }

        public bool RecordResult(IMatchup matchup)
        {
            var winner = CreateWinnerMatchupEntry(matchup.Teams.OrderByDescending(x => x.Score).First());
            matchup.NextRound.Teams.Add(winner);
            return true;
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
