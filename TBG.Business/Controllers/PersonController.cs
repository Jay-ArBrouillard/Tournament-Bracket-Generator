using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class PersonController : IPersonController
    {
        public bool validatePerson(IPerson thisPerson, IPerson thatPerson)
        {
            if (thisPerson == null) { return false; }

            if (string.IsNullOrEmpty(thisPerson.FirstName)) { return false; }

            if (string.IsNullOrEmpty(thisPerson.LastName)) { return false; }

            if (string.IsNullOrEmpty(thisPerson.Email)) { return false; }

            if (string.IsNullOrEmpty(thisPerson.Phone)) { return false; }

            if (!isValidEmail(thisPerson.Email)) { return false; }

            if (!isValidPhoneNumber(thisPerson.Phone)) { return false; }

            if (thatPerson == null) { return true; }

            if (thatPerson.FirstName != null && thatPerson.LastName != null && thatPerson.Email != null)
            {
                bool firstNamesEqual = thisPerson.FirstName.Equals(thatPerson.FirstName) ? true : false;
                bool lastNamesEqual = thisPerson.LastName.Equals(thatPerson.LastName) ? true : false;
                bool emailsEqual = thisPerson.Email.Equals(thatPerson.Email) ? true : false;
                if (firstNamesEqual && lastNamesEqual && emailsEqual) { return false; }
            }

            return true;
        }

        public bool isValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool isValidPhoneNumber(string phone)
        {
            Match match = Regex.Match(phone, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
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
