using ClosedXML.Excel;
using IDEA_X.Annotation;
using IDEA_X.HelperClasses;
using IDEA_X.IDEAX_Class;
using IDEA_X.Models;
using IDEA_X.Models.EntityFramework;
using IDEA_X.Models.ViewModels;
using IDEA_X.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace IDEA_X.Controllers
{
    [AdminAttr]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Timeline()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var post_actions = db.POST_ACTIONS.Where(x => x.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase));
            var posts = (from p in db.GENERAL_POSTS
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
            return View(postWithImg.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult BanPost(int id)
        {
            var db = new IDEA_XEntities();
            var post = db.GENERAL_POSTS.Where(val => val.POST_ID == id).SingleOrDefault();
            post.POSTING_STATUS = "BANNED";
            db.SaveChanges();
            // return RedirectToAction("Timeline");
            return Json(new { ischange = true, JsonRequestBehavior.AllowGet });

        }

   [HttpPost]
        public ActionResult UnBanPost(int id)
        {
            var db = new IDEA_XEntities();
            var post = db.GENERAL_POSTS.Where(val => val.POST_ID == id).SingleOrDefault();
            post.POSTING_STATUS = "ACTIVE";
            db.SaveChanges();
            // return RedirectToAction("Timeline");
            return Json(new { ischange = true, JsonRequestBehavior.AllowGet });

        }


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

                if (action != null)
                {

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

            return Json(new { upvoteCount = upvoteCount, downvoteCount = downvoteCount, JsonRequestBehavior.AllowGet });


        }



        [HttpPost]
        public ActionResult SearchPost(string searchText)
        {
            var db = new IDEA_XEntities();
            var post = db.GENERAL_POSTS.Where(val => val.POST_TAG.Trim().ToLower().Contains(searchText.ToLower()) || val.AUTHOR.Trim().ToLower().Contains(searchText.ToLower()) || val.POST_ID.ToString().Contains(searchText) ).ToList();
            //return Json(new { POST_LIST = post, JsonRequestBehavior.AllowGet });
            var result = new { POST_LIST = post };
           
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
                             POSTING_STATUS = p.POSTING_STATUS,
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
                                   POSTING_STATUS = p.POSTING_STATUS,
                                   PROFILE_IMG = post.PROFILE_PICTURE,
                               }).ToList();
            return View(postWithImg);
        }

        public ActionResult Profile()
        {
            var db = new IDEA_XEntities();
            var ad = db.ADMINS.ToList();

            return View(ad);

           // var db = new IDEA_XEntities();
           //// var user_detail = db.USER_DETAILS.Where(x => x.USERNAME.Contains(id.ToLower())).ToList();
           // var posts = (from p in db.USER_DETAILS
           //              where p.USERNAME == id
           //              select p).ToList();
           // return View(posts);
        }

        public ActionResult UserActivity(String id)
        {

            var db = new IDEA_XEntities();
            var countpost = (from i in db.POST_ACTIONS
                             where id.Equals(i.USERNAME)
                             select i).ToList();
            int countLike = 0;
            int countDislike = 0;
            foreach (var a in countpost)
            {
                if (a.POST_STATUS.Equals("UPVOTE"))
                {
                    countLike++;
                }
                if (a.POST_STATUS.Equals("DOWNVOTE"))
                {
                    countDislike++;
                }

            }

            int countLiketake = 0;
            int countDisliketake = 0;
            var counttakepost = (from i in db.GENERAL_POSTS
                             where i.AUTHOR.Equals(id)                          
                             select i).ToList();
            foreach (var a in counttakepost)
            {
                countLiketake += a.POST_LIKE;
                    countDisliketake += a.POST_DISLIKE;
            }





            ActivityActionAdmin apa = new ActivityActionAdmin();
            apa.USERNAME = id;
            apa.LIKE = countLike;
            apa.DISLIKE = countDislike;
   apa.LIKEtake = countLiketake;
            apa.DISLIKEtake = countDisliketake;


            return View(apa);
        }


        [HttpGet]
        public ActionResult SearchUserActivityProgress(String name)
        {


            var db = new IDEA_XEntities();
            var countpost = (from i in db.GENERAL_POSTS
                             where i.AUTHOR.Equals(name)
                             orderby i.POST_ID descending
                             select i).ToList();
            List<ActivityPostAdmin> postact = new List<ActivityPostAdmin>();
            foreach (var a in countpost)
            {
                ActivityPostAdmin postact1 = new ActivityPostAdmin();
                postact1.AUTHOR = a.AUTHOR;
                postact1.POST_ID = a.POST_ID;
                postact1.LIKE = a.POST_LIKE;
                postact1.DISLIKE = a.POST_DISLIKE;
                postact.Add(postact1);
            }
            /*
  var countUSER = (from i in db.USER_DETAILS
                             select i).ToList();
            List<ActivityActionAdmin> useract = new List<ActivityActionAdmin>();
            foreach (var a in countUSER)
            {
                ActivityActionAdmin useract1 = new ActivityActionAdmin();
                useract1.USERNAME = a.USERNAME;
                 var countAction = (from i in db.POST_ACTIONS
                                    where i.USERNAME.Equals(a.USERNAME)
                                    select i).ToList();
                int countLike = 0;
                int countDislike = 0;
                foreach (var b in countAction)
                {
                    if (b.POST_STATUS.Equals("UPVOTE"))
                    { countLike++; }
                    if (b.POST_STATUS.Equals("DOWNVOTE"))
                    { countDislike++; }
                }
                useract1.LIKE = countLike;
                useract1.DISLIKE = countDislike;
                useract.Add(useract1);
            }
     */

            var result = new { POSTLIST = postact/*, ACTIONLIST= useract*/ };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }


        [HttpPost]
        public ActionResult UserSearch(string SEARCH_MESSAGE)
        {
            var db = new IDEA_XEntities();

            var user_detail = db.USER_DETAILS.Where(x => x.USERNAME.Contains(SEARCH_MESSAGE)).ToList();

            return View(user_detail);

        }
        [HttpPost]
        public ActionResult SearchProfile(string searchText)
        {
            var db = new IDEA_XEntities();

            var ulist = (db.USER_DETAILS.Where(val => val.USERNAME.Trim().ToLower().Contains(searchText.ToLower()) || val.FIRST_NAME.Trim().ToLower().Contains(searchText.ToLower()) || val.LAST_NAME.Trim().ToLower().Contains(searchText.ToLower()) || (val.FIRST_NAME + " " + val.LAST_NAME).Trim().ToLower().Contains(searchText.ToLower())).ToList());

            return Json(new { USER_LIST = ulist, JsonRequestBehavior.AllowGet });
        }

        [HttpGet]
        public ActionResult UserProfile(string id)
        {
           
            var db = new IDEA_XEntities();
          
            var user_details = (from u_d in db.USER_DETAILS
                                where u_d.USERNAME.Equals(id, StringComparison.OrdinalIgnoreCase)
                                select u_d
                                ).SingleOrDefault();

            if (user_details != null)
            {
                return View(user_details);
            }
            else
            {
                return Redirect("/Admin/AdministrativeView/" + id+"?page="+Request["page"]);

            }
        }

        [HttpGet]
        public ActionResult AdministrativeView(string id)
        {
            var db = new IDEA_XEntities();

            var user = (from u in db.ALL_USERS
                                where u.USERNAME.Equals(id, StringComparison.OrdinalIgnoreCase)
                                select u
                                ).SingleOrDefault();


            return View(user);
        }


        

        [HttpPost]
        public ActionResult CheckBan(string uname)
        {
           
            var db = new IDEA_XEntities();

            var user_details = (from u_d in db.ALL_USERS
                                where u_d.USERNAME.Equals(uname)
                                select u_d
                                ).SingleOrDefault();
            if (user_details.LEVEL.Equals("BANNED"))
            {
                return Json(new { isban = true, JsonRequestBehavior.AllowGet });
            }
            
 return Json(new { isban = false, JsonRequestBehavior.AllowGet });
        }


     [HttpPost]
        public ActionResult BanUser(string uname)
        {
           
            var db = new IDEA_XEntities();

            var user_details = (from u_d in db.ALL_USERS
                                where u_d.USERNAME.Equals(uname)
                                select u_d
                                ).SingleOrDefault();
            user_details.LEVEL = "BANNED";
            db.SaveChanges();
            
 return Json(new { isban = true, JsonRequestBehavior.AllowGet });
        }

     [HttpPost]
        public ActionResult UnBanUser(string uname)
        {

            var db = new IDEA_XEntities();

            var user_details = (from u_d in db.ALL_USERS
                                where u_d.USERNAME.Equals(uname)
                                select u_d
                                ).SingleOrDefault();
            user_details.LEVEL = "USER";
            db.SaveChanges();

            return Json(new { isban = false, JsonRequestBehavior.AllowGet });
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

        public ActionResult Monitoring()
        {
            var db = new IDEA_XEntities();
            var countpost = (from i in db.GENERAL_POSTS
                             orderby i.POST_ID descending
                             select i).ToList();
            int countLike = 0;
            int countDislike = 0;
            foreach (var a in countpost)
            {
                countLike += a.POST_LIKE;
                countDislike += a.POST_DISLIKE;
            }
            ActivityPostAdmin apa = new ActivityPostAdmin();
            apa.LIKE = countLike;
            apa.DISLIKE = countDislike;



            return View(apa);
        }

        [HttpPost]
        public ActionResult SearchActivityProgress()
        {


            var db = new IDEA_XEntities();
            var countpost = (from i in db.GENERAL_POSTS
                             orderby i.POST_ID descending
                             select i).ToList();
            List<ActivityPostAdmin> postact = new List<ActivityPostAdmin>();
            foreach (var a in countpost)
            {
                ActivityPostAdmin postact1 = new ActivityPostAdmin();
                postact1.AUTHOR = a.AUTHOR;
                postact1.POST_ID = a.POST_ID;
                postact1.LIKE = a.POST_LIKE;
                postact1.DISLIKE = a.POST_DISLIKE;
                postact.Add(postact1);
            }
            /*
  var countUSER = (from i in db.USER_DETAILS
                             select i).ToList();
            List<ActivityActionAdmin> useract = new List<ActivityActionAdmin>();
            foreach (var a in countUSER)
            {
                ActivityActionAdmin useract1 = new ActivityActionAdmin();
                useract1.USERNAME = a.USERNAME;
                 var countAction = (from i in db.POST_ACTIONS
                                    where i.USERNAME.Equals(a.USERNAME)
                                    select i).ToList();
                int countLike = 0;
                int countDislike = 0;
                foreach (var b in countAction)
                {
                    if (b.POST_STATUS.Equals("UPVOTE"))
                    { countLike++; }
                    if (b.POST_STATUS.Equals("DOWNVOTE"))
                    { countDislike++; }
                }
                useract1.LIKE = countLike;
                useract1.DISLIKE = countDislike;
                useract.Add(useract1);
            }
     */

            var result = new { POSTLIST = postact/*, ACTIONLIST= useract*/ };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }

        [HttpPost]
        public ActionResult UserActivityProgress()
        {


            var db = new IDEA_XEntities();
            var countpost = (from i in db.GENERAL_POSTS
                             orderby i.POST_ID descending
                             select i).ToList();
            List<ActivityPostAdmin> postact = new List<ActivityPostAdmin>();
            foreach (var a in countpost)
            {
                ActivityPostAdmin postact1 = new ActivityPostAdmin();
                postact1.AUTHOR = a.AUTHOR;
                postact1.POST_ID = a.POST_ID;
                postact1.LIKE = a.POST_LIKE;
                postact1.DISLIKE = a.POST_DISLIKE;
                postact.Add(postact1);
            }
            /*
  var countUSER = (from i in db.USER_DETAILS
                             select i).ToList();
            List<ActivityActionAdmin> useract = new List<ActivityActionAdmin>();
            foreach (var a in countUSER)
            {
                ActivityActionAdmin useract1 = new ActivityActionAdmin();
                useract1.USERNAME = a.USERNAME;
                 var countAction = (from i in db.POST_ACTIONS
                                    where i.USERNAME.Equals(a.USERNAME)
                                    select i).ToList();
                int countLike = 0;
                int countDislike = 0;
                foreach (var b in countAction)
                {
                    if (b.POST_STATUS.Equals("UPVOTE"))
                    { countLike++; }
                    if (b.POST_STATUS.Equals("DOWNVOTE"))
                    { countDislike++; }
                }
                useract1.LIKE = countLike;
                useract1.DISLIKE = countDislike;
                useract.Add(useract1);
            }
     */

            var result = new { POSTLIST = postact/*, ACTIONLIST= useract*/ };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }
        public ActionResult UserLogIn()
        {
            var db = new IDEA_XEntities();
            var users =(from u_d in db.LOGINS
                      orderby u_d.LOGIN_ID descending
                         select u_d).ToList();

            return View(users);
        }

        [HttpPost]
        public ActionResult SearchUserLogIn(string text)
        {
           
            var db = new IDEA_XEntities();
            var users = (from u_d in db.LOGINS
                         where u_d.LOGIN_ID.ToString().Contains(text)
                         || u_d.USERNAME.Contains(text)
                         || u_d.EMAIL.Contains(text)
                         || u_d.LOGIN_TIME.Contains(text)
                         || u_d.LOGIN_IP.Contains(text)
                         orderby u_d.LOGIN_ID descending
                         select u_d).ToList();
            var result = new { LIST = users };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }

        public ActionResult DownloadUserLogIn_Admin()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var dulog = db.LOGINS.ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Login_ID"),
                                                     new DataColumn("Username"),
                                                     new DataColumn("Email"),
                                                     new DataColumn("Login_Time"),
                                                     new DataColumn("Login_IP"),

                                                 });
            foreach (var item in dulog)
            {
                dt.Rows.Add(item.LOGIN_ID, item.USERNAME, item.EMAIL, item.LOGIN_TIME, item.LOGIN_IP);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_LogIn_Details.xlsx");
                }
            }

        }



        public ActionResult UserList()
        {
            var db = new IDEA_XEntities();
          
            var users = (from u_d in db.ALL_USERS
                         orderby u_d.USERNAME ascending
                         select u_d).ToList();
            return View(users);
        }
        [HttpPost]
        public ActionResult SearchUserList(string text)
        {

            var db = new IDEA_XEntities();
            var users = (from u_d in db.ALL_USERS
                         where u_d.USERNAME.Contains(text)
                         || u_d.EMAIL.Contains(text)
                         || u_d.LEVEL.Contains(text)
                        orderby u_d.USERNAME ascending
                         select u_d).ToList();
            var result = new { LIST = users };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }

        public ActionResult DownloadUserList_Admin()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var ul = db.ALL_USERS.ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Username"),
                                                     new DataColumn("Email"),
                                                     new DataColumn("Level"),
                                                 });
            foreach (var item in ul)
            {
                dt.Rows.Add(item.USERNAME, item.EMAIL, item.LEVEL);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_Connected_Users_Details.xlsx");
                }
            }

        }







        public ActionResult UserPosts()
        {
            return View();
        }
        public ActionResult User_Affairs_Controller()
        {
            return View();
        }



        public ActionResult AllUserDetails()
        {
            var db = new IDEA_XEntities();
            var users = (from u_d in db.USER_DETAILS
                         orderby u_d.USERNAME ascending
                         select u_d).ToList();

            return View(users);
        }
        [HttpPost]
        public ActionResult SearchAllUserDetails(string text)
        {

            var db = new IDEA_XEntities();
            var users = (from u_d in db.USER_DETAILS
                         where u_d.USERNAME.Contains(text)
                         || (u_d.FIRST_NAME+" "+u_d.LAST_NAME).Contains(text)
                         || u_d.DATE_OF_BIRTH.Contains(text)
                         || u_d.GENDER.Contains(text)
                         || u_d.MOBILE.Contains(text)
                         || u_d.COUNTRY.Contains(text)
                         || u_d.INDUSTRY.Contains(text)
                         || u_d.EDUCATIONAL_INSTITUTION.Contains(text)
                         || u_d.DEPARTMENT.Contains(text)
                         || u_d.SIGNUP_TIME.Contains(text)
                          || u_d.USER_STATUS.Contains(text)
                           || u_d.SIGNUP_IP.Contains(text)
                         orderby u_d.USERNAME ascending
                         select u_d).ToList();
            var result = new { LIST = users };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }
        public ActionResult DownloadAllUserDetails_Admin()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var dalluser = db.USER_DETAILS.ToList();

            DataTable dt = new DataTable("Grid");



            dt.Columns.AddRange(new DataColumn[19] { new DataColumn("Username"),
                                                     new DataColumn("First_Name"),
                                                     new DataColumn("Last_Name"),
                                                     new DataColumn("Headline"),
                                                     new DataColumn("Date_Of_Birth"),
                                                     new DataColumn("Gender"),
                                                     new DataColumn("Mobile"),
                                                     new DataColumn("User_Address"),
                                                     new DataColumn("User_State"),
                                                     new DataColumn("Zip_Code"),
                                                     new DataColumn("Country"),
                                                     new DataColumn("Industry"),
                                                     new DataColumn("Educational_Institution"),
                                                     new DataColumn("Department"),
                                                     new DataColumn("Contact_URL"),
                                                     new DataColumn("Profile_Picture"),
                                                     new DataColumn("SignUp_Time"),
                                                     new DataColumn("User_Status"),
                                                     new DataColumn("SignUp_IP"),

                                                 });
            foreach (var item in dalluser)
            {
                var img = "";
                if (item.PROFILE_PICTURE != null)
                {
                    img += "This post contain an image in IDEA-X";
                }
                else
                {
                    img += "No image for this post in IDEA-X";
                }
                dt.Rows.Add(item.USERNAME, item.FIRST_NAME, item.LAST_NAME, item.HEADLINE, item.DATE_OF_BIRTH, item.GENDER, item.MOBILE, item.USER_ADDRESS, item.USER_STATE, item.ZIP_CODE, item.COUNTRY, item.INDUSTRY, item.EDUCATIONAL_INSTITUTION, item.DEPARTMENT, item.CONTACT_URL, img, item.SIGNUP_TIME, item.USER_STATUS, item.SIGNUP_IP);

            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_All_User_Details.xlsx");
                }
            }

        }




        public ActionResult MessageRequests()
        {
            var db = new IDEA_XEntities();
        
            var msgreq = (from u_d in db.MESSAGE_REQUESTS
                          orderby u_d.REQUEST_ID descending
                         select u_d).ToList();
            return View(msgreq);
        }
        [HttpPost]
        public ActionResult SearchMessageRequests(string text)
        {

            var db = new IDEA_XEntities();
            var users = (from u_d in db.MESSAGE_REQUESTS
                         where u_d.REQUEST_ID.ToString().Contains(text)
                         || u_d.SENDER.Contains(text)
                         || u_d.RECEIVER.Contains(text)
                         || u_d.MESSAGE_TIME.Contains(text)
                        orderby u_d.REQUEST_ID descending
                         select u_d).ToList();
            var result = new { LIST = users };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }

        public ActionResult DownloadMessageRequests_Admin()
        {
            var uname = (string)Session["UNAME"];

            var db = new IDEA_XEntities();
            var req = db.MESSAGE_REQUESTS.ToList();
            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Request_ID"),
                                                     new DataColumn("Sender"),
                                                     new DataColumn("Receiver"),
                                                     new DataColumn("Message_Time"),

                                                 });
            foreach (var item in req)
            {
                dt.Rows.Add(item.REQUEST_ID, item.SENDER, item.RECEIVER, item.MESSAGE_TIME);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_Message_Request_Details.xlsx");
                }
            }

        }

        public ActionResult ChatSessions()
        {
            var db = new IDEA_XEntities();
          
            var chat = (from u_d in db.CHAT_BOXS
                         orderby u_d.CHAT_SESSION ascending
                         select u_d).ToList();
            return View(chat);
        }
        [HttpPost]
        public ActionResult SearchChatSessions(string text)
        {

            var db = new IDEA_XEntities();
            var chats = (from u_d in db.CHAT_BOXS
                         where u_d.CHAT_SESSION.Contains(text)
                         || u_d.SENDER.Contains(text)
                         || u_d.RECEIVER.Contains(text)
                         || u_d.CHAT_TIME.Contains(text)
                         orderby u_d.CHAT_SESSION ascending
                         select u_d).ToList();
            var result = new { LIST = chats };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }

        public ActionResult DownloadChatSession_Admin()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var cs = db.CHAT_BOXS.ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Chat_Session"),
                                                     new DataColumn("Sender"),
                                                     new DataColumn("Receiver"),
                                                     new DataColumn("Chat_Time"),
                                                     
                                                 });
            foreach (var item in cs)
            {            
                dt.Rows.Add(item.CHAT_SESSION, item.SENDER, item.RECEIVER, item.CHAT_TIME);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_Chat_Session_Details.xlsx");
                }
            }
        }


        public ActionResult PostActions()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            List<POST_ACTIONS> post_actions = new List<POST_ACTIONS>();

            foreach (var p in db.GENERAL_POSTS)
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
                         join p in db.GENERAL_POSTS
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


        [HttpPost]
        public ActionResult SearchPostActions(string text)
        {


            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            List<POST_ACTIONS> post_actions = new List<POST_ACTIONS>();

            foreach (var p in db.GENERAL_POSTS)
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
                         join p in db.GENERAL_POSTS
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

            
            var posts_t = (from u_d in posts
                         where u_d.POST_ID.ToString().Contains(text)
                         || u_d.USERNAME.Contains(text)
                         || u_d.AUTHOR.Contains(text)
                         || u_d.TIMELINE_TEXT.Contains(text)
                         || u_d.POSTING_TIME.Contains(text)
                          || u_d.POST_TAG.Contains(text)
                           || u_d.POSTING_STATUS.Contains(text)
                            || u_d.POST_STATUS.Contains(text)                      
                         select u_d).ToList();
            var result = new { LIST = posts_t };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }



        public ActionResult DownloadPostActions_Admin()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            List<POST_ACTIONS> post_actions = new List<POST_ACTIONS>();

            foreach (var p in db.GENERAL_POSTS)
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
                         join p in db.GENERAL_POSTS
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



            dt.Columns.AddRange(new DataColumn[11] { new DataColumn("Post_ID"),
                                                     new DataColumn("Author"),
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
                else
                {
                    img += "No image for this post in IDEA-X";
                }
                dt.Rows.Add(item.POST_ID,item.AUTHOR, item.TIMELINE_TEXT, img, item.POSTING_TIME, item.POST_TAG, item.POSTING_STATUS, item.POST_LIKE, item.POST_DISLIKE, item.USERNAME, item.POST_STATUS);

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








        public ActionResult PostArchives()
        {
            var db = new IDEA_XEntities();
          
            var parchive = (from u_d in db.GENERAL_POSTS
                         orderby u_d.POST_ID descending
                         select u_d).ToList();
            return View(parchive);
        }

        [HttpPost]
        public ActionResult SearchPostArchives(string text)
        {

            var db = new IDEA_XEntities();
            var posts = (from u_d in db.GENERAL_POSTS
                         where u_d.POST_ID.ToString().Contains(text)                       
                         || u_d.AUTHOR.Contains(text)
                         || u_d.TIMELINE_TEXT.Contains(text)
                         || u_d.POSTING_TIME.Contains(text)
                          || u_d.POST_TAG.Contains(text)
                           || u_d.POSTING_STATUS.Contains(text)
                            || u_d.POST_IP.Contains(text)
                         select u_d).ToList();
            var result = new { LIST = posts };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }

        public ActionResult DownloadPostArchives_Admin()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var dparch = db.GENERAL_POSTS.ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Post_ID"),
                                                     new DataColumn("Author"),
                                                     new DataColumn("Timeline_Text"),
                                                     new DataColumn("Timeline_Image"),
                                                     new DataColumn("Posting_Time"),
                                                     new DataColumn("Posting_Status"),
                                                     new DataColumn("Post_Like"),
                                                     new DataColumn("Post_Dislike"),
                                                     new DataColumn("Post_IP"),
                                                     new DataColumn("Post_Tag"),

                                                 });
            foreach (var item in dparch)
            {
                var img = "";
                if (item.TIMELINE_IMAGE != null)
                {
                    img += "This post contain an image in IDEA-X";
                }
                else
                {
                    img += "No image for this post in IDEA-X";
                }
                dt.Rows.Add(item.POST_ID, item.AUTHOR, item.TIMELINE_TEXT, img, item.POSTING_TIME, item.POSTING_STATUS, item.POST_LIKE, item.POST_DISLIKE, item.POST_IP, item.POST_TAG);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_General_Post_Details.xlsx");
                }
            }

        }





        public ActionResult GetPostImageAdmin(int id)
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

        public ActionResult GetUserImageAdmin(string user)
        {
            var db = new IDEA_XEntities();
            var img = (from i in db.USER_DETAILS
                       where i.USERNAME == user
                       select i.PROFILE_PICTURE
                       ).SingleOrDefault();
            if (img != null)
            {
                return File(img, "image/jpg");
            }
            return new EmptyResult();
        }


        public ActionResult ChangePassword()
        {
            return PartialView();
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

        public ActionResult RequestPassChange(string password, string newpassword)
        {

            var uname = Session["UNAME"].ToString();
            var db = new IDEA_XEntities();
            string errmsg = "";
            var user1 = db.ALL_USERS.Where(u => u.USERNAME == uname).SingleOrDefault();

            if (user1 != null)
            {
                //TODO : changed here --adb
                string prev_e_pass = EncryptionAndHashLogic.HashPassword(password);
                string e_pass = EncryptionAndHashLogic.HashPassword(newpassword);
                if (user1.PASSWORD.Equals(prev_e_pass))
                {

                    var user2 = db.ADMINS.Where(u => u.USERNAME == uname).SingleOrDefault();
                    user2.USER_PASSWORD = e_pass;
                    user1.PASSWORD = e_pass;
                    db.SaveChanges();
                    return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
                }

            }
            errmsg += "Failed! We can't able to change your password. please make sure your old password is correct.";


            return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
        }


        /*

           public ActionResult UserLogIn()
                {
                    var db = new IDEA_XEntities();
                    var users =(from u_d in db.LOGINS
                              orderby u_d.LOGIN_ID descending
                                 select u_d).ToList();

                    return View(users);
                }

                [HttpPost]
                public ActionResult SearchUserLogIn(string text)
                {

                    var db = new IDEA_XEntities();
                    var users = (from u_d in db.LOGINS
                                 where u_d.LOGIN_ID.ToString().Contains(text)
                                 || u_d.USERNAME.Contains(text)
                                 || u_d.EMAIL.Contains(text)
                                 || u_d.LOGIN_TIME.Contains(text)
                                 || u_d.LOGIN_IP.Contains(text)
                                 orderby u_d.LOGIN_ID descending
                                 select u_d).ToList();
                    var result = new { LIST = users };
                    return new IDEAX_Function().SerializeJsonRequest(result);

                }

                public ActionResult DownloadUserLogIn_Admin()
                {
                    var uname = (string)Session["UNAME"];
                    var db = new IDEA_XEntities();
                    var dulog = db.LOGINS.ToList();

                    DataTable dt = new DataTable("Grid");

                    dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Login_ID"),
                                                             new DataColumn("Username"),
                                                             new DataColumn("Email"),
                                                             new DataColumn("Login_Time"),
                                                             new DataColumn("Login_IP"),

                                                         });
                    foreach (var item in dulog)
                    {
                        dt.Rows.Add(item.LOGIN_ID, item.USERNAME, item.EMAIL, item.LOGIN_TIME, item.LOGIN_IP);
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);

                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_LogIn_Details.xlsx");
                        }
                    }

                }


         */

        public ActionResult PendingReport()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                         where u_d.REPORT_STATUS.Equals("PENDING")
                         orderby u_d.REPORT_ID ascending
                         select u_d).ToList();

            return View(reports);
        }
        [HttpPost]
       public ActionResult SearchPendingReport(String text)
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                         where u_d.REPORT_STATUS.Equals("PENDING") &&
                           (
                           u_d.REPORT_ID.ToString().Contains(text) ||
                           u_d.POST_ID.ToString().Contains(text) ||
                           u_d.POST_AUTHOR.Contains(text) ||
                           u_d.REPORTED_BY.Contains(text) ||
                            u_d.REPORT_CATEGORY.Contains(text) ||
                           u_d.REPORT_TIME.Contains(text) ||
                           u_d.REPORT_IP.Contains(text) ||
                           u_d.REPORT_STATUS.Contains(text)
                           )
                           orderby u_d.REPORT_ID ascending
                         select u_d).ToList();
            var result = new { LIST = reports };
            return new IDEAX_Function().SerializeJsonRequest(result);
        }


        public ActionResult DownloadPendingReports_Admin()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                           where u_d.REPORT_STATUS.Equals("PENDING")
                           orderby u_d.REPORT_ID ascending
                           select u_d).ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("Report_ID"),
                                                     new DataColumn("Post_ID"),
                                                     new DataColumn("Post_Author"),
                                                     new DataColumn("Reported_By"),
                                                     new DataColumn("Report_Category"),
                                                     new DataColumn("Report_Details"),
                                                     new DataColumn("Report_Time"),
                                                     new DataColumn("Report_IP"),
                                                     new DataColumn("Report_Status"),

                                                 });
            foreach (var item in reports)
            {
                dt.Rows.Add(item.REPORT_ID,
                    item.POST_ID,
                    item.POST_AUTHOR,
                    item.REPORTED_BY,
                    item.REPORT_CATEGORY,
                    item.REPORT_DETAILS,
                    item.REPORT_TIME,
                    item.REPORT_IP,
                    item.REPORT_STATUS
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","IDEA-X_Users_PendingReports.xlsx");
                }
            }

        }
        public ActionResult ReportInvestigate(int rid)
        {
          
            var db = new IDEA_XEntities();
            var rep = (from pa in db.POST_REPORT.Where(x => x.REPORT_ID == rid)
                       select pa).SingleOrDefault();
            rep.REPORT_STATUS = "INVESTIGATING";
            db.SaveChanges();

            return Json(new { msg = true, JsonRequestBehavior.AllowGet });
        }







        public ActionResult InvestigatingReport()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                           where u_d.REPORT_STATUS.Equals("INVESTIGATING")
                           orderby u_d.REPORT_ID ascending
                           select u_d).ToList();

            return View(reports);
        }

        [HttpPost]
        public ActionResult SearchInvestigatingReport(String text)
        {

            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                           where u_d.REPORT_STATUS.Equals("INVESTIGATING") &&
                             (
                             u_d.REPORT_ID.ToString().Contains(text) ||
                             u_d.POST_ID.ToString().Contains(text) ||
                             u_d.POST_AUTHOR.Contains(text) ||
                             u_d.REPORTED_BY.Contains(text) ||
                              u_d.REPORT_CATEGORY.Contains(text) ||
                             u_d.REPORT_TIME.Contains(text) ||
                             u_d.REPORT_IP.Contains(text) ||
                             u_d.REPORT_STATUS.Contains(text)
                             )
                           orderby u_d.REPORT_ID ascending
                           select u_d).ToList();
            var result = new { LIST = reports };
            return new IDEAX_Function().SerializeJsonRequest(result);
        }


        public ActionResult DownloadInvestigatingReports_Admin()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                           where u_d.REPORT_STATUS.Equals("INVESTIGATING")
                           orderby u_d.REPORT_ID ascending
                           select u_d).ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("Report_ID"),
                                                     new DataColumn("Post_ID"),
                                                     new DataColumn("Post_Author"),
                                                     new DataColumn("Reported_By"),
                                                     new DataColumn("Report_Category"),
                                                     new DataColumn("Report_Details"),
                                                     new DataColumn("Report_Time"),
                                                     new DataColumn("Report_IP"),
                                                     new DataColumn("Report_Status"),

                                                 });
            foreach (var item in reports)
            {
                dt.Rows.Add(item.REPORT_ID,
                    item.POST_ID,
                    item.POST_AUTHOR,
                    item.REPORTED_BY,
                    item.REPORT_CATEGORY,
                    item.REPORT_DETAILS,
                    item.REPORT_TIME,
                    item.REPORT_IP,
                    item.REPORT_STATUS
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "IDEA-X_Users_InvestigatingReports.xlsx");
                }
            }

        }
        public ActionResult ClosedReportClick(int rid)
        {
           
            var db = new IDEA_XEntities();
            var rep = (from pa in db.POST_REPORT.Where(x => x.REPORT_ID == rid)
                       select pa).SingleOrDefault();
            rep.REPORT_STATUS = "CLOSED";
           
            db.SaveChanges();

            return Json(new { msg = true, JsonRequestBehavior.AllowGet });
        }



        [HttpPost]
        public ActionResult UserNameValidation(string uname_text)
        {
            var db = new IDEA_XEntities();
            var user = (from u in db.ALL_USERS
                        where u.USERNAME == uname_text
                        select u).SingleOrDefault();
            if (user != null)
            {

                return Json(new { msg = "A user with this username already exist", JsonRequestBehavior.AllowGet });
            }


            return Json(new { msg = "", JsonRequestBehavior.AllowGet });
        }


        [HttpPost]
        public ActionResult EmailValidation(string mail_text)
        {
            //string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            string pattern = @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (regex.IsMatch(mail_text.Trim()))
            {
                var db = new IDEA_XEntities();
                var user = (from u in db.ALL_USERS
                            where u.EMAIL == mail_text
                            select u).SingleOrDefault();
                if (user != null)
                {

                    return Json(new { msg = "A user with this email already exist", JsonRequestBehavior.AllowGet });
                }

                else
                {
                    return Json(new { msg = "", JsonRequestBehavior.AllowGet });
                }

            }
            else
            {
                return Json(new { msg = "This is not a valid Email", JsonRequestBehavior.AllowGet });
            }


        }
        public ActionResult ViewUserController()
        {
            var db = new IDEA_XEntities();
            var users = (from u_d in db.USER_ACCESS_CONTROLLER
                         orderby u_d.USERNAME descending
                         select u_d).ToList();

            return View(users);
        }


        [HttpPost]
        public ActionResult SearchUserController(string text)
        {

            var db = new IDEA_XEntities();
            var users = (from u_d in db.USER_ACCESS_CONTROLLER
                         where  u_d.USERNAME.Contains(text)
                         || u_d.EMAIL.Contains(text)
                        
                         orderby u_d.USERNAME descending
                         select u_d).ToList();
            var result = new { LIST = users };
            return new IDEAX_Function().SerializeJsonRequest(result);

        }
        public ActionResult DownloadUserController_Admin()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var dulog = db.USER_ACCESS_CONTROLLER.ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[2] {
                                                     new DataColumn("Username"),
                                                     new DataColumn("Email")
                                                    

                                                 });
            foreach (var item in dulog)
            {
                dt.Rows.Add(item.USERNAME, item.EMAIL);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uname + "_UserController_Details.xlsx");
                }
            }

        }

        [HttpGet]
        public ActionResult AddUserController()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUserController(UAC_AddVM user)
        {
            var db = new IDEA_XEntities();

            if (ModelState.IsValid)
            {
                ALL_USERS allUsers = new ALL_USERS()
                {
                    EMAIL = user.EMAIL,
                    PASSWORD = user.PASSWORD,
                    LEVEL = "UAC",
                    USERNAME = user.USERNAME
                };
                //TODO : changed here --adb
                allUsers.PASSWORD = EncryptionAndHashLogic.HashPassword(user.PASSWORD);
                db.ALL_USERS.Add(allUsers);

                USER_ACCESS_CONTROLLER Users = new USER_ACCESS_CONTROLLER()
                {
                    EMAIL = user.EMAIL,
                    USER_PASSWORD = user.PASSWORD,
                    USERNAME = user.USERNAME
                };
                //TODO : changed here --adb
                Users.USER_PASSWORD = EncryptionAndHashLogic.HashPassword(Users.USER_PASSWORD);
                db.USER_ACCESS_CONTROLLER.Add(Users);
                db.SaveChanges();
                return RedirectToAction("ViewUserController");
            }
            return View(user);
        }
        public ActionResult ClosedReport()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                           where u_d.REPORT_STATUS.Equals("CLOSED")
                           orderby u_d.REPORT_ID descending
                           select u_d).ToList();

            return View(reports);
        }

        [HttpPost]
        public ActionResult SearchClosedReport(String text)
        {

            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                           where u_d.REPORT_STATUS.Equals("CLOSED") &&
                             (
                             u_d.REPORT_ID.ToString().Contains(text) ||
                             u_d.POST_ID.ToString().Contains(text) ||
                             u_d.POST_AUTHOR.Contains(text) ||
                             u_d.REPORTED_BY.Contains(text) ||
                              u_d.REPORT_CATEGORY.Contains(text) ||
                             u_d.REPORT_TIME.Contains(text) ||
                             u_d.REPORT_IP.Contains(text) ||
                             u_d.REPORT_STATUS.Contains(text)
                             )
                           orderby u_d.REPORT_ID ascending
                           select u_d).ToList();
            var result = new { LIST = reports };
            return new IDEAX_Function().SerializeJsonRequest(result);
        }

        public ActionResult DownloadClosedReports_Admin()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.POST_REPORT
                           where u_d.REPORT_STATUS.Equals("CLOSED")
                           orderby u_d.REPORT_ID ascending
                           select u_d).ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("Report_ID"),
                                                     new DataColumn("Post_ID"),
                                                     new DataColumn("Post_Author"),
                                                     new DataColumn("Reported_By"),
                                                     new DataColumn("Report_Category"),
                                                     new DataColumn("Report_Details"),
                                                     new DataColumn("Report_Time"),
                                                     new DataColumn("Report_IP"),
                                                     new DataColumn("Report_Status"),

                                                 });
            foreach (var item in reports)
            {
                dt.Rows.Add(item.REPORT_ID,
                    item.POST_ID,
                    item.POST_AUTHOR,
                    item.REPORTED_BY,
                    item.REPORT_CATEGORY,
                    item.REPORT_DETAILS,
                    item.REPORT_TIME,
                    item.REPORT_IP,
                    item.REPORT_STATUS
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "IDEA-X_Users_ClosedReports.xlsx");
                }
            }

        }

        public ActionResult PendingMessage()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.CONTACTS
                           where u_d.STATUS.Equals("PENDING")
                           orderby u_d.CONTACT_ID ascending
                           select u_d).ToList();

            return View(reports);
        }

        [HttpPost]
        public ActionResult SearchPendingMessage(String text)
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.CONTACTS
                           where u_d.STATUS.Equals("PENDING") &&
                             (
                             u_d.CONTACT_ID.ToString().Contains(text) ||
                             u_d.FIRST_NAME.ToString().Contains(text) ||
                             u_d.LAST_NAME.Contains(text) ||
                             u_d.USERNAME.Contains(text) ||
                              u_d.EMAIL.Contains(text) ||
                             u_d.MESSAGE.Contains(text) ||
                             u_d.LEVEL.Contains(text) ||
                             u_d.STATUS.Contains(text)||
                             u_d.LOGIN_TIME.Contains(text)||
                             u_d.LOGIN_IP.Contains(text)
                             )
                           orderby u_d.CONTACT_ID ascending
                           select u_d).ToList();
            var result = new { LIST = reports };
            return new IDEAX_Function().SerializeJsonRequest(result);
        }


        public ActionResult DownloadPendingMessages_Admin()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.CONTACTS
                           where u_d.STATUS.Equals("PENDING")
                           orderby u_d.CONTACT_ID ascending
                           select u_d).ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("CONTACT_ID"),
                                                     new DataColumn("FIRST_NAME"),
                                                     new DataColumn("LAST_NAME"),
                                                     new DataColumn("USERNAME"),
                                                     new DataColumn("EMAIL"),
                                                     new DataColumn("MESSAGE"),
                                                     new DataColumn("LEVEL"),
                                                     new DataColumn("STATUS"),
                                                     new DataColumn("LOGIN_TIME"),
                                                     new DataColumn("LOGIN_IP")

                                                 });
            foreach (var item in reports)
            {
                dt.Rows.Add(
                    item.CONTACT_ID,
                    item.FIRST_NAME,
                    item.LAST_NAME,
                    item.USERNAME,
                    item.EMAIL,
                    item.MESSAGE,
                    item.LEVEL,
                    item.STATUS,
                    item.LOGIN_TIME,
                    item.LOGIN_IP
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "IDEA-X_Users_PendingMessages.xlsx");
                }
            }

        }

        public ActionResult MessageSolve(int rid)
        {

            var db = new IDEA_XEntities();
            var rep = (from pa in db.CONTACTS.Where(x => x.CONTACT_ID == rid)
                       select pa).SingleOrDefault();
            rep.STATUS = "SOLVED";
            db.SaveChanges();

            return Json(new { msg = true, JsonRequestBehavior.AllowGet });
        }
        public ActionResult SolvedMessage()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.CONTACTS
                           where u_d.STATUS.Equals("SOLVED")
                           orderby u_d.CONTACT_ID descending
                           select u_d).ToList();

            return View(reports);
        }

        [HttpPost]
        public ActionResult SearchSolvedMessage(String text)
        {

            var db = new IDEA_XEntities();
            var reports = (from u_d in db.CONTACTS
                           where u_d.STATUS.Equals("SOLVED") &&
                             (
                           u_d.CONTACT_ID.ToString().Contains(text) ||
                             u_d.FIRST_NAME.ToString().Contains(text) ||
                             u_d.LAST_NAME.Contains(text) ||
                             u_d.USERNAME.Contains(text) ||
                              u_d.EMAIL.Contains(text) ||
                             u_d.MESSAGE.Contains(text) ||
                             u_d.LEVEL.Contains(text) ||
                             u_d.STATUS.Contains(text) ||
                             u_d.LOGIN_TIME.Contains(text) ||
                             u_d.LOGIN_IP.Contains(text)
                             )
                           orderby u_d.CONTACT_ID ascending
                           select u_d).ToList();
            var result = new { LIST = reports };
            return new IDEAX_Function().SerializeJsonRequest(result);
        }

        public ActionResult DownloadSolvedMessages_Admin()
        {
            var db = new IDEA_XEntities();
            var reports = (from u_d in db.CONTACTS
                           where u_d.STATUS.Equals("SOLVED")
                           orderby u_d.CONTACT_ID ascending
                           select u_d).ToList();

            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("CONTACT_ID"),
                                                     new DataColumn("FIRST_NAME"),
                                                     new DataColumn("LAST_NAME"),
                                                     new DataColumn("USERNAME"),
                                                     new DataColumn("EMAIL"),
                                                     new DataColumn("MESSAGE"),
                                                     new DataColumn("LEVEL"),
                                                     new DataColumn("STATUS"),
                                                     new DataColumn("LOGIN_TIME"),
                                                     new DataColumn("LOGIN_IP")

                                                 });
            foreach (var item in reports)
            {
                dt.Rows.Add(
                    item.CONTACT_ID,
                    item.FIRST_NAME,
                    item.LAST_NAME,
                    item.USERNAME,
                    item.EMAIL,
                    item.MESSAGE,
                    item.LEVEL,
                    item.STATUS,
                    item.LOGIN_TIME,
                    item.LOGIN_IP
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "IDEA-X_Users_SolvedMessages.xlsx");
                }
            }

        }
        public ActionResult MessageDelete(int rid)
        {

            var db = new IDEA_XEntities();
            var rep = (from pa in db.CONTACTS.Where(x => x.CONTACT_ID == rid)
                       select pa).SingleOrDefault();
            db.CONTACTS.Remove(rep);
            db.SaveChanges();

            return Json(new { msg = true, JsonRequestBehavior.AllowGet });
        }

        public ActionResult RevokeReport(int rid)
        {
            
            var db = new IDEA_XEntities();
            var rep = (from pa in db.POST_REPORT.Where(x => x.REPORT_ID == rid)
                       select pa).SingleOrDefault();
            db.POST_REPORT.Remove(rep);
            db.SaveChanges();

            return Json(new { msg = true, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        // TODO : added get new post -- adb
        public ActionResult GetNewPost(string pr_id)
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();
            var post_actions = db.POST_ACTIONS.Where(x => x.USERNAME.Equals(uname, StringComparison.OrdinalIgnoreCase));
            var posts = (from p in db.GENERAL_POSTS
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

       


            var idx = postWithImg.FindIndex(e => e.POST_ID == Convert.ToInt32(pr_id));

            if (idx != 0)
            {
                var newlst = postWithImg.GetRange(idx - 1, 1).First();

                var result = new { obj = newlst };


                return new IDEAX_Function().SerializeJsonRequest(result);
            }
            return new EmptyResult();

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
    }
}