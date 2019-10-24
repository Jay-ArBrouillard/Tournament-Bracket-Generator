using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Models
{
    public class Team : ITeam
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public List<IPerson> TeamMembers { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public ObservableCollection<TeamMember> Members { get; set; } //For TreeView

        public Team(string teamName, List<IPerson> teamMembers, int wins = 0, int losses = 0)
        {
            TeamName = teamName;
            TeamMembers = teamMembers;
            Wins = wins;
            Losses = losses;
            if (teamMembers != null)
            {
                //Populate Members for the Treeview
                foreach (IPerson person in teamMembers)
                {
                    Members.Add(new TeamMember(person.FirstName, 0));
                }
            }
        }
    }

    public class TeamMember
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public TeamMember(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

}
