using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface IPersonController
    {
        bool validatePerson(IPerson thisPerson, IPerson thatPerson);
        bool validateWinLoss(string wins, string losses);
    }
}
