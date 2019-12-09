using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class Team : ITeam
    {
        public int TeamId { get; set; }
        /// <summary>
        /// Represents this team's team name.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Represents all the team members in this team.
        /// </summary>
        public List<IPerson> TeamMembers { get; set; } = new List<IPerson>();

        /// <summary>
        /// Represents how many matchups this team has won.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Represents how many matchups this team has lost.
        /// </summary>
        public int Losses { get; set; }
    }
}
