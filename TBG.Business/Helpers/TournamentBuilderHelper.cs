using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Business.Models;
using TBG.Core.Interfaces;

namespace TBG.Business.Helpers
{
    public static class TournamentBuilderHelper
    {
        public static List<ITournamentEntry> GetSeededEntries(List<ITournamentEntry> entries)
        {
            entries = entries.OrderByDescending(x => x.Seed).ToList();
            bool useHiLoSeeding = entries.Any(x => x.Seed > 0);
            if (useHiLoSeeding)
            {
                List<ITournamentEntry> seededParticipants = new List<ITournamentEntry>();
                for (int i = 0; i < entries.Count / 2; i++)
                {
                    seededParticipants.Add(entries[i]);
                    seededParticipants.Add(entries[entries.Count - 1 - i]);
                }
                return seededParticipants;
            }
            else return entries;
        }

        public static Queue<ITournamentEntry> GetTeamQueue(List<ITournamentEntry> entries)
        {
            Queue<ITournamentEntry> teamQueue = new Queue<ITournamentEntry>();
            foreach (var team in entries)
            {
                teamQueue.Enqueue(team);
            }

            return teamQueue;
        }

        public static List<IRound> GetRounds(int numRounds)
        {
            var result = new List<IRound>();
            for (int i = 1; i <= numRounds; i++)
            {
                result.Add(new Round()
                {
                    RoundNum = i
                });
            }
            return result;
        }

        public static int GetRoundCounts(int fieldSize, int logBase)
        {
            return (int)Math.Log(fieldSize, logBase);
        }

        public static void AddTeamsToPairings(Queue<ITournamentEntry> teamQueue, ITournament tournament, IRound round)
        {
            BuildPairings(round, teamQueue.Count);

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
                NotificationHelper.NotifyParticipants(matchup, tournament);
            }
        }

        private static void BuildPairings(IRound round, int entryCount)
        {
            for (int i = 0; i < entryCount / 2; i++)
            {
                round.Matchups.Add(new Matchup()
                {
                    MatchupId = i,
                    MatchupEntries = new List<IMatchupEntry>()
                });
            }
        }
    }
}
