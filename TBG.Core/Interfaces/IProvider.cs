using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface IProvider
    {
        bool createTournament(ITournament entry);
        bool createTeam(ITeam entry);
        ITeam getTeam(string teamName);
        bool createPerson(IPerson entry);
        bool createUser(IUser thisUser);
        IUser getUser(string userName);
        bool updateLoginTime(IUser thisUser);
        List<IPerson> getPeople();
        List<ITournament> GetAllTournaments();
        List<ITournamentType> GetTournamentTypes();
    }
}
