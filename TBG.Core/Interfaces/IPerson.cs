﻿namespace TBG.Core.Interfaces
{
    public interface IPerson
    {
        int PersonId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        int Wins { get; set; }
        int Losses { get; set; }

        double Ratio { get; set; }
    }
}
