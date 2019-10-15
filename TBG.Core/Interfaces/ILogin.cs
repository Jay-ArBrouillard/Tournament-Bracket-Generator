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
        /// Returns true the password matches the user in the databas.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        bool Validate(string user, string pass);
    }
}
