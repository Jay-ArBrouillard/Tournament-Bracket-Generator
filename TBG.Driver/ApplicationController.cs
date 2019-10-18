using TBG.Business;
using TBG.Core.Interfaces;
using TBG.Data.Classes;

namespace TBG.Driver
{
    public static class ApplicationController
    {
        public static IDatabaseProvider getDatabaseProvider()
        {
            return new DatabaseProvider();
        }

        public static IController getController()
        {
            return new BusinessController();
        }

        public static ILoginController getLoginController()
        {
            return new LoginController();
        }

    }
}
