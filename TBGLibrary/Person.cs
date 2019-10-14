﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGLibrary
{
    public class Person
    {
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
        public string Number { get; set; }
    }
}
