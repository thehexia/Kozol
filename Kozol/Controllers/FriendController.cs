using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kozol.Models;
using Kozol.Models.ViewModels;
using Kozol.Utilities;

namespace Kozol.Controllers
{
    // 
    // I'm now officially treating friends like Xbox Live
    // You can add a friend without them adding you back
    // therefore, its more like a favorites users list
    // this is way simpler
    //
    public class FriendController : Controller
    {
        KozolContainer db = new KozolContainer();

        //main view for friendslist tab
        public ActionResult FriendsList()
        {
            return View();
        }

        //gets current users friends list
        public JsonResult GetFriendsList()
        {
            int userId = (int)Session["userId"];
            if (userId != null)
            {
                var friendsList = from Friends in db.Friendships
                                  where Friends.SenderID == userId
                                  join Users in db.Users
                                  on Friends.ReceiverID equals Users.ID
                                  select new FriendshipViewModel
                                  {
                                      FriendId = Friends.ReceiverID,
                                      FriendUsername = Users.Username,
                                      FriendEmail = Users.Email,
                                      PublicKey = Users.Public_Key_n
                                  };
                return Json(friendsList.ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "not logged in" });
        }

        //add a friend
        public JsonResult AddFriend(string username)
        {
            int receiverId = UserManager.GetUserIdByUsername(username);
            int senderId = (int)Session["userId"];
            if (senderId != null && receiverId > 0)
            {
                bool result = RequestManager.CreateFriendship(senderId, receiverId);
                return Json(new { success = result, message = "" }); 
            }

            return Json(new { success = false, message = "failed to add" });
        }

        public JsonResult RemoveFriend(string username)
        {
            int receiverId = UserManager.GetUserIdByUsername(username);
            int senderId = (int)Session["userId"];
            if (senderId != null && receiverId > 0)
            {
                bool result = RequestManager.RejectFriendship(senderId, receiverId);
                return Json(new { success = result, message = "" });
            }
            return Json(new { success = false, message = "failed to rmv" });

        }
    }
}
