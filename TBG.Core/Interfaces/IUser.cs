using System;

namespace TBG.Core.Interfaces
{
    public interface IUser
    {
        int UserId { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool Active { get; set; }
        bool Admin { get; set; }
        DateTime LastLogin { get; set; }
    }
}
