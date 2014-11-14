using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Kozol.Models
{
    public class KozolDbContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Administrator> Admins { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<Invite> Invites { get; set; }
    }
}