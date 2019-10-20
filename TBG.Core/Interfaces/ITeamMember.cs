using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITeamMember
    {
        int PersonTeamId { get; set; }
        int TeamId { get; set; }
        int PersonId { get; set; }
    }
}
