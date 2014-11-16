using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Kozol.Models;

namespace Kozol.Controllers
{
    public class MessageController : Controller
    {
        KozolDbContext db = new KozolDbContext();
        UsersContext users = new UsersContext();

        [Authorize]
        public void InsertMessage(Message message)
        {
            message.Timestamp = DateTime.Now;
            message.SenderId = WebSecurity.CurrentUserId;

            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }
    }
}
