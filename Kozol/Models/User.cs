//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kozol.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Avatar_Custom = false;
            this.Messages = new HashSet<Message>();
            this.Administrations = new HashSet<Channel>();
            this.Creations = new HashSet<Channel>();
            this.Speaches = new HashSet<Channel>();
            this.FriendshipsSent = new HashSet<Friendship>();
            this.FriendshipsReceived = new HashSet<Friendship>();
            this.InvitesSent = new HashSet<Invite>();
            this.InvitesReceived = new HashSet<Invite>();
            this.Roles = new HashSet<UserRoleMap>();
        }
    
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> LastActivity { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public string Salt { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Username { get; set; }
        public Nullable<System.Guid> Avatar { get; set; }
        public bool Avatar_Custom { get; set; }
        public int Public_Key_n { get; set; }
        public int Public_Key_e { get; set; }
    
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Channel> Administrations { get; set; }
        public virtual ICollection<Channel> Creations { get; set; }
        public virtual ICollection<Channel> Speaches { get; set; }
        public virtual ICollection<Friendship> FriendshipsSent { get; set; }
        public virtual ICollection<Friendship> FriendshipsReceived { get; set; }
        public virtual ICollection<Invite> InvitesSent { get; set; }
        public virtual ICollection<Invite> InvitesReceived { get; set; }
        public virtual ICollection<UserRoleMap> Roles { get; set; }
    }
}
