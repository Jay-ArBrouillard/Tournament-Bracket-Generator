using TBG.Business;
using TBG.Core.Interfaces;
using TBG.Data.Classes;

namespace TBG.Driver
{
    public static class ApplicationController
    {
        public static IProvider GetProvider()
        {
            return new DatabaseProvider();
        }

        public static IController GetController()
        {
            return new BusinessController();
        }

        public static ILoginController GetLoginController()
        {
            return new LoginController();
        }

        public static IPersonController getPersonController()
        {
            return new PersonController();
        }

        public static ITeamController getTeamController()
        {
            return new TeamController();
        }
    }
}
