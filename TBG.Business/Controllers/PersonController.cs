using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class PersonController : IPersonController
    {
        public bool validatePerson(IPerson person)
        {
            if (string.IsNullOrEmpty(person.FirstName)) { return false; }

            if (string.IsNullOrEmpty(person.LastName)) { return false; }

            if (string.IsNullOrEmpty(person.Email)) { return false; }

            if (string.IsNullOrEmpty(person.Phone)) { return false; }

            if (!isValidEmail(person.Email)) { return false; }

            if (!isValidPhoneNumber(person.Phone)) { return false; }

            return true;
        }

        public bool isValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool isValidPhoneNumber(string phone)
        {
            Match match = Regex.Match(phone, @"^\d{3}-\d{3}-\d{4}$");
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool validateWinLoss(string wins, string losses)
        {
            if (!Int32.TryParse(wins, out int x)) { return false; }

            if (!Int32.TryParse(losses, out int y)) { return false; }

            return true;
        }
    }
}
