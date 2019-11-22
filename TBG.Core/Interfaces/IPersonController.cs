namespace TBG.Core.Interfaces
{
    public interface IPersonController
    {
        bool validatePerson(IPerson thisPerson, IPerson thatPerson);
        bool validateWinLoss(string wins, string losses);
    }
}
