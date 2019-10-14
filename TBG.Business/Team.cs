using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business
{
    public class Team : ITeam
    {
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
        public int Wins { get; }

        /// <summary>
        /// Represents how many matchups this team has lost.
        /// </summary>
        public int Losses { get; }
    }
}
