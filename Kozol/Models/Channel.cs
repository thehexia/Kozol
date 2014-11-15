using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kozol.Models
{
    public class Channel
    {
        [Key]
        public int ChannelId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public virtual UserProfile Creator { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Capacity { get; set; }

        [Required]
        public bool SlowMode { get; set; }

        [Required]
        public bool AdminMode { get; set; }

        [Required]
        public bool QuietMode { get; set; }

        [Required]
        public bool InviteMode { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
        public virtual ICollection<Administrator> Admins { get; set; }
    }
}