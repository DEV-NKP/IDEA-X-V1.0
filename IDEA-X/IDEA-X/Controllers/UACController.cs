using ClosedXML.Excel;
using IDEA_X.Annotation;
using IDEA_X.HelperClasses;
using IDEA_X.IDEAX_Class;
using IDEA_X.Models;
using IDEA_X.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDEA_X.Controllers
{
    [UACAttr]
    public class UACController : Controller
    {
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

        public ActionResult Profile()
        {
            var uname = (string)Session["UNAME"];
            var db = new IDEA_XEntities();

            var user_details = (from u_d in db.USER_ACCESS_CONTROLLER
                                where u_d.USERNAME.Equals(uname)
                                select u_d
                                ).SingleOrDefault();

            return View(user_details);


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

           
                return View(user_details);
          
        }



        [HttpPost]
        public ActionResult SearchPost(string searchText)
        {
            var db = new IDEA_XEntities();
            var post = db.GENERAL_POSTS.Where(val => val.POST_TAG.Trim().ToLower().Contains(searchText.ToLower()) || val.AUTHOR.Trim().ToLower().Contains(searchText.ToLower()) || val.POST_ID.ToString().Contains(searchText)).ToList();
            //return Json(new { POST_LIST = post, JsonRequestBehavior.AllowGet });
            var result = new { POST_LIST = post };

            return new IDEAX_Function().SerializeJsonRequest(result);
        }

        public ActionResult SearchPostViewUAC(int id)
        {
           
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

        public ActionResult RequestPassChange(string password, string newpassword)
        {

            var uname = Session["UNAME"].ToString();
            var db = new IDEA_XEntities();
            string errmsg = "";
            var user1 = db.ALL_USERS.Where(u => u.USERNAME == uname).SingleOrDefault();

            if (user1 != null)
            {   //TODO : changed here --adb
                string prev_e_pass = EncryptionAndHashLogic.HashPassword(password);
                string e_pass = EncryptionAndHashLogic.HashPassword(newpassword);
                if (user1.PASSWORD.Equals(prev_e_pass))
                {

                    var user2 = db.USER_ACCESS_CONTROLLER.Where(u => u.USERNAME == uname).SingleOrDefault();
                    user2.USER_PASSWORD = e_pass;
                    user1.PASSWORD = e_pass;
                    db.SaveChanges();
                    return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
                }

            }
            errmsg += "Failed! We can't able to change your password. please make sure your old password is correct.";


            return Json(new { msg = errmsg, JsonRequestBehavior.AllowGet });
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


        public ActionResult DownloadPendingReportsUAC()
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

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "IDEA-X_Users_PendingReports.xlsx");
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


        public ActionResult DownloadInvestigatingReportsUAC()
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

        public ActionResult DownloadClosedReportsUAC()
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