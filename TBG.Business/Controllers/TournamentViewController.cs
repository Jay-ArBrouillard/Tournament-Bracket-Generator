using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class TournamentViewerControl : ITournamentViewer
    {
        public bool addWinnerToNextRound(IRound round, IMatchupEntry winner)
        {
            return false;
        }
    }
}
