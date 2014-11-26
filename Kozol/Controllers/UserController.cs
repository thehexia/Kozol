using Kozol.Models;
using Kozol.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Diagnostics;
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

        public ActionResult Register()
        {
            return View("~/Views/Kozol/Register.cshtml");
        }

        [HttpPost]
        //not requiring first or last names right now because who cares
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                CreateUserStatus status = UserManager.CreateUser(model.Email, model.Password, model.Username);
                if (status > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //if we get this far we know bad things happened so redisplay the form
            return View("~/Views/Kozol/Register.cshtml", model);
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

            return Json(userList.ToList(), JsonRequestBehavior.AllowGet);
        }

        //
        // Provide a set of Json Result Wrappers for functions found in UserManager
        //
        public JsonResult SearchUser(string query)
        {
            var users =    from Users in db.Users
                           where Users.Username.Contains(query) || 
                                 Users.Email.Contains(query) ||
                                 Users.NameFirst.Contains(query) ||
                                 Users.NameLast.Contains(query)
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

            Debug.Write(Json(users.ToList(), JsonRequestBehavior.AllowGet));
            return Json(users.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUser(string username)
        {
            var user = (from Users in db.Users
                        where Users.Username.Contains(username)
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
                        }).FirstOrDefault();

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserId(string email)
        {
            return Json(UserManager.GetUserId(email), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserName(string email)
        {
            return Json(UserManager.GetUserName(email), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserName(int userId)
        {
            return Json(UserManager.GetUserName(userId), JsonRequestBehavior.AllowGet);
        }
    }
}
