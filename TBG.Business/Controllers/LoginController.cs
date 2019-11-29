using System;
using TBG.Business.Models;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class LoginController : ILoginController
    {
        public IUser setLoginTime(IUser thisUser)
        {
            thisUser.LastLogin = DateTime.Now;
            return thisUser;
        }

        public IUser validateLogin(string username, string password, IUser thatUser)
        {
            if (thatUser == null) { return null; }

            //Check UserName
            if (!username.Equals(thatUser.UserName)) { return null; }

            //Check Password
            if (!password.Equals(thatUser.Password)) { return null; }

            var thisUser = thatUser;
            thisUser.LastLogin = DateTime.Now;

            return thisUser;
        }

        public IUser validateRegister(string username, string password, IUser thatUser)
        {
            if (thatUser != null) { return null; }
            if (string.IsNullOrEmpty(username)) { return null ; }
            if (string.IsNullOrEmpty(password)) { return null; }

            return new User()
            {
                UserName = username,
                Password = password,
                Active = true,
                Admin = false,
                LastLogin = DateTime.Now
            };
        }
    }
}
