using System.Collections.Generic;

namespace TBG.Core.Interfaces
{
    public interface IProvider
    {
        bool sourceActive { get; set; }
        ITournament createTournament(ITournament entry);
        ITournament deleteTournament(int id);
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
        List<ITournament> getAllTournaments();
        List<ITournamentType> getTournamentTypes();
        IPrize createPrize(IPrize prize);
        List<ITeamMember> getTeamMembersByTeamId(int teamId);
        List<IPrize> getAllPrizes();
        IMatchup saveMatchupScore(IMatchup matchup);
        ITournament saveActiveRound(ITournament tournament);
        IUser updateUser(IUser entity);
        ITournament getTournament(int id);
        List<ITeam> getTeamsFromTournamentId(int tournamentId);
        List<ITournamentEntry> saveTournamentEntry(IMatchup matchup);
        IMatchup savePersonStats(IMatchup matchup);
        IMatchup saveTeamScore(IMatchup matchup);
    }
}
