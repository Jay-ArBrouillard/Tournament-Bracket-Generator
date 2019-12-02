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

            ActiveRound = 1;

            foreach (var round in Rounds)
            {
                AddTeamsToPairings(teamQueue, round);
            }
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

            ActiveRound++;
            var nextRound = Rounds.Where(x => x.RoundNum == ActiveRound).First();
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

                    NotifyParticipants(matchup);
                }
            }

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

                    NotifyParticipants(matchup);
                }
            }
        }

        private int CalculateRoundTotal(int FieldSize)
        {
            return (int)Math.Log(FieldSize, 2);
        }

        private void NotifyParticipants(IMatchup matchup)
        {
            List<IMatchupEntry> matchupEntries = matchup.MatchupEntries;
            string firstTeamName = Teams.Find(x => x.TeamId == matchupEntries[0].TheTeam.TeamId).TeamName;
            string secondTeamName = Teams.Find(x => x.TeamId == matchupEntries[1].TheTeam.TeamId).TeamName;
            Email(matchupEntries[0], firstTeamName, secondTeamName);
            Email(matchupEntries[1], firstTeamName, secondTeamName);
        }

        private void Email (IMatchupEntry matchupEntry, string competitor, string opponent)
        {
            foreach (var members in matchupEntry.TheTeam.Members)
            {
                string matchupString = "Your next matchup is against: " + opponent;
                int finalRound = Rounds.Max(x => x.RoundNum);
                if (ActiveRound == finalRound)
                {
                    matchupString = "Welcome to the finals against: " + opponent;
                }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                NotificationHelper.sendEmail(members.Email,
                    $"{members.FirstName} {members.LastName}",
                    $"{TournamentName} Tournament: Round {ActiveRound} ready to start",
                    $"Hello {competitor}!" +
                    $"\nRound {ActiveRound} is ready to start.\n" + 
                    matchupString + 
                    ".\nPlease report to the scorers table for location information.");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }
    }
}
