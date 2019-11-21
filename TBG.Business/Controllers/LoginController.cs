using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class LoginController : ILoginController
    {
        public bool validateLogin(IUser thisUser, IUser thatUser)
        {
            if (thisUser == null || thatUser == null) { return false; }
            //Check UserName
            if (!thisUser.UserName.Equals(thatUser.UserName)) { return false; }

            //Check Password
            if (!thisUser.Password.Equals(thatUser.Password)) { return false; }

            thisUser.UserId = thatUser.UserId;  //They are the same users

            return true;
        }

        public bool validateRegister(IUser thisUser, IUser thatUser)
        {
            if (thisUser == null || thatUser != null) { return false; }

            if (string.IsNullOrEmpty(thisUser.UserName)) { return false; }

            if (string.IsNullOrEmpty(thisUser.Password)) { return false; }

            return true;
        }
    }
}
