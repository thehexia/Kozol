using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kozol.Models.ViewModels
{
    public class InviteViewModel
    {
        public int SenderId { get; set; }
        public int ReceieverId { get; set; }
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string SenderPublicKey { get; set; }
        public string SharedKey { get; set; }
        
    }
}