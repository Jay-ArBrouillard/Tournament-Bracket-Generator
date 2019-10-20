using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class TeamMember : ITeamMember
    {
        public int PersonTeamId { get; set; }
        public int TeamId { get; set; }
        public int PersonId { get; set; }
    }
}
