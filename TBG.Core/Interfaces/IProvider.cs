﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface IProvider
    {
        bool createTournament(ITournament entry);
        ITeam createTeam(ITeam entry);
        ITeam getTeam(string teamName);
        List<ITeam> getAllTeams();
        IPerson createPerson(IPerson entry);
        IPerson getPerson(int personId);
        IPerson getPersonByUniqueIdentifiers(string firstName, string lastName, string email);
        List<IPerson> getPeople();
        IUser createUser(IUser thisUser);
        IUser getUser(string userName);
        IUser updateLoginTime(IUser thisUser);
        List<ITournament> GetAllTournaments();
        List<ITournamentType> GetTournamentTypes();
        IPrize createPrize(IPrize prize);
        List<ITeamMember> getTeamMembersByTeamId(int teamId);
        List<IPrize> GetAllPrizes();
    }
}
