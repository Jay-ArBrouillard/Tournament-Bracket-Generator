using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG.UI.Models
{
    public class TournamentListBoxItem
    {
        public TournamentListBoxItem (string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
