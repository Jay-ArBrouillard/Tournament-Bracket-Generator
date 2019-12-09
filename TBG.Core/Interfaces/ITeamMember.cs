namespace TBG.Core.Interfaces
{
    public interface ITeamMember
    {
        int PersonTeamId { get; set; }
        int TeamId { get; set; }
        int PersonId { get; set; }
    }
}
