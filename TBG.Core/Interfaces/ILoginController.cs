namespace TBG.Core.Interfaces
{
    public interface ILoginController
    {
        bool validateRegister(IUser thisUser, IUser thatUser);
        bool validateLogin(IUser thisUser, IUser thatUser);
    }
}
