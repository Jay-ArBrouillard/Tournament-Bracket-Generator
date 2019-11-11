using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class BusinessController : IController
    {
        public void TestTournament()
        {
            var tournament = new SingleEliminationTournament()
            {
                TournamentId = 1,
                TournamentName = "Test Tournament",
                EntryFee = 50,
                TotalPrizePool = 2000,
                TournamentTypeId = 1,
                UserId = 1
            };

            for (int i = 1; i <= 8; i++)
            {
                var te = new TournamentEntry()
                {
                    TournamentEntryId = i,
                    TournamentId = 1,
                    TeamId = i,
                    Seed = i
                };

                tournament.Participants.Add(te);
            }

            tournament.BuildTournament();
            var matchup = tournament.Rounds.Find(x => x.RoundNum == 1).Pairings.Find(x => x.MatchupId == 1);
            matchup.Teams.Find(x => x.TheTeam.TeamId == 3).Score = 10;
            matchup.Teams.Find(x => x.TheTeam.TeamId == 4).Score = 11;
            tournament.RecordResult(matchup);

            var stop = "Stop";
        }

        public ITournament AnotherTestTournament()
        {
            var tournament = new SingleEliminationTournament()
            {
                TournamentId = 1,
                TournamentName = "Another Test Tournament",
                EntryFee = 50,
                TotalPrizePool = 2000,
                TournamentTypeId = 1,
                UserId = 1
            };

            int j = 1;
            for (int i = 58; i <= 65; i++)
            {
                var te = new TournamentEntry()
                {
                    TournamentEntryId = i,
                    TournamentId = 1,
                    TeamId = i,
                    Seed = j
                };
                j++;

                tournament.Participants.Add(te);
            }

            tournament.BuildTournament();
            return tournament;
        }
    }
}
