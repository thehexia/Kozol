using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Kozol.Models;

namespace Kozol.Controllers
{
    public class UsersController : Controller
    {
        KozolDbContext db = new KozolDbContext();
        UsersContext users = new UsersContext();

        public JsonResult UserList()
        {
            var userList = from Users in users.UserProfiles
                           select Users;
            return Json(userList.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserEmailLookup(string username)
        {
            var email = (from Users in users.UserProfiles
                         where Users.UserName.Contains(username)
                         select Users).FirstOrDefault();

            return Json(email, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserMessageHistory(string username)
        {
            var msgs = from Users in users.UserProfiles
                       join Messages in db.Messages
                       on Users.UserId equals Messages.SenderId
                       where Users.UserName == username
                       select Messages.Text;

            return Json(msgs.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
