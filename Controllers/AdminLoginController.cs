using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using TalentHunt.Models;
using TalentHunt.ModelView;

namespace TalentHunt.Controllers
{
    public class AdminLoginController : Controller
    {
        huntdbEntities db = new huntdbEntities();
        public static Random random = new Random();     
        public static int otp = random.Next(100000, 999999);

        // GET: Admin
        public ActionResult Login()
        {
            //HttpCookie cookie = Request.Cookies["RememberMe"];
            //if(cookie != null)
            //{
            //    ViewBag.username = cookie["username"].ToString();
            //    ViewBag.password = cookie["password"].ToString();
            //}
            return View();
        }

        [HttpPost]
        public ActionResult Login(adminlogin adminlogin)
        {
            //HttpCookie cookie = new HttpCookie("RememberMe");
            //if (admins.RememberMe == true)
            //{
            //    cookie["username"] = admins.username;
            //    cookie["password"] = admins.password;
            //    cookie.Expires = DateTime.Now.AddDays(2);
            //    Response.Cookies.Add(cookie);
            //}
            //else
            //{
            //    cookie.Expires = DateTime.Now.AddDays(-1);
            //    HttpContext.Response.Cookies.Add(cookie);
            //}

            if(ModelState.IsValid)
            {
                admin adm = db.admins.Where(a => a.username.Equals(adminlogin.username) && a.password.Equals(adminlogin.password)).FirstOrDefault();
                if (adm != null)
                {
                    Session["aid"] = adm.aid.ToString();
                    //Session["username"] = adm.username.ToString();
                    Session["name"] = adm.aname.ToString();
                    return RedirectToAction("Dashboard");
                }
                else if (adminlogin.username != null && adminlogin.password != null)
                {
                    ViewBag.errmsg = "Invalid Credentials";
                }
            }
            return View();
        }

        public ActionResult Dashboard()
        {
            if(Session["aid"] != null)
            {
                ViewBag.user = db.users;
                ViewBag.production = db.productions;
                ViewBag.bids = db.userapplies;
                ViewBag.events = db.productionevents;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult Logout()
        {
            if(Session["aid"] != null)
            {
                //Session.Clear();
                Session.Abandon();
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }

        public ActionResult ResetPassword()
        {
            if(TempData["adminemail"] == null)
            {
                return RedirectToAction("checkemail","AdminLogin");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(checkpass checkpass, string email)
        {
            if(ModelState.IsValid)
            {
                admin admin = db.admins.Where(p => p.email.Equals(email)).FirstOrDefault();
                admin.password = checkpass.password;
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Login","AdminLogin");
            }
            return View();
        }

        public ActionResult check()
        {
            return View();
        }

        [HttpPost]
        public ActionResult check(emailcheck emailcheck)
        {
            if (ModelState.IsValid)
            {

                admin admin = db.admins.Where(p => p.email.Equals(emailcheck.email)).SingleOrDefault();
                if (admin == null)
                {
                    TempData["emailerr"] = "Email invalid or not available";
                    return RedirectToAction("check", "AdminLogin");
                }
                
                string subject = "OTP";
                string message = "One Time Password: " + otp + ". Don't share it with anyone.";

                email email = new email(admin.email, subject, message);

                if (email != null)
                {
                    TempData["otp"] = true;
                }
            }
            TempData["email"] = emailcheck.email;
            return View(emailcheck);
        }

        [HttpPost]
        public ActionResult checkotp(int? eotp,emailcheck emailcheck)
        {
            if(eotp == null)
            {
                TempData["otp"] = true;
                TempData["otperr"] = "*";
                TempData["email"] = emailcheck.email;
                return RedirectToAction("check","AdminLogin");
            }

            if (otp.Equals(eotp))
            {
                TempData["adminemail"] = emailcheck.email;
                return RedirectToAction("ResetPassword","AdminLogin");
            }
            else
            {
                TempData["otp"] = true;
                TempData["otperr"] = "InCorrect OTP";
                return RedirectToAction("Check","AdminLogin");
            }

        }

    }
}