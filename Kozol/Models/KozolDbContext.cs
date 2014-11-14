using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Kozol.Models
{
    public class KozolDbContext : DbContext
    {
        public KozolDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Administrator> Admins { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<Invite> Invites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                        .HasRequired(a => a.Sender)
                        .WithMany()
                        .HasForeignKey(u => u.SenderId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                        .HasRequired(a => a.Receiver)
                        .WithMany()
                        .HasForeignKey(u => u.ReceiverId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Administrator>()
                        .HasRequired(a => a.Channel)
                        .WithMany()
                        .HasForeignKey(u => u.ChannelId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Administrator>()
                        .HasRequired(a => a.User)
                        .WithMany()
                        .HasForeignKey(u => u.UserId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Speaker>()
                        .HasRequired(a => a.Channel)
                        .WithMany()
                        .HasForeignKey(u => u.ChannelId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Speaker>()
                        .HasRequired(a => a.User)
                        .WithMany()
                        .HasForeignKey(u => u.UserId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Friends>()
                        .HasRequired(a => a.Sender)
                        .WithMany()
                        .HasForeignKey(u => u.SenderId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Friends>()
                        .HasRequired(a => a.Receiver)
                        .WithMany()
                        .HasForeignKey(u => u.ReceiverId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Invite>()
                        .HasRequired(a => a.Channel)
                        .WithMany()
                        .HasForeignKey(u => u.ChannelId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Invite>()
                        .HasRequired(a => a.Sender)
                        .WithMany()
                        .HasForeignKey(u => u.SenderId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Invite>()
                        .HasRequired(a => a.Receiver)
                        .WithMany()
                        .HasForeignKey(u => u.ReceiverId).WillCascadeOnDelete(false);
        }
    }
}