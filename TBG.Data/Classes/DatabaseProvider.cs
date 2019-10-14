using TBG.Core.Interfaces;

namespace TBG.Data.Classes
{
    class DatabaseProvider : IProvider
    {
        public bool createTournament(ITournament entry)
        {
            //Create Tournament Record
            //Create Cross Reference Records For Tournament/Team/Player
            return true;
        }

        public bool createTeam(ITeam entry)
        {
            //Create Team Record
            return true;
        }

        public bool createPerson(IPerson entry)
        {
            //Create Person Record
            return true;
        }
    }
}
