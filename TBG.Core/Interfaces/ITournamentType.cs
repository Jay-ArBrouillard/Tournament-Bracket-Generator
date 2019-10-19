using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Core.Interfaces
{
    public interface ITournamentType
    {
        int TournamentTypeId { get; set; }
        string TournamentTypeName { get; set; }
    }
}
