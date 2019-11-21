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
