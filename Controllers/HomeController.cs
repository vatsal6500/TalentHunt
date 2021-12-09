using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentHunt.Models;

namespace TalentHunt.Controllers
{
    public class HomeController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        public ActionResult Index()
        {
            ViewBag.user = db.users;
            ViewBag.production = db.productions;
            ViewBag.bids = db.userapplies;
            ViewBag.events = db.productionevents;
            ViewBag.plans = db.plans;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}