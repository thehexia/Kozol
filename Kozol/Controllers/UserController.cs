using Kozol.Models;
using Kozol.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Kozol.Models.ViewModels;

namespace Kozol.Controllers {
    public class UserController : Controller {

        KozolContainer db = new KozolContainer();

        public ActionResult UsersIndex()
        {
            var userList = from Users in db.Users
                           select new UsersViewModel
                           {
                               ID = Users.ID,
                               Email = Users.Email,
                               LastActivity = Users.LastActivity,
                               LastLogin = Users.LastLogin,
                               Created = Users.Created,
                               Username = Users.Username,
                               NameFirst = Users.NameFirst,
                               NameLast = Users.NameLast,
                               Avatar = Users.Avatar,
                               Avatar_Custom = Users.Avatar_Custom,
                               Public_Key_n = Users.Public_Key_n
                           };
            return View(userList.ToList());
        }

        [HttpPost]
        public ActionResult LoginUser(LoginModel model) {
            if (model.IsValid(model.Email, model.Password)) {
                SetLoginStatus status = UserManager.SetLogin(model.Email, model.Password);
                if (status > 0) {
                    if (model.RememberMe) {
                        HttpCookie authCookie = new HttpCookie("ASP.NET_SessionId", Session.SessionID);
                        authCookie.Expires = DateTime.Now.AddMonths(1);
                        Response.Cookies.Add(authCookie);
                    }
                } else if (status == SetLoginStatus.NotFound) {
                    // Inform user that user account was not found.
                } else {
                    // Some kind of other error.
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogoutUser() {
            HttpCookie authCookie = new HttpCookie("ASP.NET_SessionId", Session.SessionID);
            authCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(authCookie);
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetUserList()
        {
            var userList = from Users in db.Users
                           select new MinUsersViewModel 
                           {
                               Username = Users.Username,
                               NameFirst = Users.NameFirst,
                               NameLast = Users.NameLast,
                               Avatar = Users.Avatar,
                               Avatar_Custom = Users.Avatar_Custom,
                               Public_Key_n = Users.Public_Key_n
                           };

            return Json(userList.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
