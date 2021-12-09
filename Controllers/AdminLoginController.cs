using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentHunt.Models;
using TalentHunt.ModelView;

namespace TalentHunt.Controllers
{
    public class AdminLoginController : Controller
    {
        huntdbEntities db = new huntdbEntities();
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
    }
}