using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.UI.Classes
{
    public class Person : IPerson
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double Ratio { get; set; }

        public Person()
        {

        }

        public Person(string FirstName, string LastName, string Email, string Phone, int Wins = 0, int Losses = 0)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Wins = Wins;
            this.Losses = Losses;
            if (Wins == 0 || Losses == 0)
            {
                Ratio = 0.0;
            }
            else
            {
                if (Wins + Losses == 0)
                {
                    Ratio = 0;
                }
                else
                {
                    Ratio = (double)Wins / (Wins + Losses);
                }
            }
        }

    }
}
