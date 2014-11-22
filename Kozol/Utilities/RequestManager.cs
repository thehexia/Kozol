using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kozol.Models;

namespace Kozol.Utilities
{
    public class RequestManager
    {
        // Returns true iff insert was successful
        public static bool CreateInvite(int senderId, int receiverId, int channelId, string sharedKey)
        {
            using (KozolContainer db = new KozolContainer())
            {
                try
                {
                    Invite inv = new Invite { SenderID = senderId, ReceiverID = receiverId, ChannelID = channelId, Shared_Key = sharedKey };
                    db.Invites.Add(inv);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e) { return false; }
            }
        }

        // Returns true iff deletion was successful
        // Use this to reject or expire invites
        public static bool DeleteInvite(int senderId, int receiverId, int channelId)
        {
            using (KozolContainer db = new KozolContainer())
            {
                try
                {
                    var inv = db.Invites.Find(senderId, receiverId, channelId);
                    db.Invites.Remove(inv);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e) { return false; }
            }
        }

        public static bool CreateFriendship(int senderId, int receiverId)
        {
            using (KozolContainer db = new KozolContainer())
            {
                try
                {
                    Friendship f = new Friendship { SenderID = senderId, ReceiverID = receiverId };
                    db.Friendships.Add(f);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e) { return false; }
            }
        }

        // Returns true iff the friendship was accepted
        public static bool AcceptFriendship(int senderId, int receiverId)
        {
            using (KozolContainer db = new KozolContainer())
            {
                try
                {
                    var req = db.Friendships.Find(senderId, receiverId);
                    req.Accepted = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e) { return false; }
            }
        }

        // Returns true iff the friendship was successfully rejected and deleted
        public static bool RejectFriendship(int senderId, int receiverId)
        {
            using (KozolContainer db = new KozolContainer())
            {
                try
                {
                    var req = db.Friendships.Find(senderId, receiverId);
                    db.Friendships.Remove(req);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e) { return false; }
            }
        }
    }
}