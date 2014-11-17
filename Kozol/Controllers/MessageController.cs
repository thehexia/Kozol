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

        // Gets messages of a specific chat room
        // Returns json result

        // Inserts a message
        // Returns true iff insert was successful else false
        [Authorize]
        public bool AddMessage(Message message)
        {
            message.Timestamp = DateTime.Now;
            message.SenderId = WebSecurity.CurrentUserId;

            if (ModelState.IsValid)
            {
                //confirm that there is a channel with the receiving id
                var channel = db.Channels.Find(message.ReceiverId);
                if (channel == null)
                    return false;

                db.Messages.Add(message);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
