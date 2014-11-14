using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kozol.Models
{
    public class Friends
    {
        [Key]
        [Column(Order = 1)]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual UserProfile Sender { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual UserProfile Receiver { get; set; }

        [Required]
        public bool Accepted { get; set; }
    }
}