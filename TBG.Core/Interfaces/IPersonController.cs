namespace TBG.Core.Interfaces
{
    public interface IPersonController
    {
        bool validatePerson(IPerson person);
        bool validateWinLoss(string wins, string losses);
    }
}
