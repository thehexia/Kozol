using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kozol.Models.ViewModels
{
    public class UsersViewModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> LastActivity { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Username { get; set; }
        public Nullable<System.Guid> Avatar { get; set; }
        public bool Avatar_Custom { get; set; }
        public string Public_Key_n { get; set; }
    }

    public class MinUsersViewModel
    {
        public string Username { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public Nullable<System.Guid> Avatar { get; set; }
        public bool Avatar_Custom { get; set; }
        public string Public_Key_n { get; set; }
    }
}