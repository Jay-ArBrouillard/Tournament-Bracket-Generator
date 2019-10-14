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
        bool createPerson(IPerson entry);
    }
}
