using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kozol.Models
{
    public class ChannelViewModel
    {
        public int ID { get; set; }
        public int CreatorID { get; set; }
        public string Name { get; set; }
        public System.DateTime Created { get; set; }
        public int Capacity { get; set; }
        public bool Mode_Slow { get; set; }
        public bool Mode_Admin { get; set; }
        public bool Mode_Quiet { get; set; }
        public bool Mode_Invite { get; set; }
    }
}
