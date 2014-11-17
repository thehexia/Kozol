using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebMatrix.WebData;
using Kozol.Models;

namespace Kozol.Controllers
{
    using System.Diagnostics;

    public class ChannelController : Controller
    {
        KozolDbContext db = new KozolDbContext();

        public ActionResult Index()
        {
            var channels = from Channels in db.Channels
                           select Channels;
            return View(channels.ToList());
        }

        public JsonResult ChannelList()
        {
            var channels = from Channels in db.Channels
                           select Channels;
            return Json(channels.ToList(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public EmptyResult AddChannel(Channel channel)
        {
            channel.CreatorId = WebSecurity.CurrentUserId;
            channel.Created = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Channels.Add(channel);
                db.SaveChanges();
            }
            return new EmptyResult();
        }
    }
}
