using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kozol.Models
{
    public class Invite
    {
        [Key]
        public int InviteId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual UserProfile Sender { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual UserProfile Receiver { get; set; }

        public int ChannelId { get; set; }

        public virtual Channel Channel { get; set; }

        //optional. if its null then its just not an ecnrypted chat
        public string SharedKey { get; set; }
    }
}