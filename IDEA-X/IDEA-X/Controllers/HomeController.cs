using IDEA_X.HelperClasses;
using IDEA_X.IDEAX_Class;
using IDEA_X.Models;
using IDEA_X.Models.EntityFramework;
using IDEA_X.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IDEA_X.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          


            if (Request.Cookies["UNAME"] != null && Request.Cookies["LEVEL"] != null)
            {
                if (Request.Cookies["LEVEL"].Value == "USER")
                {
                    var db = new IDEA_XEntities();
                    String uname = Request.Cookies["UNAME"].Value;
                    var user = db.ALL_USERS.Where(u => (u.USERNAME.Equals(uname))).SingleOrDefault();

                    if (user.LEVEL == "USER")
                    {
                        Session["UNAME"] = Request.Cookies["UNAME"].Value;
                        Session["LEVEL"] = Request.Cookies["LEVEL"].Value;

                        return RedirectToAction("Timeline", "User");
                    }
                    else {
                        ViewBag.message = "This user is banned for some issue. Please contact with Admin.";
                        foreach (string key in Request.Cookies.AllKeys)
                        {
                            HttpCookie c = Request.Cookies[key];
                            c.Expires = DateTime.Now.AddMonths(-1);
                            Response.AppendCookie(c);
                        }


                        Session.RemoveAll();
                        return RedirectToAction("Login");
                    }
                  
                   
                }


                    if (Request.Cookies["LEVEL"].Value == "ADMIN")
                    {
                    Session["UNAME"] = Request.Cookies["UNAME"].Value;
                    Session["LEVEL"] = Request.Cookies["LEVEL"].Value;
                        return RedirectToAction("Timeline", "Admin");
                    }

                if (Request.Cookies["LEVEL"].Value == "UAC")
                {
                    Session["UNAME"] = Request.Cookies["UNAME"].Value;
                    Session["LEVEL"] = Request.Cookies["LEVEL"].Value;
                    return RedirectToAction("Timeline", "UAC");
                }
                return View();
            }


            return View();
        }

    

    [HttpGet]
        public ActionResult MeetTheTeam()
        {
            return View();
        }
[HttpGet]
public ActionResult About()
        {

            return View();
        }
       

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserCredentialsVM user)
        {

            if (ModelState.IsValid)
            {
                //using(BinaryReader br = new BinaryReader(user.PROFILE_PICTURE.InputStream))
                //{
                //    user.PROFILE_IMG = br.ReadBytes(user.PROFILE_PICTURE.ContentLength);
                //}

                if(user.PROFILE_PICTURE==null)
                {
                    Image newImage = Image.FromFile(Server.MapPath("~/image/dashboard/Default_pfp.jpg"));
                    user.PROFILE_IMG = new IDEAX_Function().defImageToByte(newImage);
                }

                else
                {

 user.PROFILE_IMG = new IDEAX_Function().imageToByte(user.PROFILE_PICTURE);
                }

               
                Session["user_cred"] = user;
                //UserSignUp.EMAIL = us1.EMAIL;


                //UserSignUp.FIRST_NAME = us1.FIRST_NAME;
                //UserSignUp.LAST_NAME = us1.LAST_NAME;
                //UserSignUp.DATE_OF_BIRTH = us1.DATE_OF_BIRTH;
                //UserSignUp.GENDER = us1.GENDER;
                // UserSignUp.PROFILE_PICTURE = new IDEAX_Function().imageTObyte(postedFile);

                return RedirectToAction("SignUpExtend");

            }
            //return View(us1);
            return View(user);
        }



        [HttpGet]
        public ActionResult SignUpExtend()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpExtend(UserDetailsVM user)
        {

            var db = new IDEA_XEntities();
            
            
            var user_cred =(UserCredentialsVM) Session["user_cred"];

          
            if (ModelState.IsValid)
            {
                //UserSignUp.USERNAME = us2.USERNAME;
                //UserSignUp.PASSWORD = us2.PASSWORD;
                //UserSignUp.COUNTRY = us2.COUNTRY;
                //UserSignUp.INDUSTRY = us2.INDUSTRY;
                //UserSignUp.EDUCATIONAL_INSTITUTION = us2.EDUCATIONAL_INSTITUTION;
                //UserSignUp.DEPARTMENT = us2.DEPARTMENT;
                //UserSignUp.MOBILE = us2.MOBILE;
                //UserSignUp.HEADLINE = us2.HEADLINE;

                ALL_USERS allUsers = new ALL_USERS()
                {
                    EMAIL = user_cred.EMAIL,
                    PASSWORD = user.PASSWORD,
                    LEVEL = "USER",
                    USERNAME = user.USERNAME
                };




                //allUsers.USERNAME = UserSignUp.USERNAME;
                //allUsers.PASSWORD = UserSignUp.PASSWORD;
                //allUsers.EMAIL = UserSignUp.EMAIL;
                //allUsers.LEVEL = "USER";
                //db.ALL_USERS.Add(allUsers);
                //TODO : changed here --adb
                allUsers.PASSWORD = EncryptionAndHashLogic.HashPassword(user.PASSWORD);
                db.ALL_USERS.Add(allUsers);
                USER_DETAILS udetail = new USER_DETAILS()
                {
                    USERNAME = user.USERNAME,
                    FIRST_NAME = user_cred.FIRST_NAME,
                    LAST_NAME = user_cred.LAST_NAME,
                    HEADLINE = user.HEADLINE,
                    DATE_OF_BIRTH = user_cred.DATE_OF_BIRTH,
                    GENDER = user_cred.GENDER,
                    MOBILE = user.MOBILE,
                    USER_ADDRESS = "N/A",
                    USER_STATE = "N/A",
                    ZIP_CODE = "N/A",
                    COUNTRY = user.COUNTRY,
                    INDUSTRY = user.INDUSTRY,
                    EDUCATIONAL_INSTITUTION = user.EDUCATIONAL_INSTITUTION,
                    DEPARTMENT = user.DEPARTMENT,
                    CONTACT_URL = "N/A",
                    SIGNUP_TIME = new IDEAX_Function().time(),
                    USER_STATUS = "ACTIVE",
                    SIGNUP_IP = new IDEAX_Function().ip(),
                    PROFILE_PICTURE = user_cred.PROFILE_IMG,
                };
                //udetail.USERNAME = UserSignUp.USERNAME;
                //udetail.FIRST_NAME = UserSignUp.FIRST_NAME;
                //udetail.LAST_NAME = UserSignUp.LAST_NAME;
                //udetail.HEADLINE = UserSignUp.HEADLINE;
                //udetail.DATE_OF_BIRTH = UserSignUp.DATE_OF_BIRTH;
                //udetail.GENDER = UserSignUp.GENDER;
                //udetail.MOBILE = UserSignUp.MOBILE;
                //udetail.USER_ADDRESS = "N/A";
                //udetail.USER_STATE = "N/A";
                //udetail.ZIP_CODE = "N/A";
                //udetail.COUNTRY = UserSignUp.COUNTRY;
                //udetail.INDUSTRY = UserSignUp.INDUSTRY;
                //udetail.EDUCATIONAL_INSTITUTION = UserSignUp.EDUCATIONAL_INSTITUTION;
                //udetail.DEPARTMENT = UserSignUp.DEPARTMENT;
                //udetail.CONTACT_URL = "N/A";
                ////  udetail.PROFILE_PICTURE = null;
                //udetail.SIGNUP_TIME = new IDEAX_Function().time();
                //udetail.USER_STATUS = "ACTIVE";
                //udetail.SIGNUP_IP = new IDEAX_Function().ip();
                db.USER_DETAILS.Add(udetail);

                db.SaveChanges();


                Session.Remove("user_cred");

                return RedirectToAction("Login");


            }
            //return View(us2);
            return View(user);
        }








        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogIN login)
        {
            string msg = "";
            var db = new IDEA_XEntities();
            //var us = db.ALL_USERS.Where(uu =>  (uu.USERNAME == login.USERNAME || uu.EMAIL == login.USERNAME) && uu.PASSWORD == login.PASSWORD && uu.LEVEL == ("BANNED"));
            // if (us != null)
            // {

            //  msg = "This user is banned for some issue. Please contact with Admin.";
            // return View();

            // }
            //TODO : changed here --adb
            string l_pass = EncryptionAndHashLogic.HashPassword(login.PASSWORD);
            var user = db.ALL_USERS.Where(u => (u.USERNAME == login.USERNAME || u.EMAIL == login.USERNAME)
            && u.PASSWORD.Equals(l_pass)).SingleOrDefault();
            //var user = db.ALL_USERS.Where(u => (u.USERNAME == login.USERNAME || u.EMAIL == login.USERNAME) && u.PASSWORD == login.PASSWORD ).SingleOrDefault();
            if (user != null)
                {

                if (user.LEVEL != "BANNED")
                {
                     LOGIN logdetail = new LOGIN();
                    logdetail.USERNAME = user.USERNAME;
                    logdetail.EMAIL = user.EMAIL;
                    logdetail.LOGIN_TIME = new IDEAX_Function().time();
                    logdetail.LOGIN_IP = new IDEAX_Function().ip();
                    db.LOGINS.Add(logdetail);
                    db.SaveChanges();


                    Session["UNAME"] = user.USERNAME;
                    Session["LEVEL"] = user.LEVEL;

                    if (login.REMEMBER == "REMEMBER")
                    {
                        Response.Cookies["UNAME"].Value = user.USERNAME;
                        Response.Cookies["UNAME"].Expires = DateTime.Now.AddMonths(1);

                        Response.Cookies["LEVEL"].Value = user.LEVEL;
                        Response.Cookies["LEVEL"].Expires = DateTime.Now.AddMonths(1);
                    }



                    if (user.LEVEL == "USER")
                    {
                        return RedirectToAction("Timeline", "User");
                    }

                    if (user.LEVEL == "ADMIN")
                    {
                        return RedirectToAction("Timeline", "Admin");
                    }
                    if (user.LEVEL == "UAC")
                    {
                        return RedirectToAction("Timeline", "UAC");
                    }
                }
                else
                {
                   
                    ViewBag.message = "This user is banned for some issue. Please contact with Admin.";
                    return Login();
                }

                }

            ViewBag.message  = "Username/Email or Password couldn't find in our database.";

                
           






            return View(login);
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



        [HttpPost]
        public ActionResult EmailValidationReg(string mail_text)
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

                    return Json(new { msg = "", JsonRequestBehavior.AllowGet });
                }

                else
                {
                    return Json(new { msg = "This user isn't registered in this sytem.", JsonRequestBehavior.AllowGet });
                }


            }
            else
            {
                return Json(new { msg = "This is not a valid Email", JsonRequestBehavior.AllowGet });
            }


        }

        [HttpPost]
        public ActionResult EmailValidationForget(string mail_text)
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

                    return Json(new { msg = "", JsonRequestBehavior.AllowGet });
                }

                else
                {
                    return Json(new { msg = "Mail not found", JsonRequestBehavior.AllowGet });
                }

            }
            else
            {
                return Json(new { msg = "This is not a valid Email", JsonRequestBehavior.AllowGet });
            }


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
        public ActionResult UserNameValidationReg(string uname_text)
        {
            var db = new IDEA_XEntities();
            var user = (from u in db.ALL_USERS
                        where u.USERNAME == uname_text
                        select u).SingleOrDefault();
            if (user != null)
            {

                return Json(new { msg = "", JsonRequestBehavior.AllowGet });
            }


            return Json(new { msg = "This user isn't registered in this sytem.", JsonRequestBehavior.AllowGet });
        }


        private Random random = new Random();
        public string RandomString(int length)
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost]
        public async Task<ActionResult> SendCode(string remail)
        {
            string code = RandomString(5);

            //String body = "Your code is: " + code;
            bool retVal = false;
            try
            {
                retVal = await MailService.SendMail(remail, code);
                return Json(new { ret = retVal,msg=code, JsonRequestBehavior.AllowGet });
            }
            catch (Exception)
            {
                
            }
            //try
            //{
            //    MailMessage mailMessage = new MailMessage();
            //    mailMessage.From = new MailAddress("niloykantipaul@yahoo.com");

            //    mailMessage.To.Add(remail);

            //    mailMessage.Subject = "Validation Process for IDEA-X";

            //    // mailMessage.Attachments.Add(new Attachment(@"C:\\attachedfile.jpg"));
            //    mailMessage.Body = body;
            //    //mailMessage.IsBodyHtml = true;

            //    SmtpClient smtpClient = new SmtpClient("smtp.mail.yahoo.com");
            //    smtpClient.Port = 587;
            //    smtpClient.Credentials = new NetworkCredential("niloykantipaul@yahoo.com", "kqfzbitsnobxooly");
            //    smtpClient.EnableSsl = true;
            //    smtpClient.Send(mailMessage);
            //}
            //catch (Exception ex)
            //{

            //}



            //return Json(new { msg = code, JsonRequestBehavior.AllowGet });
            return Json(new { ret = retVal, JsonRequestBehavior.AllowGet });
        }




        [HttpGet]
        public ActionResult ForgotPasswordEmail()
        {
            return View();
        }



        [HttpGet]
        public ActionResult ForgotChangePassword(string id)
        {
            var db = new IDEA_XEntities();
            var user = (from p in db.ALL_USERS where p.EMAIL == id select p).SingleOrDefault();

            
            //TODO : changed here --adb
            ForgetPassword fogp = new ForgetPassword();
            fogp.CONFIRM_PASSWORD = "";//user.PASSWORD;
            fogp.PASSWORD = "";//user.PASSWORD;
            fogp.EMAIL = user.EMAIL;
            return View(fogp);

        }

        [HttpPost]
        public ActionResult ForgotChangePassword(ForgetPassword fogp)
        {
            if (ModelState.IsValid)
            {
                var db = new IDEA_XEntities();
                var user = (from p in db.ALL_USERS where p.EMAIL == fogp.EMAIL select p).SingleOrDefault();

                //TODO : changed here --adb
                string e_pass = EncryptionAndHashLogic.HashPassword(fogp.PASSWORD);
                user.PASSWORD = e_pass;
                db.SaveChanges();


                return RedirectToAction("Login");
            }

            return View(fogp);
        }
[HttpGet]
  public ActionResult Contact()
        {
            

            return View();
        }
     


        [HttpPost]
        public ActionResult ContactNonReg(String FIRST_NAME, String LAST_NAME, String EMAIL, String MESSAGE)
        {
            var db = new IDEA_XEntities();
            // var user_detail = db.USER_DETAILS.Where(x => x.USERNAME.Contains(id.ToLower())).ToList();
            var users = (from p in db.CONTACTS
                         where p.EMAIL.Equals(EMAIL) && p.STATUS.Equals("PENDING")
                         select p).ToList();
           
                var count = 0;
                foreach(var u in users)
                {
                    count++;
                }

                if(count<3)
                {
                    CONTACT c = new CONTACT();
                    c.FIRST_NAME = FIRST_NAME;
                    c.LAST_NAME = LAST_NAME;
                    c.USERNAME = "";
                    c.EMAIL = EMAIL;
                    c.MESSAGE = MESSAGE;
                    c.LEVEL = "NONREG";
                    c.STATUS = "PENDING";
                    c.LOGIN_TIME = new IDEAX_Function().time();
                    c.LOGIN_IP = new IDEAX_Function().ip();
                    db.CONTACTS.Add(c);
                    db.SaveChanges();

                    return Json(new { msg = "Save", JsonRequestBehavior.AllowGet });
                }
                return Json(new { msg = "DoNotSave", JsonRequestBehavior.AllowGet });

            



        }

        [HttpPost]
        public ActionResult ContactReg(String USERNAME, String EMAIL, String MESSAGE)
        {

            var db = new IDEA_XEntities();
            // var user_detail = db.USER_DETAILS.Where(x => x.USERNAME.Contains(id.ToLower())).ToList();
            var users = (from p in db.CONTACTS
                         where p.EMAIL.Equals(EMAIL) && p.STATUS.Equals("PENDING") && p.USERNAME.Equals(USERNAME)
                         select p).ToList();
            var count = 0;

            if (users.Count() > 0)
            {

                foreach (var u in users)
                {
                    count++;
                }
            }
            if (count < 3)
            {
                var email = (from e in db.ALL_USERS
                             where e.EMAIL == EMAIL && e.USERNAME == USERNAME && (e.LEVEL == "USER" || e.LEVEL == "BANNED")
                             select e).SingleOrDefault();
                if (email != null)
                {
                    var name = (from u in db.USER_DETAILS
                                where u.USERNAME == email.USERNAME
                                select u).SingleOrDefault();

                    CONTACT c = new CONTACT();
                    c.FIRST_NAME = name.FIRST_NAME;
                    c.LAST_NAME = name.LAST_NAME;
                    c.USERNAME = email.USERNAME;
                    c.EMAIL = EMAIL;
                    c.MESSAGE = MESSAGE;
                    c.LEVEL = email.LEVEL;
                    c.STATUS = "PENDING";
                    c.LOGIN_TIME = new IDEAX_Function().time();
                    c.LOGIN_IP = new IDEAX_Function().ip();
                    db.CONTACTS.Add(c);
                    db.SaveChanges();

                    return Json(new { msg = "Save", JsonRequestBehavior.AllowGet });
                }
                return Json(new { msg = "UserNotMatch", JsonRequestBehavior.AllowGet });
            }
            return Json(new { msg = "DoNotSave", JsonRequestBehavior.AllowGet });


        }


    }
}