using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            List<ITeam> teams,
            List<ITournamentPrize> prizesInTournament
        )
        {
            var tournament = TournamentTypeHelper.GetNewTournament(tournamentType);
            tournament.TournamentName = tournamentName;
            tournament.EntryFee = entryFee;
            tournament.TotalPrizePool = totalPrizePool;
            tournament.UserId = userId;
            tournament.TournamentEntries = participants;
            tournament.Teams = teams;
            tournament.TournamentPrizes = prizesInTournament;

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

        public List<ITournamentPrize> validatePrizes(List<IPrize> prizes)
        {
            var result = new List<ITournamentPrize>();
            foreach(var prize in prizes)
            {
                result.Add(new TournamentPrize()
                {
                    PrizeId = prize.PrizeId,
                    PlaceId = prize.PlaceNumber
                });
            }

            return result;
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

        public List<IResultDataRow> populateResultsGrid(ITournament tournament, List<IMatchup> matchups, List<IPrize> prizes)
        {
            List<IResultDataRow> dataRows = new List<IResultDataRow>();

            foreach (var matchup in matchups)
            {
                for (int i = 0; i < matchup.MatchupEntries.Count; i++) //var matchupEntry in matchup.MatchupEntries)
                {
                    IResultDataRow row = new ResultDataRow();
                    StringBuilder playerBuilder = new StringBuilder();
                    int numTeamMembers = matchup.MatchupEntries[i].TheTeam.Members.Count;
                    for (int j = 0; j < numTeamMembers; j++) //var player in matchup.TheTeam.Members)
                    {
                        IPerson player = matchup.MatchupEntries[i].TheTeam.Members[j];
                        playerBuilder.Append(player.FirstName).Append(" ").Append(player.LastName);
                        if (j != numTeamMembers - 1)
                        {
                            playerBuilder.Append(" + ");
                        }
                    }
                    ITeam thisTeam = tournament.Teams.Find(x => x.TeamId == matchup.MatchupEntries[i].TheTeam.TeamId);
                    row.Players = playerBuilder.ToString();
                    row.TeamName = thisTeam.TeamName;
                    var te = tournament.TournamentEntries.Find(x => x.TeamId == thisTeam.TeamId);
                    row.Placing = tournament.TournamentEntries.IndexOf(te) + 1;
                    row.Wins = matchup.MatchupEntries[i].TheTeam.Wins;
                    row.Losses = matchup.MatchupEntries[i].TheTeam.Losses;
                    row.WinLoss = Math.Round(calculateWinPercentage(row.Wins, row.Losses), 3);
                    row.CareerWins = thisTeam.Wins + matchup.MatchupEntries[i].TheTeam.Wins;
                    row.CareerLosses = thisTeam.Losses + matchup.MatchupEntries[i].TheTeam.Losses;
                    row.CareerWinLoss = Math.Round(calculateWinPercentage(row.CareerWins, row.CareerLosses), 3);
                    var prize = tournament.TournamentPrizes.Find(y => y.PlaceId == row.Placing);
                    row.Winnings = prize != null ? prizes.Find(x => x.PrizeId == prize.PrizeId).PrizeAmount : 0;

                    dataRows.Add(row);
                }
            }

            return dataRows;
        }

        private double calculateWinPercentage(int wins, int losses)
        {
            if (wins + losses == 0)
            {
                return 0;
            }

            return (double)wins / (wins + losses);
        }

        public ITournament reSeedTournament(ITournament tournament)
        {
            var matchups = tournament.Rounds.SelectMany(x => x.Matchups).ToList();
            foreach (var entry in tournament.TournamentEntries)
            {
                var wins = 0;
                var losses = 0;
                var entryMatchupEntries = matchups.SelectMany(y => y.MatchupEntries).Where(z => z.TheTeam.TeamId == entry.TeamId);
                foreach (var matchupEntry in entryMatchupEntries)
                {
                    var matchup = matchups.Where(x => x.MatchupId == matchupEntry.MatchupId).First();
                    var entryScore = matchup.MatchupEntries.Where(x => x.TheTeam.TeamId == entry.TeamId).First().Score;
                    var opponentScore = matchup.MatchupEntries.Where(x => x.TheTeam.TeamId != entry.TeamId).First().Score;
                    if (entryScore > opponentScore) { wins++; }
                    else { losses++; }
                }
                if (losses == 0) { entry.Seed = 1; }
                else { entry.Seed = calculateWinPercentage(wins, losses); }

            }

            return tournament;
        }
    }
}
