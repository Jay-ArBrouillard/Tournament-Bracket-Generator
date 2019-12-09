using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TBG.Core.Interfaces;
using TBG.UI.Classes;

namespace TBG.UI.Models
{
    public class ParticipantTree : DependencyObject
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public double Seed { get; set; }
        public ObservableCollection<Person> Members { get; set; } = new ObservableCollection<Person>();
    }
}
