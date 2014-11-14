using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Kozol.Models;

namespace Kozol.Controllers
{
    public class ChannelController : Controller
    {
        KozolDbContext db = new KozolDbContext();
        UsersContext users = new UsersContext();

        public ActionResult Index()
        {
            var channels = from Channels in db.Channels
                           select Channels;
            return View(channels);
        }

        public EmptyResult AddChannel()
        {
            Channel c = new Channel();

            c.Name = "Test";
            c.Capacity = 2;
            c.Created = DateTime.Now;
            c.Creator = users.UserProfiles.FirstOrDefault(u => u.UserId == WebSecurity.CurrentUserId);
            c.AdminMode = false;
            c.InviteMode = false;
            c.QuietMode = false;
            c.SlowMode = false;

            db.Channels.Add(c);
            db.SaveChanges();

            return new EmptyResult();
        }
    }
}
