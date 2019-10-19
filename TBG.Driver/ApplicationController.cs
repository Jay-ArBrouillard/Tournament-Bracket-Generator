﻿using TBG.Business;
using TBG.Core.Interfaces;
using TBG.Data.Classes;

namespace TBG.Driver
{
    public static class ApplicationController
    {
        public static IProvider getDatabaseProvider()
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
