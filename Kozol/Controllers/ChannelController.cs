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

        public JsonResult ChannelList()
        {
            var channels = from channel in db.Channels
                           select new ChannelViewModel
                           {
                               ID = channel.ID,
                               Name = channel.Name,
                               Created = channel.Created,
                               Capacity = channel.Capacity,
                               Mode_Admin = channel.Mode_Admin,
                               Mode_Slow = channel.Mode_Slow,
                               Mode_Quiet = channel.Mode_Quiet,
                               Mode_Invite = channel.Mode_Invite
                           };;

            return Json(channels.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddChannel(Channel channel)
        {
            channel.Created = DateTime.Now;
            if (Session["userId"] != null)
            {
                channel.Creator = db.Users.Find(Session["userId"]);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                db.Channels.Add(channel);
                db.SaveChanges();
            }
            //need this because of circular reference caused by users
            return Json(new { ID = channel.ID,
                              //Creator = channel.Creator,
                              Created = channel.Created,
                              Capacity = channel.Capacity,
                              Mode_Admin = channel.Mode_Admin,
                              Mode_Slow = channel.Mode_Slow,
                              Mode_Quiet = channel.Mode_Quiet, 
                              Mode_Invite = channel.Mode_Invite
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
