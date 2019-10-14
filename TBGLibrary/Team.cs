using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public class Team
    {
        /// <summary>
        /// Represents this team's team name.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Represents all the team members in this team.
        /// </summary>
        public List<Person> TeamMembers { get; set; } = new List<Person>();

        /// <summary>
        /// Represents the skill level of this team.
        /// </summary>
        public int Seed { get; set; }

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
