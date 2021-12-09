using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentHunt.Models;
using TalentHunt.ModelView;

namespace TalentHunt.Controllers
{
    public class UserApplyController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        // GET: UserApply
        public ActionResult Index()
        {
            if (Session["uid"] != null)
            {
                int uid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.userapplies.Where(x => x.userid.Equals(uid));
                if (result.Count() == 0)
                {
                    TempData["NotFound"] = "No biddings yet";
                }
                TempData["pdata"] = result;
                return View(db.userapplies.ToList());
            }

            if (Session["pid"] != null)
            {
                int pid = Convert.ToInt32(HttpContext.Session["pid"]);
                var result = db.userapplies.Where(x => x.pid.Equals(pid));
                if (result.Count() == 0)
                {
                    TempData["NotFound"] = "No biddings yet";
                }
                TempData["pdata"] = result;
                return View(db.userapplies.ToList());
            }
            var userapplies = db.userapplies.Include(u => u.production).Include(u => u.productionevent).Include(u => u.user);
            return View(userapplies.ToList());
        }

        // GET: UserApply/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userapply userapply = db.userapplies.Find(id);
            if (userapply == null)
            {
                return HttpNotFound();
            }
            return View(userapply);
        }

        // GET: UserApply/Create
        public ActionResult Create(int? id)
        {
            ViewBag.eid = id;
            ViewBag.pid = new SelectList(db.productions, "pid", "pname");
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename");
            ViewBag.userid = new SelectList(db.users, "userid", "fname");
            return View();
        }

        // POST: UserApply/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "uaid,pid,peid,userid,appdate,expay,message")] userapplyv userapplyv, int eid)
        {
            if (ModelState.IsValid)
            {
                int uid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.productionevents.Where(p => p.peid.Equals(eid)).SingleOrDefault();

                userapplyv.appdate = DateTime.Now;
                userapplyv.pid = result.pid;
                userapplyv.peid = eid;
                userapplyv.userid = uid;

                userapply userapply = new userapply();
                AutoMapper.Mapper.Map(userapplyv, userapply);

                db.userapplies.Add(userapply);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pid = new SelectList(db.productions, "pid", "pname", userapplyv.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userapplyv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userapplyv.userid);
            return View(userapplyv);
        }

        // GET: UserApply/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userapply userapply = db.userapplies.Find(id);

            userapplyv userapplyv = new userapplyv();
            AutoMapper.Mapper.Map(userapply, userapplyv);

            if (userapply == null)
            {
                return HttpNotFound();
            }
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", userapply.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userapply.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userapply.userid);
            return View(userapply);
        }

        // POST: UserApply/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "uaid,pid,peid,userid,appdate,expay,message")] userapplyv userapplyv)
        {
            if (ModelState.IsValid)
            {
                userapply userapply = new userapply();
                AutoMapper.Mapper.Map(userapplyv,userapply);
                db.Entry(userapply).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", userapplyv.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userapplyv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userapplyv.userid);
            return View(userapplyv);
        }

        // GET: UserApply/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userapply userapply = db.userapplies.Find(id);
            if (userapply == null)
            {
                return HttpNotFound();
            }
            return View(userapply);
        }

        // POST: UserApply/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userapply userapply = db.userapplies.Find(id);
            db.userapplies.Remove(userapply);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}