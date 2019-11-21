using TBG.Core.Interfaces;

namespace TBG.Business.Models
{
    public class Person : IPerson
    {
        public int PersonId { get; set; }
        /// <summary>
        /// Represents this Person's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents this Person's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents this Person's email address in the form example@email.com.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Represents this Person's phone number in the form (###-###-####).
        /// </summary>
        public string Phone { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
