using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class TournamentController : ITournamentController
    {
        public bool validateEntryFee(string entryFee)
        {
            if (!int.TryParse(entryFee, out _)) { return false; }

            return true;
        }
    }
}
