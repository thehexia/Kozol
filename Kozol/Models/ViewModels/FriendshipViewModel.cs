using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kozol.Models.ViewModels
{
    public class FriendshipViewModel
    {
        public int FriendId { get; set; }
        public string FriendUsername { get; set; }
        public string FriendEmail { get; set; }
        public string PublicKey { get; set; }
    }
}
