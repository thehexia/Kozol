using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Kozol.Models;
using Kozol.Models.ViewModels;

namespace Kozol.Controllers
{
    public class InviteController : Controller
    {
        KozolContainer db = new KozolContainer();

        public ActionResult MyInvites()
        {
            try
            {
                int userId = (int)Session["userId"];
                var invites = from Invites in db.Invites
                              join Users in db.Users
                              on Invites.SenderID equals Users.ID
                              join Channels in db.Channels
                              on Invites.ChannelID equals Channels.ID
                              where Invites.ReceiverID == userId
                              select new InviteViewModel
                              {
                                  SenderId = Invites.SenderID,
                                  SenderUsername = Users.Username,
                                  ReceieverId = Invites.ReceiverID,
                                  SenderPublicKey = Users.Public_Key_n,
                                  ChannelId = Channels.ID,
                                  ChannelName = Channels.Name,
                                  SharedKey = Invites.Shared_Key
                              };
                return View(invites.ToList());
            }
            catch (Exception e) { }
            return View();
        }

        public JsonResult GetInvites()
        {
            try
            {
                int userId = (int)Session["userId"];
                var invites = from Invites in db.Invites
                              join Users in db.Users
                              on Invites.SenderID equals Users.ID
                              join Channels in db.Channels
                              on Invites.ChannelID equals Channels.ID
                              select new InviteViewModel
                              {
                                  SenderId = Invites.SenderID,
                                  SenderUsername = Users.Username,
                                  ReceieverId = Invites.ReceiverID,
                                  SenderPublicKey = Users.Public_Key_n,
                                  ChannelId = Channels.ID,
                                  ChannelName = Channels.Name,
                                  SharedKey = Invites.Shared_Key
                              };
                return Json(invites.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) { }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddInvite()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddInvite(InviteViewModel inv) {
            Invite invite = new Invite();
            Channel ch = db.Channels.Find(inv.ChannelId);

            User sender = db.Users.Find((int)Session["userId"]);

            User receiver = db.Users
                              .Where(c => c.Username == inv.ReceiverUsername)
                              .FirstOrDefault();

            invite.ChannelID = ch.ID;
            invite.ReceiverID = receiver.ID;
            invite.SenderID = sender.ID;
            invite.Shared_Key = inv.SharedKey;

            if (ModelState.IsValid)
            {
                db.Invites.Add(invite);
                db.SaveChanges();
                return View();
            }
            return View(inv);
        }

        public ActionResult DeleteInvite(int senderId, int channelId)
        {
            int receiverId = (int)Session["userId"];
            var inv = db.Invites.Find(senderId, receiverId, channelId);
            Debug.WriteLine(receiverId);
            Debug.WriteLine(senderId);
            Debug.WriteLine(channelId);

            if (inv != null)
            {
                db.Invites.Remove(inv);
                db.SaveChanges();
            }
            return RedirectToAction("MyInvites");
        }
    }
} //namespace
