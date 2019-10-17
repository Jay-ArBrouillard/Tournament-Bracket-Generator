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

        public static IDatabaseLogin GetDatabaseLogin()
        {
            return new DatabaseLogin();
        }

    }
}
