using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kozol.Models
{
    public class Administrator
    {
        [Key]
        [Column(Order = 1)]
        public int ChannelId { get; set; }

        [ForeignKey("ChannelId")]
        public virtual Channel Channel { get; set; }

        [Key]
        [Column(Order = 2)]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }

    public class Speaker
    {
        [Key]
        [Column(Order = 1)]
        public int ChannelId { get; set; }

        [ForeignKey("ChannelId")]
        public virtual Channel Channel { get; set; }

        [Key]
        [Column(Order = 2)]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}