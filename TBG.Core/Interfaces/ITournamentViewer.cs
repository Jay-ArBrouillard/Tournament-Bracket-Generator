namespace TBG.Core.Interfaces
{
    public interface ITournamentViewer
    {
        bool addWinnerToNextRound(IRound round, IMatchupEntry winner);
    }
}
