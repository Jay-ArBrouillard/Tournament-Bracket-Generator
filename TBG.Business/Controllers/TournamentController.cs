using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TBG.Business.Helpers;
using TBG.Business.Models;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class TournamentController : ITournamentController
    {
        public ITournament createTournament(
            string tournamentName, 
            ITournamentType tournamentType, 
            int userId, 
            double entryFee, 
            double totalPrizePool,
            List<ITournamentEntry> participants,
            List<ITeam> teams
        )
        {
            var tournament = TournamentTypeHelper.GetNewTournament(tournamentType);
            tournament.TournamentName = tournamentName;
            tournament.EntryFee = entryFee;
            tournament.TotalPrizePool = totalPrizePool;
            tournament.UserId = userId;
            tournament.TournamentEntries = participants;
            tournament.Teams = teams;

            return tournament.BuildTournament();
        }

        public bool validateTournamentName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return false; }
            return true;
        }

        public double validateEntryFee(string number)
        {
            double result;
            if (string.IsNullOrEmpty(number) || !double.TryParse(number, out result)) { return -1; }
            return result;
        }

        public bool validateTournamentType(ITournamentType type)
        {
            if (type == null) { return false; }
            return true;
        }

        public bool validateParticipantCount(int count, ITournamentType tournamentType)
        {
            if (count <= 1) { return false; }
            if (!isEven(count)) { return false; }
            if (tournamentType.TournamentTypeId == 1 && !isPowerOfTwo(count)) { return false; }
            return true;
        }

        public double validateTotalPrizePool(string pool, int numParticipants, double entryFee)
        {
            double poolDouble;
            if (!double.TryParse(pool, out poolDouble))
            {
                return -1;
            }

            if (Math.Abs(poolDouble - (numParticipants * entryFee)) > 0.01)
            {
                return -1;
            }
            return poolDouble;
        }

        public ITournament rebuildTournament(ITournament savedTournament)
        {
            var tournament = TournamentTypeHelper.ConvertTournamentType(savedTournament);
            tournament.RebuildTournament();
            return tournament;
        }

        public int validateScore(string score)
        {
            int result;

            if (!int.TryParse(score, out result))
            {
                return -1;
            }

            return result;
        }

        public bool ScoreMatchup(IMatchup matchup, int team1Score, int team2Score)
        {
            matchup.MatchupEntries[0].Score = team1Score;
            matchup.MatchupEntries[1].Score = team2Score;
            matchup.Completed = true;
            //Keep track of wins and losses only in this tournament
            if (matchup.MatchupEntries[0].Score > matchup.MatchupEntries[1].Score)
            {
                matchup.MatchupEntries[0].TheTeam.Wins++;
                matchup.MatchupEntries[1].TheTeam.Losses++;
            }
            else
            {
                matchup.MatchupEntries[0].TheTeam.Losses++;
                matchup.MatchupEntries[1].TheTeam.Wins++;
            }
            return true;
        }

        public bool validateRoundCompletion(IRound round)
        {
            return !round.Matchups.Where(x => x.Completed == false).Any();
        }

        public ITournament advanceRound(ITournament tournament)
        {
            var convertedTournament = TournamentTypeHelper.ConvertTournamentType(tournament);
            var lastRound = convertedTournament.Rounds.Max(x => x.RoundNum);
            convertedTournament.AdvanceRound();
            return convertedTournament;
        }

        public bool validateActiveRound (ITournament tournament)
        {
            var lastround = tournament.Rounds.Max(x => x.RoundNum);
            if (tournament.ActiveRound == lastround) { return false; }
            return true;
        }

        private bool isPowerOfTwo(int n)
        {

            if (n == 0)
                return false;

            return (int)(Math.Ceiling((Math.Log(n) /
                                       Math.Log(2)))) ==
                   (int)(Math.Floor(((Math.Log(n) /
                                      Math.Log(2)))));
        }

        private bool isEven(int n)
        {
            if (n % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
