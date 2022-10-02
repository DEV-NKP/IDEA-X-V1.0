using ClosedXML.Excel;
using Ganss.XSS;
using IDEA_X.HelperClasses;
using IDEA_X.IDEAX_Class;
using IDEA_X.Models;
using IDEA_X.Models.EntityFramework;
using IDEA_X.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IDEA_X.Controllers
{
    [LogInAttr]

    public class UserController : Controller
    {
        // GET: Post
        public ActionResult Timeline()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();

        

            var post_actions = db.POST_ACTIONS.Where(x => x.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase) );
            var posts = (from p in db.GENERAL_POSTS.Where (x => x.POSTING_STATUS.Equals("ACTIVE"))
                         join p_a in post_actions
                         on p.POST_ID equals p_a.POST_ID into p_t
                         from post in p_t.DefaultIfEmpty()
                         orderby p.POST_ID descending
                         select new POST_TIMELINE
                         {
                             POST_ID = p.POST_ID,
                             POST_TAG = p.POST_TAG,
                             TIMELINE_TEXT = p.TIMELINE_TEXT,
                             AUTHOR = p.AUTHOR,
                             POST_LIKE = p.POST_LIKE,
                             POST_DISLIKE = p.POST_DISLIKE,
                             POST_STATUS = post.POST_STATUS,
                             TIMELINE_IMAGE = p.TIMELINE_IMAGE,
                         }).ToList();

            var postWithImg = (from p in posts
                               join u_d in db.USER_DETAILS
                               on p.AUTHOR equals u_d.USERNAME into pi_t
                               from post in pi_t.DefaultIfEmpty()
                               select new POST_TIMELINE
                               {
                                   POST_ID = p.POST_ID,
                                   POST_TAG = p.POST_TAG,
                                   TIMELINE_TEXT = p.TIMELINE_TEXT,
                                   AUTHOR = p.AUTHOR,
                                   POST_LIKE = p.POST_LIKE,
                                   POST_DISLIKE = p.POST_DISLIKE,
                                   POST_STATUS = p.POST_STATUS,
                                   TIMELINE_IMAGE = p.TIMELINE_IMAGE,
                                   PROFILE_IMG = post.PROFILE_PICTURE,
                               }).ToList();
            foreach (var prop in postWithImg)
            {
                var postsRep = (from pa in db.POST_REPORT.Where(x => x.REPORTED_BY.Equals(uname) && x.POST_ID == prop.POST_ID)
                             select pa).SingleOrDefault();
                if (postsRep != null)
                {
                    prop.REPORTED_POST = "REPORTED";
                }
                else { 
                 prop.REPORTED_POST = "CLEAR";
                }

            }

            //TODO : only passed a single object in view --adb
            return View(postWithImg.FirstOrDefault());
        }
        public ActionResult Profile()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();

            var user_details = (from u_d in db.USER_DETAILS
                                where u_d.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase)
                                select u_d
                                ).SingleOrDefault();

            return View(user_details);
        }
        public ActionResult Personal_Timeline()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var post_actions = db.POST_ACTIONS.Where(x => x.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase));
            var posts = (from p in db.GENERAL_POSTS
                         where p.AUTHOR.Trim().ToLower().Contains(uname.Trim().ToLower()) 
                         join p_a in post_actions
                         on p.POST_ID equals p_a.POST_ID into p_t
                         from post in p_t.DefaultIfEmpty()
                         orderby p.POST_ID descending
                         select new POST_TIMELINE
                         {
                             POST_ID = p.POST_ID,
                             POST_TAG = p.POST_TAG,
                             TIMELINE_TEXT = p.TIMELINE_TEXT,
                             AUTHOR = p.AUTHOR,
                             POST_LIKE = p.POST_LIKE,
                             POST_DISLIKE = p.POST_DISLIKE,
                             TIMELINE_IMAGE = p.TIMELINE_IMAGE,
                             POST_STATUS = post.POST_STATUS,
                             POSTING_STATUS = p.POSTING_STATUS
                         }).ToList();

            var postWithImg = (from p in posts
                               join u_d in db.USER_DETAILS
                               on p.AUTHOR equals u_d.USERNAME into pi_t
                               from post in pi_t.DefaultIfEmpty()
                               select new POST_TIMELINE
                               {
                                   POST_ID = p.POST_ID,
                                   POST_TAG = p.POST_TAG,
                                   TIMELINE_TEXT = p.TIMELINE_TEXT,
                                   AUTHOR = p.AUTHOR,
                                   POST_LIKE = p.POST_LIKE,
                                   POST_DISLIKE = p.POST_DISLIKE,
                                   POST_STATUS = p.POST_STATUS,
                                   POSTING_STATUS = p.POSTING_STATUS,
                                   TIMELINE_IMAGE = p.TIMELINE_IMAGE,
                                   PROFILE_IMG = post.PROFILE_PICTURE,
                               }).ToList();
            return View(postWithImg);
        }

        public ActionResult GetImage(string name)
        {
            var db = new IDEA_XEntities();
            var img = (from i in db.USER_DETAILS
                       where i.USERNAME.Equals(name, StringComparison.OrdinalIgnoreCase)
                       select i.PROFILE_PICTURE).SingleOrDefault();
            if (img != null)
            {

                return File(img, "image/jpg");
            }
            return new EmptyResult();

        }

        public ActionResult GetPostImage(int id)
        {
            var db = new IDEA_XEntities();
            var img = (from i in db.GENERAL_POSTS
                       where i.POST_ID == id
                       select i.TIMELINE_IMAGE
                       ).SingleOrDefault();
            if (img != null)
            {
                return File(img, "image/jpg");
            }
            return new EmptyResult();
        }

        [HttpPost]
  public ActionResult UserActivity()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var countLike = (from i in db.GENERAL_POSTS
                       where i.AUTHOR == uname
                       select i.POST_LIKE
                       ).ToList();
            int like = 0;
            int dislike = 0;
            foreach (int a in countLike)
            {
                like += a;
            }

            var countDisLike = (from i in db.GENERAL_POSTS
                             where i.AUTHOR == uname
                             select i.POST_DISLIKE
                     ).ToList();

            foreach (int a in countDisLike)
            {
                dislike += a;
            }

            return Json(new { like = like, dislike = dislike, JsonRequestBehavior.AllowGet });
        }


        public ActionResult GetPostImageUser()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var img = (from i in db.USER_DETAILS
                       where i.USERNAME.Equals(uname)
                       select i.PROFILE_PICTURE
                       ).SingleOrDefault();
            if (img != null)
            {
                return File(img, "image/jpg");
            }
            return new EmptyResult();
        }

        public ActionResult SearchMessage(string Text)
        {
            var db = new IDEA_XEntities();
            var msg = db.ALL_USERS.Where(val => val.USERNAME.Trim().ToLower().Contains(Text.ToLower())).ToList();



            return Json(new { MSG_LIST = msg, JsonRequestBehavior.AllowGet });
        }


        public ActionResult Chat(string id)
        {
            if(Session["s_name"] != null)
            {
                Session.Remove("s_name");
            }
            if(Session["r_name"] != null)
            {
                Session.Remove("r_name");
            }
            var db = new IDEA_XEntities();
            var user_detail = db.USER_DETAILS.Where(x => x.USERNAME.Contains(id.ToLower())).ToList();
            var posts = (from p in db.USER_DETAILS
                         where p.USERNAME == id
                         select p).ToList();
            return View(posts);

        }


        //[HttpPost]
        //public ActionResult ChatSearch(string SEARCH_MESSAGE)
        //{
        //    var db = new IDEA_XEntities();
        //    var user_detail = db.USER_DETAILS.Where(x => x.USERNAME.Contains(SEARCH_MESSAGE)).ToList();

        //    return View(user_detail);

  [HttpPost]
        public ActionResult PostAction(int PostId, String VoteCondition)
        {
            
            var db = new IDEA_XEntities();
            var uname = (string)Session["UNAME"];
            var upvoteCount = 0;
            var downvoteCount = 0;
            if (VoteCondition.Equals("UPVOTE-DECLINE"))
            { 
            
           
            var post = (from p in db.GENERAL_POSTS
                        where p.POST_ID == PostId
                        select p).SingleOrDefault();


            post.POST_LIKE--;
                db.SaveChanges();

                var action = db.POST_ACTIONS.Where(a =>
                a.USERNAME.Equals(uname) && a.POST_ID == PostId).SingleOrDefault();

                db.POST_ACTIONS.Remove(action);
                db.SaveChanges();

                upvoteCount = post.POST_LIKE;
                downvoteCount = post.POST_DISLIKE;
            }

            if (VoteCondition.Equals("UPVOTE-ACCEPT"))
            {


                var post = (from p in db.GENERAL_POSTS
                            where p.POST_ID == PostId
                            select p).SingleOrDefault();


                post.POST_LIKE++;

                db.SaveChanges();
                var action = db.POST_ACTIONS.Where(a =>
                a.USERNAME.Equals(uname) && a.POST_ID == PostId).SingleOrDefault();

                if (action != null) {
                 
                        var postCdown = (from p in db.GENERAL_POSTS
                            where p.POST_ID == PostId
                            select p).SingleOrDefault();


                    postCdown.POST_DISLIKE--;
                    
                    db.POST_ACTIONS.Remove(action);
                    db.SaveChanges();
                }


              
                var p_a = new POST_ACTIONS();
                p_a.POST_ID = PostId;
                p_a.USERNAME = uname;
                p_a.POST_STATUS = "UPVOTE";
                db.POST_ACTIONS.Add(p_a);
                db.SaveChanges();

                upvoteCount = post.POST_LIKE;
                downvoteCount = post.POST_DISLIKE;
            }

            if (VoteCondition.Equals("DOWNVOTE-DECLINE"))
            {


                var post = (from p in db.GENERAL_POSTS
                            where p.POST_ID == PostId
                            select p).SingleOrDefault();


                post.POST_DISLIKE--;
                db.SaveChanges();

                var action = db.POST_ACTIONS.Where(a =>
                a.USERNAME.Equals(uname) && a.POST_ID == PostId).SingleOrDefault();

                db.POST_ACTIONS.Remove(action);
                db.SaveChanges();

                upvoteCount = post.POST_LIKE;
                downvoteCount = post.POST_DISLIKE;
            }


            if (VoteCondition.Equals("DOWNVOTE-ACCEPT"))
            {


                var post = (from p in db.GENERAL_POSTS
                            where p.POST_ID == PostId
                            select p).SingleOrDefault();


                post.POST_DISLIKE++;

                db.SaveChanges();
                var action = db.POST_ACTIONS.Where(a =>
                a.USERNAME.Equals(uname) && a.POST_ID == PostId).SingleOrDefault();

                if (action != null)
                {

                    var postCup = (from p in db.GENERAL_POSTS
                                     where p.POST_ID == PostId
                                     select p).SingleOrDefault();


                    postCup.POST_LIKE--;

                    db.POST_ACTIONS.Remove(action);
                    db.SaveChanges();
                }



                var p_a = new POST_ACTIONS();
                p_a.POST_ID = PostId;
                p_a.USERNAME = uname;
                p_a.POST_STATUS = "DOWNVOTE";
                db.POST_ACTIONS.Add(p_a);
                db.SaveChanges();

                upvoteCount = post.POST_LIKE;
                downvoteCount = post.POST_DISLIKE;
            }


   db.SaveChanges();

            return Json(new { upvoteCount = upvoteCount, downvoteCount= downvoteCount, JsonRequestBehavior.AllowGet });


        }




 

        public ActionResult Logout()
        {
            foreach (string key in Request.Cookies.AllKeys)
            {
                HttpCookie c = Request.Cookies[key];
                c.Expires = DateTime.Now.AddMonths(-1);
                Response.AppendCookie(c);
            }


            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SearchPost(string searchText)
        {
            var db = new IDEA_XEntities();
            var post = db.GENERAL_POSTS.Where(val => (val.POST_TAG.ToLower().Contains(searchText.ToLower()) || val.AUTHOR.ToLower().Contains(searchText.ToLower())) && val.POSTING_STATUS.Equals("ACTIVE")).ToList();
            //return Json(new { POST_LIST = post, JsonRequestBehavior.AllowGet });
            var result = new { POST_LIST = post };
            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;

            //var returnVal = new ContentResult
            //{
            //    Content = serializer.Serialize(result),
            //    ContentType = "application/json",
            //    ContentEncoding = Encoding.UTF8,
            //};
            //return returnVal;
            return new IDEAX_Function().SerializeJsonRequest(result);

        }


        [HttpPost]
        public ActionResult ChatSearch(string SEARCH_MESSAGE)
        {
            var db = new IDEA_XEntities();
            var user_detail = db.USER_DETAILS.Where(x => x.USERNAME.Contains(SEARCH_MESSAGE)).ToList();

            return View(user_detail);

        }
        [HttpPost]
        public ActionResult SearchMate(string searchText)
        {
            var db = new IDEA_XEntities();
            var user = db.USER_DETAILS.Where(val => val.USERNAME.Trim().ToLower().Contains(searchText.ToLower()) || val.FIRST_NAME.Trim().ToLower().Contains(searchText.ToLower()) || val.LAST_NAME.Trim().ToLower().Contains(searchText.ToLower())).ToList();
            //return Json(new { MATE_LIST = user, JsonRequestBehavior.AllowGet });
            var result =new { MATE_LIST = user };
            return new IDEAX_Function().SerializeJsonRequest(result);
        }

        [HttpGet]
        public ActionResult ChatMate(string id)
        {
            string uname = (string)Session["UNAME"];
            string mname = id;
            var db = new IDEA_XEntities();
            var user = db.CHAT_BOXS.Where(val => (val.SENDER.Equals(uname) && val.RECEIVER.Equals(mname)) || (val.SENDER.Equals(mname) && val.RECEIVER.Equals(uname))).SingleOrDefault();
            if (user == null)
            {
                return RedirectToAction("ChatRequest/" + mname);
            }
            Session["s_name"] = uname;
            Session["r_name"] = id;
            //var chatlist = (from m in db.USER_MESSAGES
            //                where m.SESSION_NAME == user.CHAT_SESSION
            //                join u in db.USER_DETAILS
            //                on m.SENDER equals u.USERNAME into cl
            //                from chat in cl.DefaultIfEmpty()
            //                select new USER_CHAT_MESSAGE
            //                {
            //                    MESSAGE = m,
            //                    PROFILE_IMG = chat.PROFILE_PICTURE

            //                }
            //                ).ToList();

            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;

            //var resultContent = new ContentResult
            //{
            //    Content = serializer.Serialize(chatlist),
            //    ContentType = "application/json",
            //    ContentEncoding = Encoding.UTF8
            //};
            //return resultContent;
            //return Json(serializer.Serialize(chatlist), JsonRequestBehavior.AllowGet);
            var reciever = (from r in db.USER_DETAILS
                            where r.USERNAME.Equals(mname)
                            select r
                            ).SingleOrDefault();

            return View(reciever);
        }

        [HttpPost]
        public ActionResult GetChatHistory()
        {
            string sender =(string) Session["s_name"];
            string receiver =(string) Session["r_name"];
            var db = new IDEA_XEntities();
            var user = db.CHAT_BOXS.Where(val => (val.SENDER.Equals(sender) && val.RECEIVER.Equals(receiver)) || (val.SENDER.Equals(receiver) && val.RECEIVER.Equals(sender))).SingleOrDefault();
       
            var chatlist = (from m in db.USER_MESSAGES
                            where m.SESSION_NAME == user.CHAT_SESSION
                            join u in db.USER_DETAILS
                            on m.SENDER equals u.USERNAME into cl
                            from chat in cl.DefaultIfEmpty()
                            select new USER_CHAT_MESSAGE
                            {
                                MESSAGE = m,
                                PROFILE_IMG = chat.PROFILE_PICTURE

                            }
                            ).ToList();
            var d_chatlist = chatlist.Select(s =>
            {
                s.MESSAGE.USER_MESSAGE = EncryptionAndHashLogic.DecryptMsg(s.MESSAGE.USER_MESSAGE,
                    s.MESSAGE.SESSION_NAME, user.CHAT_TIME);
                return s;
            }).ToList();
           
            var chats = new { list = d_chatlist, sender = Session["s_name"] };
            
            return new IDEAX_Function().SerializeJsonRequest(chats);

        }
        [HttpPost]
        public ActionResult SendChatMessage(string msg)
        {
            // var sanitizer = new HtmlSanitizer();
            //string chat_msg = msg;//sanitizer.Sanitize(msg);
            string sender = (string)Session["s_name"];
            string receiver = (string)Session["r_name"];
            var db = new IDEA_XEntities();
            var user1 = db.CHAT_BOXS.Where(val => (val.SENDER.Equals(sender) && val.RECEIVER.Equals(receiver)) || (val.SENDER.Equals(receiver) && val.RECEIVER.Equals(sender))).SingleOrDefault();
            //TODO : changed here --adb
            string chat_msg = EncryptionAndHashLogic.EncryptMsg(msg.Trim(),
                user1.CHAT_SESSION, user1.CHAT_TIME);

            var user_msg = new USER_MESSAGES
            {
                SENDER = sender,
                RECEIVER = receiver,
                USER_MESSAGE = chat_msg,
                SESSION_NAME = user1.CHAT_SESSION,
                MESSAGE_TIME = new IDEAX_Function().time(),
            };

            db.USER_MESSAGES.Add(user_msg);
            db.SaveChanges();

            
            var user = db.CHAT_BOXS.Where(val => (val.SENDER.Equals(sender) && val.RECEIVER.Equals(receiver)) || (val.SENDER.Equals(receiver) && val.RECEIVER.Equals(sender))).SingleOrDefault();
            //if(user == null)
            //{
            //    return Json("Error getting data", JsonRequestBehavior.AllowGet);
            //}

            var chatlist = (from m in db.USER_MESSAGES
                            where m.SESSION_NAME == user.CHAT_SESSION
                            && m.MESSAGE_TIME == user_msg.MESSAGE_TIME
                            join u in db.USER_DETAILS
                            on m.SENDER equals u.USERNAME into cl
                            from chat in cl.DefaultIfEmpty()
                            select new USER_CHAT_MESSAGE
                            {
                                MESSAGE = m,
                                PROFILE_IMG = chat.PROFILE_PICTURE

                            }
                            ).ToList();

            /* var serializer = new JavaScriptSerializer();
             serializer.MaxJsonLength = Int32.MaxValue;*/
            //TODO : changed here --adb
            var d_chatlist = chatlist.Select(s =>
            {
                s.MESSAGE.USER_MESSAGE = EncryptionAndHashLogic.DecryptMsg(s.MESSAGE.USER_MESSAGE,
                    s.MESSAGE.SESSION_NAME, user.CHAT_TIME);
                return s;
            }).ToList();
            var chats = new { list = d_chatlist, sender = Session["s_name"] };
        /*    var resultContent = new ContentResult
            {
                Content = serializer.Serialize(chats),
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };*/
           // return resultContent;
            return new IDEAX_Function().SerializeJsonRequest(chats);


           // return Json(new {val = chats}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ChatRequest(string id)
        {
            string uname = (string)Session["UNAME"];
            string mname = id;
            var db = new IDEA_XEntities();
            var user = db.USER_DETAILS.Where(val => val.USERNAME.Equals(mname)).SingleOrDefault();


            return View(user);
        }

        [HttpPost]
        public ActionResult SendChatRequest(string mname)
        {

            string uname = (string)Session["UNAME"];

            var db = new IDEA_XEntities();

            MESSAGE_REQUESTS reqdetails = new MESSAGE_REQUESTS();


            reqdetails.SENDER = uname;
            reqdetails.RECEIVER = mname;
            reqdetails.MESSAGE_TIME = new IDEAX_Function().time();

            db.MESSAGE_REQUESTS.Add(reqdetails);
            db.SaveChanges();
            return Json(new { req = reqdetails, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult AcceptChatRequest(string mname)
        {

            string uname = (string)Session["UNAME"];

            var db = new IDEA_XEntities();

            var urec = db.MESSAGE_REQUESTS.Where(val => val.SENDER.Equals(mname) && val.RECEIVER.Equals(uname)).SingleOrDefault();
            CHAT_BOXS chab = new CHAT_BOXS();
            if (urec != null)
            {

                db.MESSAGE_REQUESTS.Remove(urec);
                db.SaveChanges();
                string session = uname + "_TO_" + mname;
                chab.CHAT_SESSION = session;
                chab.SENDER = uname;
                chab.RECEIVER = mname;
                chab.CHAT_TIME = new IDEAX_Function().time();
                db.CHAT_BOXS.Add(chab);
                db.SaveChanges();
            }





            return Json(new { req = chab, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult DeleteChatRequest(string mname)
        {

            string uname = (string)Session["UNAME"];

            var db = new IDEA_XEntities();

            var urec = db.MESSAGE_REQUESTS.Where(val => val.SENDER.Equals(mname) && val.RECEIVER.Equals(uname)).SingleOrDefault();
            CHAT_BOXS chab = new CHAT_BOXS();
            if (urec != null)
            {

                db.MESSAGE_REQUESTS.Remove(urec);
                db.SaveChanges();
            
            }





            return Json(new { req = chab, JsonRequestBehavior.AllowGet });
        }
       [HttpPost]
        public ActionResult CancelChatRequest(string mname)
        {

            string uname = (string)Session["UNAME"];

            var db = new IDEA_XEntities();

            var urec = db.MESSAGE_REQUESTS.Where(val => val.SENDER.Equals(uname) && val.RECEIVER.Equals(mname)).SingleOrDefault();
            CHAT_BOXS chab = new CHAT_BOXS();
            if (urec != null)
            {

                db.MESSAGE_REQUESTS.Remove(urec);
                db.SaveChanges();
            
            }





            return Json(new { req = chab, JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public ActionResult CheckChatRequest(string mname)
        {

            string uname = (string)Session["UNAME"];
            string msg1 = "";
            var db = new IDEA_XEntities();

            var urec = db.MESSAGE_REQUESTS.Where(val => val.SENDER.Equals(mname) && val.RECEIVER.Equals(uname)).SingleOrDefault();

            if (urec != null)
            {
                msg1 = "WAITING_ACCEPT";
            }
            else
            {
                var usender = db.MESSAGE_REQUESTS.Where(val => val.SENDER.Equals(uname) && val.RECEIVER.Equals(mname)).SingleOrDefault();
                if (usender != null)
                {
                    msg1 = "ALREADY_SEND";
                }

            }
            return Json(new { msg = msg1, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult SearchOldMate()
        {
            string uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            //  var user = db.CHAT_BOXS.Where(val => val.SENDER.Equals(uname) || val.RECEIVER.Equals(uname)).ToList();



            var rec_user = db.CHAT_BOXS.Where(val => val.SENDER.Equals(uname) && !val.RECEIVER.Equals(uname)).ToList();
            var send_user = db.CHAT_BOXS.Where(val => val.RECEIVER.Equals(uname) && !val.SENDER.Equals(uname)).ToList();
            var both_user = db.CHAT_BOXS.Where(val => val.RECEIVER.Equals(uname) && val.SENDER.Equals(uname)).ToList();

            var req_user = db.MESSAGE_REQUESTS.Where(val => val.RECEIVER.Equals(uname)).ToList();
            var send_req = db.MESSAGE_REQUESTS.Where(val => val.SENDER.Equals(uname) && !val.RECEIVER.Equals(uname)).ToList();
            List<USER_DETAILS> user = new List<USER_DETAILS>();

            foreach (var u in rec_user)
            {
                user.Add(db.USER_DETAILS.Where(val => val.USERNAME.Equals(u.RECEIVER)).SingleOrDefault());
            }
            foreach (var u in send_user)
            {
                user.Add(db.USER_DETAILS.Where(val => val.USERNAME.Equals(u.SENDER)).SingleOrDefault());
            }
            foreach (var u in both_user)
            {
                user.Add(db.USER_DETAILS.Where(val => val.USERNAME.Equals(u.SENDER)).SingleOrDefault());
            }
            foreach (var u in req_user)
            {
                user.Add(db.USER_DETAILS.Where(val => val.USERNAME.Equals(u.SENDER)).SingleOrDefault());
            }
            foreach (var u in send_req)
            {
                user.Add(db.USER_DETAILS.Where(val => val.USERNAME.Equals(u.RECEIVER)).SingleOrDefault());
            }

            //return Json(new { OLD_MATE_LIST = user},JsonRequestBehavior.AllowGet);
            var result = new { OLD_MATE_LIST = user };
            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;

            //var returnVal = new ContentResult
            //{
            //    Content = serializer.Serialize(result),
            //    ContentType = "application/json",
            //    ContentEncoding = Encoding.UTF8,
            //};
            //return returnVal;
            return new IDEAX_Function().SerializeJsonRequest(result);
        }


        public ActionResult SearchPostView(int id)
        {
            //var uname = (string)Session["UNAME"];
            //var db = new IDEA_XEntities();
            //var post_actions = db.POST_ACTIONS.Where(x => x.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase));
            //var posts = (from p in db.GENERAL_POSTS
            //             where p.POST_ID == id
            //             join p_a in post_actions
            //             on p.POST_ID equals p_a.POST_ID into p_t
            //             from post in p_t.DefaultIfEmpty()
            //             select new POST_TIMELINE
            //             {
            //                 POST_ID = p.POST_ID,
            //                 POST_TAG = p.POST_TAG,
            //                 TIMELINE_TEXT = p.TIMELINE_TEXT,
            //                 AUTHOR = p.AUTHOR,
            //                 POST_LIKE = p.POST_LIKE,
            //                 POST_DISLIKE = p.POST_DISLIKE,
            //                 POST_STATUS = post.POST_STATUS
            //             }).ToList();
            //return View(posts);
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var post_actions = db.POST_ACTIONS.Where(x => x.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase));
            var posts = (from p in db.GENERAL_POSTS
                         where p.POST_ID == id
                         join p_a in post_actions
                         on p.POST_ID equals p_a.POST_ID into p_t
                         from post in p_t.DefaultIfEmpty()
                         select new POST_TIMELINE
                         {
                             POST_ID = p.POST_ID,
                             POST_TAG = p.POST_TAG,
                             TIMELINE_TEXT = p.TIMELINE_TEXT,
                             AUTHOR = p.AUTHOR,
                             POST_LIKE = p.POST_LIKE,
                             POST_DISLIKE = p.POST_DISLIKE,
                             TIMELINE_IMAGE = p.TIMELINE_IMAGE,
                             POST_STATUS = post.POST_STATUS
                         }).ToList();

            var postWithImg = (from p in posts
                               join u_d in db.USER_DETAILS
                               on p.AUTHOR equals u_d.USERNAME into pi_t
                               from post in pi_t.DefaultIfEmpty()
                               select new POST_TIMELINE
                               {
                                   POST_ID = p.POST_ID,
                                   POST_TAG = p.POST_TAG,
                                   TIMELINE_TEXT = p.TIMELINE_TEXT,
                                   AUTHOR = p.AUTHOR,
                                   POST_LIKE = p.POST_LIKE,
                                   POST_DISLIKE = p.POST_DISLIKE,
                                   POST_STATUS = p.POST_STATUS,
                                   TIMELINE_IMAGE = p.TIMELINE_IMAGE,
                                   PROFILE_IMG = post.PROFILE_PICTURE,
                               }).ToList();

            foreach (var prop in postWithImg)
            {
                var postsRep = (from pa in db.POST_REPORT.Where(x => x.REPORTED_BY.Equals(uname) && x.POST_ID == prop.POST_ID)
                                select pa).SingleOrDefault();
                if (postsRep != null)
                {
                    prop.REPORTED_POST = "REPORTED";
                }
                else
                {
                    prop.REPORTED_POST = "CLEAR";
                }

            }


            return View(postWithImg);
        }

        public ActionResult DeletePost(int id)
        {
            var db = new IDEA_XEntities();
            var post_actions = (from p_a in db.POST_ACTIONS
                                where p_a.POST_ID == id
                                select p_a).ToList();
            post_actions.ForEach(val => db.POST_ACTIONS.Remove(val));

            var post_reports = (from p_a in db.POST_REPORT
                                where p_a.POST_ID == id
                                select p_a).ToList();
            post_reports.ForEach(val => db.POST_REPORT.Remove(val));

            var posts = (from p in db.GENERAL_POSTS
                         where p.POST_ID == id
                         select p
                         ).ToList();

            posts.ForEach(val => db.GENERAL_POSTS.Remove(val));

            db.SaveChanges();
            TempData["deletePostSucess"] = "Post deleted sucessfully";
            return RedirectToAction("Personal_Timeline", "User");
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var uname = Session["UNAME"].ToString();
            var db = new IDEA_XEntities();
            var user_details = (from u_d in db.USER_DETAILS
                                where u_d.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase)
                                select u_d
                           ).SingleOrDefault();
            return View(user_details);
        }


        [HttpPost]
        public ActionResult EditProfile(EditUserProfile updateUser)
        {
            if (ModelState.IsValid)
            {

                var uname = Session["UNAME"].ToString();
                var db = new IDEA_XEntities();
                var user_details = (from u_d in db.USER_DETAILS
                                    where u_d.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase)
                                    select u_d).SingleOrDefault();


                if (updateUser.FIRST_NAME != null)
                {
                    user_details.FIRST_NAME = updateUser.FIRST_NAME;
                }
                if (updateUser.LAST_NAME != null)
                {
                    user_details.LAST_NAME = updateUser.LAST_NAME;
                }
                if (updateUser.HEADLINE != null)
                {
                    user_details.HEADLINE = updateUser.HEADLINE;
                }
                if (updateUser.DATE_OF_BIRTH != null)
                {
                    user_details.DATE_OF_BIRTH = updateUser.DATE_OF_BIRTH;
                }
                if (updateUser.GENDER != null)
                {
                    user_details.GENDER = updateUser.GENDER;
                }
                if (updateUser.MOBILE != null)
                {
                    user_details.MOBILE = updateUser.MOBILE;
                }
                if (updateUser.USER_ADDRESS != null)
                {
                    user_details.USER_ADDRESS = updateUser.USER_ADDRESS;
                }
                if (updateUser.USER_STATE != null)
                {
                    user_details.USER_STATE = updateUser.USER_STATE;
                }
                if (updateUser.ZIP_CODE != null)
                {
                    user_details.ZIP_CODE = updateUser.ZIP_CODE;
                }
                if (updateUser.COUNTRY != null)
                {
                    user_details.COUNTRY = updateUser.COUNTRY;
                }
                if (updateUser.INDUSTRY != null)
                {
                    user_details.INDUSTRY = updateUser.INDUSTRY;
                }
                if (updateUser.EDUCATIONAL_INSTITUTION != null)
                {
                    user_details.EDUCATIONAL_INSTITUTION = updateUser.EDUCATIONAL_INSTITUTION;
                }
                if (updateUser.DEPARTMENT != null)
                {
                    user_details.DEPARTMENT = updateUser.DEPARTMENT;
                }
                if (updateUser.CONTACT_URL != null)
                {
                    user_details.CONTACT_URL = updateUser.CONTACT_URL;
                }

                if (updateUser.USER_STATUS != null)
                {
                    user_details.USER_STATUS = updateUser.USER_STATUS;
                }



                if (updateUser.PROFILE_PICTURE != null)
                {
                    updateUser.PROFILE_IMG = new IDEAX_Function().imageToByte(updateUser.PROFILE_PICTURE);
                    user_details.PROFILE_PICTURE = updateUser.PROFILE_IMG;
                }




                db.SaveChanges();

                return RedirectToAction("Profile");

            }

            return View(updateUser);






        }




        public ActionResult ChangePassword()
        {
            return PartialView();
        }

    public ActionResult EditPostRequest()
        {
            return PartialView();
        }
    public ActionResult ReportPostRequest()
        {
            return PartialView();
        }

        public ActionResult RequestPassChange(string password, string newpassword)
        {

            var uname = Session["UNAME"].ToString();
            var db = new IDEA_XEntities();
            string errmsg = "";
            var user1 = db.ALL_USERS.Where(u => u.USERNAME == uname).SingleOrDefault();

            if (user1 != null)
            {
                //if (user1.PASSWORD == password)
                //{
                //    user1.PASSWORD = newpassword;
                //    db.SaveChanges();
                //    return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
                //}
                //TODO : changed here --adb
                string prev_e_pass = EncryptionAndHashLogic.HashPassword(password);
                string e_pass = EncryptionAndHashLogic.HashPassword(newpassword);
                if (user1.PASSWORD.Equals(prev_e_pass))
                {
                    user1.PASSWORD = e_pass;
                    db.SaveChanges();
                    return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
                }

            }
            errmsg += "Failed! We can't able to change your password. please make sure your old password is correct.";


            return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
        }

        [HttpGet]

        public ActionResult CreatePost()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreatePost(CreatePostVM createPost)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(createPost.TIMELINE_TEXT) || createPost.TIMELINE_IMG != null)
                {
                    var uname = Session["UNAME"].ToString();
                    var db = new IDEA_XEntities();
                    var new_post = new GENERAL_POSTS()
                    {
                        AUTHOR = uname,
                        TIMELINE_TEXT = createPost.TIMELINE_TEXT,
                        POST_LIKE = 0,
                        POST_DISLIKE = 0,
                        POST_IP = new IDEAX_Function().ip(),
                        POSTING_TIME = new IDEAX_Function().time(),
                        POSTING_STATUS = "ACTIVE"
                    };

                    if (createPost.POST_TAG != null)
                    {
                        new_post.POST_TAG = $"#{createPost.POST_TAG}";
                    }
                    if (createPost.POST_TAG == null)
                    {
                        new_post.POST_TAG = $"#idea";
                    }
                    if (createPost.TIMELINE_IMG != null)
                    {
                        new_post.TIMELINE_IMAGE = new IDEAX_Function().imageToByte(createPost.TIMELINE_IMG);
                    }
                    db.GENERAL_POSTS.Add(new_post);
                    db.SaveChanges();
                    return RedirectToAction("Personal_Timeline", "User");
                }
            }
            return RedirectToAction("Personal_Timeline", "User");
        }
        [HttpGet]
        public ActionResult EditPost(int id)
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var post = (from i in db.GENERAL_POSTS
                        where i.POST_ID == id && i.AUTHOR.Equals(uname)
                        select i).SingleOrDefault();
            if (post != null)
            {
                EditPostVM editreq = new EditPostVM();
                editreq.POST_IDR = id;
                editreq.POST_TAGR = post.POST_TAG;
                editreq.TIMELINE_TEXTR = post.TIMELINE_TEXT;
                editreq.TIMELINE_IMGRC = post.TIMELINE_IMAGE;
                return View(editreq);
            }
            return RedirectToAction("Personal_Timeline");
        }


        [HttpPost]
        public ActionResult EditPost(EditPostVM editreq)
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var post = (from i in db.GENERAL_POSTS
                        where i.POST_ID == editreq.POST_IDR && i.AUTHOR.Equals(uname)
                        select i).SingleOrDefault();


            if (editreq.POST_TAGR.IndexOf('#') == -1)
            {
                post.POST_TAG = $"#{editreq.POST_TAGR}";
            }
            else
            {
                post.POST_TAG = editreq.POST_TAGR;
            }

            post.TIMELINE_TEXT = editreq.TIMELINE_TEXTR;

            if (editreq.TIMELINE_IMGR != null)
            {
                post.TIMELINE_IMAGE = new IDEAX_Function().imageToByte(editreq.TIMELINE_IMGR);
            }
            db.SaveChanges();

            return RedirectToAction("Personal_Timeline");
        }




        [HttpGet]
        public ActionResult ReportPost(int id)
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var post = (from i in db.GENERAL_POSTS
                        where i.POST_ID == id
                        select i).SingleOrDefault();
            
            if (post != null)
            {
                var checkrep = (from i in db.POST_REPORT
                            where i.POST_ID==id && i.REPORTED_BY.Equals(uname)
                            select i).SingleOrDefault();
                if (checkrep == null)
                {
                    ReportPostVM repreq = new ReportPostVM();
                    repreq.POST_ID = id;
                    repreq.POST_AUTHOR = post.AUTHOR;
                    return View(repreq);
                }
                else { 
                 return RedirectToAction("Timeline");
                }

         
            }
            return RedirectToAction("Timeline");
            
        }


        [HttpPost]
        public ActionResult ReportPost(ReportPostVM repreq)
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();

            if (repreq.REPORT_DETAILS!=null || repreq.REPORT_CATEGORY!=null )
            {
                repreq.REPORT_TIME = new IDEAX_Function().time();
                repreq.REPORT_IP = new IDEAX_Function().ip();
                repreq.REPORT_STATUS = "PENDING";
                repreq.REPORTED_BY = uname;

                POST_REPORT newreport = new POST_REPORT();

                newreport.POST_ID = repreq.POST_ID;
                newreport.POST_AUTHOR = repreq.POST_AUTHOR;
                newreport.REPORT_CATEGORY = repreq.REPORT_CATEGORY;
                newreport.REPORT_DETAILS = repreq.REPORT_DETAILS;
                newreport.REPORT_TIME = repreq.REPORT_TIME;
                newreport.REPORT_IP = repreq.REPORT_IP;
                newreport.REPORT_STATUS = repreq.REPORT_STATUS;
                newreport.REPORTED_BY = repreq.REPORTED_BY;

                db.POST_REPORT.Add(newreport);
                db.SaveChanges();
            
            }

            return RedirectToAction("Timeline");
        }






        public ActionResult Activity()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var countLike = (from i in db.GENERAL_POSTS
                             where i.AUTHOR == uname
                             orderby i.POST_ID descending
                             select i.POST_LIKE
                       ).ToList();
            int like = 0;
            int dislike = 0;
            int[] arrayLike = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] arrayDislike = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (int a in countLike)
            {
                like += a;
            }
            int flagLike = 9;
 foreach (int a in countLike)
            {

                if (flagLike >= 0)
                {
                    arrayLike[flagLike] = a;
                    flagLike--;
                }
            }


            var countDisLike = (from i in db.GENERAL_POSTS
                                where i.AUTHOR == uname
                                orderby i.POST_ID descending
                                select i.POST_DISLIKE
                     ).ToList();

            foreach (int a in countDisLike)
            {
                dislike += a;
            }

            int flagDisLike = 9;
            foreach (int a in countDisLike)
            {
                if(flagDisLike>=0)
                { arrayDislike[flagDisLike] = a;
                flagDisLike--;}
                
            }

            UserActivityVM uactivity = new UserActivityVM();
            uactivity.TotalUpvote = like;
            uactivity.TotalDownvote = dislike;
            uactivity.PostLike1 = arrayLike[0];
            uactivity.PostLike2 = arrayLike[1];
            uactivity.PostLike3 = arrayLike[2];
            uactivity.PostLike4 = arrayLike[3];
            uactivity.PostLike5 = arrayLike[4];
            uactivity.PostLike6 = arrayLike[5];
            uactivity.PostLike7 = arrayLike[6];
            uactivity.PostLike8 = arrayLike[7];
            uactivity.PostLike9 = arrayLike[8];
            uactivity.PostLike10 = arrayLike[9];

            uactivity.PostDisLike1 = arrayDislike[0];
            uactivity.PostDisLike2 = arrayDislike[1];
            uactivity.PostDisLike3 = arrayDislike[2];
            uactivity.PostDisLike4 = arrayDislike[3];
            uactivity.PostDisLike5 = arrayDislike[4];
            uactivity.PostDisLike6 = arrayDislike[5];
            uactivity.PostDisLike7 = arrayDislike[6];
            uactivity.PostDisLike8 = arrayDislike[7];
            uactivity.PostDisLike9= arrayDislike[8];
            uactivity.PostDisLike10 = arrayDislike[9];
            return View(uactivity);
        }









        //Kaushik Start


        public ActionResult passMatchForLoginInfo()
        {
            return PartialView();
        }

        //public ActionResult checkPass(string password)
        //{
        //    var uname = Session["UNAME"].ToString();
        //    var db = new IDEA_XEntities();
        //    string errmsg = "";
        //    var user1 = db.ALL_USERS.Where(u => u.USERNAME == uname).SingleOrDefault();

        //    if (user1 != null)
        //    {
        //        if (user1.PASSWORD == password)
        //        {
        //            return RedirectToAction("logInfoView");

        //            //return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
        //        }

        //        else
        //        {
        //            return RedirectToAction("Activity");
        //        }

        //    }

        //    return RedirectToAction("Activity");

        //}



        public ActionResult checkPass(string password)
        {
            var uname = Session["UNAME"].ToString();
            var db = new IDEA_XEntities();
            string errmsg = "";
            var user1 = db.ALL_USERS.Where(u => u.USERNAME == uname).SingleOrDefault();

            if (user1 != null)
            {
                //TODO : changed here --adb
                string prev_e_pass = EncryptionAndHashLogic.HashPassword(password);
                
                if (user1.PASSWORD.Equals(prev_e_pass))
                {
                    db.SaveChanges();
                    return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
                }

            }
            errmsg += "Sorry your password is not matched. Try again.";


            return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
        }






        public ActionResult logInfoView()
        {
            var uname = Session["UNAME"].ToString();
            var db = new IDEA_XEntities();
            var users = db.LOGINS.Where(u => u.USERNAME == uname);

            return View(users);
        }

   public ActionResult DownloadUserLogin_User()
        {
            var uname = Session["UNAME"].ToString();
            var db = new IDEA_XEntities();
            var users = db.LOGINS.Where(u => u.USERNAME == uname);

            DataTable dt = new DataTable("Grid");



            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("UserName"),
                                                     new DataColumn("Email"),
                                                     new DataColumn("LoginTime"),
                                                     new DataColumn("LoginIP"),
                                                 });
            foreach (var item in users)
            {

                dt.Rows.Add(item.USERNAME, item.EMAIL, item.LOGIN_TIME, item.LOGIN_IP);

            }

            using (XLWorkbook wb = new XLWorkbook()) 
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) 
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname+"_Login_Details.xlsx");
                }
            }

        
    }


        public ActionResult postInfoView()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var general_posts = db.GENERAL_POSTS.Where(x => x.AUTHOR.Equals(uname));
            List<POST_ACTIONS> post_actions = new List<POST_ACTIONS>();

            foreach (var p in general_posts)
            {
                var post_get = (from i in db.POST_ACTIONS
                                where i.POST_ID == p.POST_ID
                                select i).ToList();




                if (post_get!=null)
                {
                    foreach(var p2 in post_get)
                    {
                        post_actions.Add(p2);
                    }
                }
                
            }


            var posts = (from pa in post_actions
                         join p in general_posts
                         on pa.POST_ID equals p.POST_ID into p_t
                         from post in p_t.DefaultIfEmpty()
                         orderby post.POST_ID descending
                         select new USER_POST_VIEW
                         {
                             POST_ID = post.POST_ID,
                             AUTHOR = post.AUTHOR,
                             TIMELINE_TEXT = post.TIMELINE_TEXT,
                             TIMELINE_IMAGE = post.TIMELINE_IMAGE,
                             POSTING_TIME = post.POSTING_TIME,
                             POST_STATUS = pa.POST_STATUS,
                             POST_LIKE = post.POST_LIKE,
                             POST_DISLIKE = post.POST_DISLIKE,
                             POST_TAG = post.POST_TAG,
                             USERNAME = pa.USERNAME,
                             POSTING_STATUS = post.POSTING_STATUS,
                         }).ToList();

            return View(posts);

        }



        public ActionResult DownloadUserPost_User()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var general_posts = db.GENERAL_POSTS.Where(x => x.AUTHOR.Equals(uname));
            List<POST_ACTIONS> post_actions = new List<POST_ACTIONS>();

            foreach (var p in general_posts)
            {
                var post_get = (from i in db.POST_ACTIONS
                                where i.POST_ID == p.POST_ID
                                select i).ToList();




                if (post_get != null)
                {
                    foreach (var p2 in post_get)
                    {
                        post_actions.Add(p2);
                    }
                }

            }


            var posts = (from pa in post_actions
                         join p in general_posts
                         on pa.POST_ID equals p.POST_ID into p_t
                         from post in p_t.DefaultIfEmpty()
                         orderby post.POST_ID descending
                         select new USER_POST_VIEW
                         {
                             POST_ID = post.POST_ID,
                             AUTHOR = post.AUTHOR,
                             TIMELINE_TEXT = post.TIMELINE_TEXT,
                             TIMELINE_IMAGE = post.TIMELINE_IMAGE,
                             POSTING_TIME = post.POSTING_TIME,
                             POST_STATUS = pa.POST_STATUS,
                             POST_LIKE = post.POST_LIKE,
                             POST_DISLIKE = post.POST_DISLIKE,
                             POST_TAG = post.POST_TAG,
                             USERNAME = pa.USERNAME,
                             POSTING_STATUS = post.POSTING_STATUS,
                         }).ToList();

            DataTable dt = new DataTable("Grid");



            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Author"),
                                                     new DataColumn("Timeline_Text"),
                                                     new DataColumn("Timeline_Image"),
                                                     new DataColumn("Posting_Time"),
                                                     new DataColumn("Post_Tag"),
                                                     new DataColumn("Post_Status"),
                                                     new DataColumn("Total_Upvote"),
                                                     new DataColumn("Total_Downvote"),
                                                     new DataColumn("Voted_By"),
                                                     new DataColumn("Vote"),
                                                    
                                                 });
            foreach (var item in posts)
            {
                var img = "";
                if (item.TIMELINE_IMAGE != null)
                {
                    img += "This post contain an image in IDEA-X";
                }
                else {
                    img += "No image for this post in IDEA-X";
                }
                dt.Rows.Add(item.AUTHOR, item.TIMELINE_TEXT,img, item.POSTING_TIME, item.POST_TAG, item.POSTING_STATUS, item.POST_LIKE, item.POST_DISLIKE, item.USERNAME, item.POST_STATUS);

            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_Post_Details.xlsx");
                }
            }


        }

    
      public ActionResult RevokeReport(int rid)
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var rep = (from pa in db.POST_REPORT.Where(x => x.REPORT_ID==rid)
                                 select pa).SingleOrDefault();
            db.POST_REPORT.Remove(rep);
            db.SaveChanges();

            return Json(new { msg = true, JsonRequestBehavior.AllowGet });
        }

        public ActionResult repInfoView()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var posts = (from pa in db.POST_REPORT.Where(x => x.REPORTED_BY.Equals(uname))
                                 orderby pa.REPORT_ID descending
                                 select pa).ToList();
            //List<POST_REPORT> post = new List<POST_REPORT>();

            //foreach (var p in general_posts)
            //{
            //    var post_get = (from i in db.POST_REPORT
            //                    select i).ToList();

            //    if (post_get != null)
            //    {
            //        foreach (var pp in post_get)
            //        {
            //            post.Add(pp);
            //        }
            //    }

            //}


           // var posts = (from pa in db.POST_REPORT.Where(x => x.REPORTED_BY.Equals(uname))orderby pa.POST_ID descending select pa).ToList();

            return View(posts);

        }

        public ActionResult DownloadUserReport_User()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var posts = (from pa in db.POST_REPORT.Where(x => x.REPORTED_BY.Equals(uname))
                         orderby pa.REPORT_ID descending
                         select pa).ToList();

            DataTable dt = new DataTable("Grid");



            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Post_ID"),
                                                     new DataColumn("Post_Author"),
                                                     new DataColumn("Report_Category"),
                                                     new DataColumn("Report_Details"),
                                                     new DataColumn("Report_Time"),
                                                     new DataColumn("Report_IP"),
                                                     new DataColumn("Report_Status"),
                                                 
                                                 });
            foreach (var item in posts)
            {
               
                dt.Rows.Add(item.POST_ID, item.POST_AUTHOR,item.REPORT_CATEGORY, item.REPORT_DETAILS, item.REPORT_TIME, item.REPORT_IP, item.REPORT_STATUS);

            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_Report_Details.xlsx");
                }
            }


        }


        public ActionResult RemoveMate(String mateName)
        {
            string sender = (string)Session["s_name"];
            string receiver = (string)Session["r_name"];
            var db = new IDEA_XEntities();
            var chat = db.CHAT_BOXS.Where(val => (val.SENDER.Equals(sender) && val.RECEIVER.Equals(receiver)) || (val.SENDER.Equals(receiver) && val.RECEIVER.Equals(sender))).SingleOrDefault();
          

            var chatlist = (from m in db.USER_MESSAGES
                            where m.SESSION_NAME == chat.CHAT_SESSION select m).ToList();
            if (chatlist != null)
            { 
             foreach (var x in chatlist)
            {
                db.USER_MESSAGES.Remove(x);
                
            }
            }
           

            db.CHAT_BOXS.Remove(chat);

            db.SaveChanges();
            return Json(new { isdel = true, JsonRequestBehavior.AllowGet });

           
        }
     public ActionResult RemoveChat(String mateName)
        {
            string sender = (string)Session["s_name"];
            string receiver = (string)Session["r_name"];
            var db = new IDEA_XEntities();
            var chat = db.CHAT_BOXS.Where(val => (val.SENDER.Equals(sender) && val.RECEIVER.Equals(receiver)) || (val.SENDER.Equals(receiver) && val.RECEIVER.Equals(sender))).SingleOrDefault();
          

            var chatlist = (from m in db.USER_MESSAGES
                            where m.SESSION_NAME == chat.CHAT_SESSION select m).ToList();
            if (chatlist != null)
            {
                foreach (var x in chatlist)
                {
                    db.USER_MESSAGES.Remove(x);

                }
            }
            //db.CHAT_BOXS.Remove(chat);

            db.SaveChanges();
            return Json(new { isdel = true, JsonRequestBehavior.AllowGet });

           
        }

[HttpGet]
        public ActionResult IdeaTracker()
        {
            String uname = (string)Session["UNAME"];

          String today= new IDEAX_Function().date_text();
         
            var db = new IDEA_XEntities();
            var note = (from pa in db.NOTES.Where(x => x.USERNAME.Equals(uname)
                        && x.NOTE_DATE.Equals(today))
                         select pa).SingleOrDefault();

            if (note != null)
            {

                return View(note);
            }


            else
            {
                Note_User n = new Note_User();
                n.NOTE_DATE = today;
                n.NOTE_TEXT = "";
                return View(n);

            }

        }

[HttpPost]
        public ActionResult IdeaTracker(Note_User n)
        {
            String uname = (string)Session["UNAME"];

        //  String today= new IDEAX_Function().date_text();

            var db = new IDEA_XEntities();
            var note = (from pa in db.NOTES.Where(x => x.USERNAME.Equals(uname)
                        && x.NOTE_DATE.Equals(n.NOTE_DATE))
                         select pa).SingleOrDefault();


            if (note != null)
            {
                note.NOTE_TEXT = n.NOTE_TEXT;
                note.NOTE_TIME = new IDEAX_Function().time();
                note.NOTE_IP = new IDEAX_Function().ip();
                db.SaveChanges();
            }
            else {
                NOTE newNote = new NOTE();

                newNote.USERNAME = uname;
                newNote.NOTE_DATE = n.NOTE_DATE;
                newNote.NOTE_TEXT = n.NOTE_TEXT;
                newNote.STATUS = "ACTIVE";
                newNote.NOTE_TIME =new IDEAX_Function().time();
                newNote.NOTE_IP =new IDEAX_Function().ip();
                db.NOTES.Add(newNote); 
                db.SaveChanges();
            }
            return View(n);

        }

        [HttpPost]
        public ActionResult SearchNote(String date)
        {
            String uname = (string)Session["UNAME"];

            String today = new IDEAX_Function().date_text();

            var db = new IDEA_XEntities();
            var note = (from pa in db.NOTES.Where(x => x.USERNAME.Equals(uname)
                        && x.NOTE_DATE.Equals(date))
                      select pa).SingleOrDefault();
          
            
                
                if (note != null)
            {
              var result = new { LIST = note }; 
                return new IDEAX_Function().SerializeJsonRequest(result);
            }


            else { 
                Note_User n = new Note_User();
                n.NOTE_DATE = date;
                n.NOTE_TEXT = "";
          var result = new { LIST = n }; 
 return new IDEAX_Function().SerializeJsonRequest(result);
         
            }


        }

        [HttpPost]
        // TODO : added get post count --adb
        public ActionResult GetPostCount()
        {
            var db = new IDEA_XEntities();
            var post_count = db.GENERAL_POSTS.ToList();
            var result = new { p_count = post_count.Count };
            return new IDEAX_Function().SerializeJsonRequest(result);
        }

        [HttpPost]
        // TODO : added get new post -- adb
        public ActionResult GetNewPost(string pr_id)
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var post_actions = db.POST_ACTIONS.Where(x => x.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase));
            var posts = (from p in db.GENERAL_POSTS.Where(x => x.POSTING_STATUS.Equals("ACTIVE"))
                         join p_a in post_actions
                         on p.POST_ID equals p_a.POST_ID into p_t
                         from post in p_t.DefaultIfEmpty()
                         select new POST_TIMELINE
                         {
                             POST_ID = p.POST_ID,
                             POST_TAG = p.POST_TAG,
                             TIMELINE_TEXT = p.TIMELINE_TEXT,
                             AUTHOR = p.AUTHOR,
                             POST_LIKE = p.POST_LIKE,
                             POST_DISLIKE = p.POST_DISLIKE,
                             POST_STATUS = post.POST_STATUS,
                             TIMELINE_IMAGE = p.TIMELINE_IMAGE,
                         }).ToList();

            var postWithImg = (from p in posts
                               join u_d in db.USER_DETAILS
                               on p.AUTHOR equals u_d.USERNAME into pi_t
                               from post in pi_t.DefaultIfEmpty()
                               select new POST_TIMELINE
                               {
                                   POST_ID = p.POST_ID,
                                   POST_TAG = p.POST_TAG,
                                   TIMELINE_TEXT = p.TIMELINE_TEXT,
                                   AUTHOR = p.AUTHOR,
                                   POST_LIKE = p.POST_LIKE,
                                   POST_DISLIKE = p.POST_DISLIKE,
                                   POST_STATUS = p.POST_STATUS,
                                   TIMELINE_IMAGE = p.TIMELINE_IMAGE,
                                   PROFILE_IMG = post.PROFILE_PICTURE,
                               }).ToList();

            foreach (var prop in postWithImg)
            {
                var postsRep = (from pa in db.POST_REPORT.Where(x => x.REPORTED_BY.Equals(uname) && x.POST_ID == prop.POST_ID)
                                select pa).SingleOrDefault();
                if (postsRep != null)
                {
                    prop.REPORTED_POST = "REPORTED";
                }
                else
                {
                    prop.REPORTED_POST = "CLEAR";
                }

            }
            var idx = postWithImg.FindIndex(e => e.POST_ID == Convert.ToInt32(pr_id));

            if (idx != 0)
            {
                var newlst = postWithImg.GetRange(idx - 1, 1).First();

                var result = new { obj = newlst };


                return new IDEAX_Function().SerializeJsonRequest(result);
            }
            return new EmptyResult();

        }
    }

}


