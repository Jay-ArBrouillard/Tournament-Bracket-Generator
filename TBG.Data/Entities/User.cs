using System;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class User : IUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool Admin { get; set; }
        public DateTime LastLogin { get; set; }
        public User()
        {

        }
    }
}
