using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
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
        public string Ratio { get; set; }

        public Person(int PersonId, string FirstName, string LastName, string Email, string Phone, int Wins = 0, int Losses = 0)
        {
            this.PersonId = PersonId;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Wins = Wins;
            this.Losses = Losses;
            if (Wins == 0 || Losses == 0)
            {
                Ratio = "0";
            }
            else
            {
                Ratio = ((decimal)Wins / (decimal)Losses).ToString("0.##").Trim();
            }
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
                Ratio = "0";
            }
            else
            {
                Ratio = ((decimal)Wins / (decimal)Losses).ToString("0.##").Trim();
            }
        }
    }
}
