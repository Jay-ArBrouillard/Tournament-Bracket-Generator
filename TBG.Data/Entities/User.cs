using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Data.Entities
{
    public class User : IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool Admin { get; set; }
        public DateTime LastLogin { get; set; }

        public User(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }

        public User(string UserName, string Password, bool Active, bool Admin)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.Active = Active;
            this.Admin = Admin;
        }

        public User(string UserName, string Password, bool Active, bool Admin, DateTime LastLogin)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.Active = Active;
            this.Admin = Admin;
            this.LastLogin = LastLogin;
        }
    }
}
