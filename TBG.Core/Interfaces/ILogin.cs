using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ILogin
    {
        /// <summary>
        /// Returns true if the username AND password matches in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        bool Validate(string user, string pass);

        /// <summary>
        /// Returns true if the username doesn't match a username in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool ValidateUserName(string user);

        /// <summary>
        /// Creates a new user in the database and returns true upon successful insert.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        bool CreateUser(string user, string pass);

        /// <summary>
        /// Updates last login for the passed user;
        /// </summary>
        /// <param name="user"></param>
        void updateLastLogin(string user);
    }
}
