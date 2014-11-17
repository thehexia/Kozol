namespace Kozol.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Kozolv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        ChannelId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Channel_ChannelId = c.Int(),
                    })
                .PrimaryKey(t => new { t.ChannelId, t.UserId })
                .ForeignKey("dbo.Channels", t => t.Channel_ChannelId)
                .ForeignKey("dbo.Channels", t => t.ChannelId)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.ChannelId)
                .Index(t => t.UserId)
                .Index(t => t.Channel_ChannelId);
            
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        ChannelId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                        SlowMode = c.Boolean(nullable: false),
                        AdminMode = c.Boolean(nullable: false),
                        QuietMode = c.Boolean(nullable: false),
                        InviteMode = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ChannelId)
                .ForeignKey("dbo.UserProfile", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(nullable: false),
                        ImageId = c.Int(),
                        PublicKey = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        Path = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(nullable: false),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Channel_ChannelId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Channels", t => t.ReceiverId)
                .ForeignKey("dbo.UserProfile", t => t.SenderId)
                .ForeignKey("dbo.Channels", t => t.Channel_ChannelId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.Channel_ChannelId);
            
            CreateTable(
                "dbo.Speakers",
                c => new
                    {
                        ChannelId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Channel_ChannelId = c.Int(),
                    })
                .PrimaryKey(t => new { t.ChannelId, t.UserId })
                .ForeignKey("dbo.Channels", t => t.ChannelId)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.Channels", t => t.Channel_ChannelId)
                .Index(t => t.ChannelId)
                .Index(t => t.UserId)
                .Index(t => t.Channel_ChannelId);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SenderId, t.ReceiverId })
                .ForeignKey("dbo.UserProfile", t => t.ReceiverId)
                .ForeignKey("dbo.UserProfile", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.Invites",
                c => new
                    {
                        InviteId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        ChannelId = c.Int(nullable: false),
                        SharedKey = c.String(),
                    })
                .PrimaryKey(t => t.InviteId)
                .ForeignKey("dbo.Channels", t => t.ChannelId)
                .ForeignKey("dbo.UserProfile", t => t.ReceiverId)
                .ForeignKey("dbo.UserProfile", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.ChannelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invites", "SenderId", "dbo.UserProfile");
            DropForeignKey("dbo.Invites", "ReceiverId", "dbo.UserProfile");
            DropForeignKey("dbo.Invites", "ChannelId", "dbo.Channels");
            DropForeignKey("dbo.Friends", "SenderId", "dbo.UserProfile");
            DropForeignKey("dbo.Friends", "ReceiverId", "dbo.UserProfile");
            DropForeignKey("dbo.Administrators", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Administrators", "ChannelId", "dbo.Channels");
            DropForeignKey("dbo.Speakers", "Channel_ChannelId", "dbo.Channels");
            DropForeignKey("dbo.Speakers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Speakers", "ChannelId", "dbo.Channels");
            DropForeignKey("dbo.Messages", "Channel_ChannelId", "dbo.Channels");
            DropForeignKey("dbo.Channels", "CreatorId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfile", "ImageId", "dbo.Images");
            DropForeignKey("dbo.Images", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.UserProfile");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.Channels");
            DropForeignKey("dbo.Administrators", "Channel_ChannelId", "dbo.Channels");
            DropIndex("dbo.Invites", new[] { "ChannelId" });
            DropIndex("dbo.Invites", new[] { "ReceiverId" });
            DropIndex("dbo.Invites", new[] { "SenderId" });
            DropIndex("dbo.Friends", new[] { "ReceiverId" });
            DropIndex("dbo.Friends", new[] { "SenderId" });
            DropIndex("dbo.Speakers", new[] { "Channel_ChannelId" });
            DropIndex("dbo.Speakers", new[] { "UserId" });
            DropIndex("dbo.Speakers", new[] { "ChannelId" });
            DropIndex("dbo.Messages", new[] { "Channel_ChannelId" });
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Images", new[] { "MessageId" });
            DropIndex("dbo.UserProfile", new[] { "ImageId" });
            DropIndex("dbo.Channels", new[] { "CreatorId" });
            DropIndex("dbo.Administrators", new[] { "Channel_ChannelId" });
            DropIndex("dbo.Administrators", new[] { "UserId" });
            DropIndex("dbo.Administrators", new[] { "ChannelId" });
            DropTable("dbo.Invites");
            DropTable("dbo.Friends");
            DropTable("dbo.Speakers");
            DropTable("dbo.Messages");
            DropTable("dbo.Images");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Channels");
            DropTable("dbo.Administrators");
        }
    }
}
