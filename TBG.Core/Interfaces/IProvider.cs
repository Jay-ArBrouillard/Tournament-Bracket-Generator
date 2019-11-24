using System.Collections.Generic;

namespace TBG.Core.Interfaces
{
    public interface IProvider
    {
        ITournament createTournament(ITournament entry);
        ITournament deleteTournament(ITournament entry);
        ITournament getTournamentByName(string entry);
        List<ITournamentEntry> createTournamentEntries(List<ITournamentEntry> entry);
        IRound createRound(IRound entry);
        ITeam createTeam(ITeam entry);
        ITeam getTeam(string teamName);
        ITeam getTeam(int teamId);
        List<ITeam> getAllTeams();
        IPerson createPerson(IPerson entry);
        IPerson getPerson(int personId);
        IPerson getPersonByUniqueIdentifiers(string firstName, string lastName, string email);
        List<IPerson> getPeople();
        IUser createUser(IUser thisUser);
        IUser getUser(string userName);
        IUser updateLoginTime(IUser thisUser);
        List<ITournament> getAllTournaments();
        List<ITournamentType> getTournamentTypes();
        IPrize createPrize(IPrize prize);
        List<ITeamMember> getTeamMembersByTeamId(int teamId);
        List<IPrize> getAllPrizes();
        List<ITournamentEntry> getTournamentEntriesByTournamentId(int tournamentId);
        IMatchup createMatchup(IMatchup entry);
        IMatchupEntry createMatchupEntry(IMatchupEntry matchupEntry);
        IRoundMatchup createRoundMatchup(IRoundMatchup roundMatchup);
        IMatchup getMatchup(int matchupId);
        IRound getRoundByTournamentIdandRoundNum(IRound round);
        List<IRoundMatchup> getRoundMatchupsByRoundId(IRoundMatchup currRound);
        List<IMatchupEntry> getMatchupEntriesByMatchupId(int matchupId);
        IMatchupEntry updateMatchupEntryScore(int matchupId, int tournamentEntryId, int score);
        void setupTournamentData(ITournament newTournament);
        ITournamentEntry getTournamentEntry(int tournamentEntryId);
        List<IRound> getRoundsByTournamentId(int tournamentId);
        IRoundMatchup getRoundMatchupByRoundIdAndMatchupNumber(IRoundMatchup roundMatchup);
        ITournament updateTournamentName(ITournament entry);
        List<IMatchup> getAllMatchups();
        IMatchup deleteMatchup(IMatchup matchup);
    }
}
