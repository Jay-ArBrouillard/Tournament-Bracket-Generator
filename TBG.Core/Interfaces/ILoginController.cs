using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ILoginController
    {
        bool validateRegister(IUser thisUser, IUser thatUser);
        bool validateLogin(IUser thisUser, IUser thatUser);
    }
}
