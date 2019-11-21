using System;

namespace TBG.Core.Interfaces
{
    public interface IPrizeController
    {
        IPrize ValidatePrize(String inName, String inPerc);
    }
}
