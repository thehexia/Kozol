using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kozol.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual UserProfile Sender { get; set; }

        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual Channel Receiver { get; set; }
    }

    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public int MessageId { get; set; }

        [Required]
        public string Path { get; set; }

        [ForeignKey("MessageId")]
        public virtual Message Message { get; set; }
    }
}