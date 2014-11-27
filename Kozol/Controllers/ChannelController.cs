using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kozol.Models;
using Kozol.Utilities;

namespace Kozol.Controllers
{
    public class ChannelController : Controller
    {
        KozolContainer db = new KozolContainer();

        public ActionResult ChannelIndex()
        {
            var channels = from channel in db.Channels
                           select channel;

            return View(channels.ToList());
        }

        public JsonResult GetChannel(int channelId)
        {
            var channels = from channel in db.Channels
                           where channel.ID == channelId
                           select new ChannelViewModel
                           {
                               ID = channel.ID,
                               Name = channel.Name,
                               CreatorID = channel.Creator.ID,
                               Creator = channel.Creator.Username,
                               Created = channel.Created,
                               Capacity = channel.Capacity,
                               Mode_Admin = channel.Mode_Admin,
                               Mode_Slow = channel.Mode_Slow,
                               Mode_Quiet = channel.Mode_Quiet,
                               Mode_Invite = channel.Mode_Invite
                           };

            return Json(channels.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChannelList()
        {
            var channels = from channel in db.Channels
                           select new ChannelViewModel
                           {
                               ID = channel.ID,
                               Name = channel.Name,
                               CreatorID = channel.Creator.ID,
                               Creator = channel.Creator.Username,
                               Created = channel.Created,
                               Capacity = channel.Capacity,
                               Mode_Admin = channel.Mode_Admin,
                               Mode_Slow = channel.Mode_Slow,
                               Mode_Quiet = channel.Mode_Quiet,
                               Mode_Invite = channel.Mode_Invite
                           };

            return Json(channels.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddChannel(Channel channel)
        {
            channel.Created = DateTime.Now;
            channel.ID = db.Channels.Max(c => c.ID) + 1;
            
            if (Session["userId"] != null)
            {
                channel.Creator = db.Users.Find(Session["userId"]);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            if (channel == null)
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            if (ModelState.IsValid)
            {
                channel.Creator.Administrations.Add(channel);
                channel.Administrators.Add(channel.Creator);
                db.Channels.Add(channel);
                db.SaveChanges();
            }
            //need this because of circular reference caused by users
            return Json(new ChannelViewModel { 
                              ID = channel.ID,
                              Name = channel.Name,
                              CreatorID = channel.Creator.ID,
                              Creator = channel.Creator.Username,
                              Created = channel.Created,
                              Capacity = channel.Capacity,
                              Mode_Admin = channel.Mode_Admin,
                              Mode_Slow = channel.Mode_Slow,
                              Mode_Quiet = channel.Mode_Quiet, 
                              Mode_Invite = channel.Mode_Invite
            }, JsonRequestBehavior.AllowGet);
        }

        //Get all the people who spoke in a channel
        public JsonResult GetChannelParticipants(int channelId)
        {
            var speakers = from Users in db.Users
                           join Messages in db.Messages
                           on Users.ID equals Messages.Sender.ID
                           join Channels in db.Channels
                           on Messages.Destination.ID equals channelId
                           select new { Users.ID, Users.Username };

            return Json(speakers.ToList().Distinct(), JsonRequestBehavior.AllowGet);
        }

        //get all the administrators of a channel
        public JsonResult GetChannelAdmins(int channelID)
        {
            try
            {
                var channel = db.Channels.Find(channelID);
                var admins = from channels in channel.Administrators.ToList()
                             select new { ID = channels.ID, Username = channels.Username };

                return Json(admins.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateChannel(Channel channel)
        {
            //current user trying to execute the action
            try
            {
                var currentUser = db.Users.Find((int)Session["userId"]);
                var updateTarget = db.Channels.Find(channel.ID);
                //check if current user is an admin
                if(!updateTarget.Administrators.Contains(currentUser))
                    return Json(new { success = false, reason = "userid: " + (int)Session["userId"] + " is not an admin of this channel: " + channel.ID }, 
                                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, reason = "current user does not exist" }, JsonRequestBehavior.AllowGet);
            }
                
            if (ModelState.IsValid)
            {
                var target = db.Channels.Find(channel.ID);

                target.Name = channel.Name;
                target.Capacity = channel.Capacity;
                target.Mode_Admin = channel.Mode_Admin;
                target.Mode_Invite = channel.Mode_Invite;
                target.Mode_Quiet = channel.Mode_Quiet;
                target.Mode_Slow = channel.Mode_Slow;

                db.SaveChanges();
                return Json(new { success = true, reason = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, reason = "failed to add model." }, JsonRequestBehavior.AllowGet);
        }

        //get all channels owned by a specific user
        public JsonResult GetUserChannels(string username)
        {
            var activity = from users in db.Users
                           join channel in db.Channels
                           on users.ID equals channel.Creator.ID
                           where users.Username.Contains(username)
                           select new ChannelViewModel
                           {
                               ID = channel.ID,
                               Name = channel.Name,
                               CreatorID = channel.Creator.ID,
                               Creator = channel.Creator.Username,
                               Created = channel.Created,
                               Capacity = channel.Capacity,
                               Mode_Admin = channel.Mode_Admin,
                               Mode_Slow = channel.Mode_Slow,
                               Mode_Quiet = channel.Mode_Quiet,
                               Mode_Invite = channel.Mode_Invite
                           };
            return Json(activity.ToList(), JsonRequestBehavior.AllowGet);
        }

        //search channel
        public JsonResult SearchChannels(string query)
        {
            var channels = from Channels in db.Channels
                           where Channels.Name.Contains(query) || 
                                 Channels.Creator.Username.Contains(query) ||
                                 Channels.ID.ToString() == query
                           select new ChannelViewModel
                           {
                               ID = Channels.ID,
                               Name = Channels.Name,
                               CreatorID = Channels.Creator.ID,
                               Creator = Channels.Creator.Username,
                               Created = Channels.Created,
                               Capacity = Channels.Capacity,
                               Mode_Admin = Channels.Mode_Admin,
                               Mode_Slow = Channels.Mode_Slow,
                               Mode_Quiet = Channels.Mode_Quiet,
                               Mode_Invite = Channels.Mode_Invite
                           };
            return Json(channels.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAdmin(int adminID, int channelID)
        {
            Channel channel;
            User user;
            User currentUser;
            try
            {
                //get the channel
                channel = db.Channels.Find(channelID);
                //get the user
                user = db.Users.Find(adminID);
                //current user
                currentUser = db.Users.Find((int)Session["userId"]);
            }
            catch (Exception e) 
            {
                channel = null;
                user = null;
                currentUser = null;
            }

            //confirm neither are null
            if (channel != null && user != null && ((int)Session["userId"] == channel.Creator.ID) || (channel.Administrators.Contains(currentUser)) )
            {
                //add the user to channel administrator list
                if(!channel.Administrators.Contains(user))
                    channel.Administrators.Add(user);
                //add the channel to the users list of administrations
                if(!user.Administrations.Contains(channel))
                    user.Administrations.Add(channel);

                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}
