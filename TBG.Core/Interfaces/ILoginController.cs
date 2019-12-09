namespace TBG.Core.Interfaces
{
    public interface ILoginController
    {
        IUser validateRegister(string username, string password, IUser thatUser);
        IUser validateLogin(string username, string password, IUser thatUser);
        IUser setLoginTime(IUser thisUser);
    }
}
