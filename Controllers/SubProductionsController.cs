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
    public class SubProductionsController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        // GET: SubProductions
        public ActionResult Index()
        {
            var subproductions = db.subproductions.Include(s => s.plan).Include(s => s.production);
            return View(subproductions.ToList());
        }

        // GET: SubProductions/Details/5
        public ActionResult Details(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subproduction subproduction = db.subproductions.Find(id);
            if (subproduction == null)
            {
                return HttpNotFound();
            }
            return View(subproduction);
        }

        // GET: SubProductions/Create
        public ActionResult Create()
        {
            ViewBag.planid = new SelectList(db.plans, "planid", "plantype");
            ViewBag.pid = new SelectList(db.productions, "pid", "pname");
            return View();
        }

        // POST: SubProductions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "spid,planid,pid,startdate,enddate")] subproduction subproduction)
        {
            if (ModelState.IsValid)
            {
                db.subproductions.Add(subproduction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subproduction.planid);
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", subproduction.pid);
            return View(subproduction);
        }

        // GET: SubProductions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subproduction subproduction = db.subproductions.Find(id);
            if (subproduction == null)
            {
                return HttpNotFound();
            }
            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subproduction.planid);
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", subproduction.pid);
            return View(subproduction);
        }

        // POST: SubProductions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "spid,planid,pid,startdate,enddate")] subproduction subproduction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subproduction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subproduction.planid);
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", subproduction.pid);
            return View(subproduction);
        }

        // GET: SubProductions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subproduction subproduction = db.subproductions.Find(id);
            if (subproduction == null)
            {
                return HttpNotFound();
            }
            return View(subproduction);
        }

        // POST: SubProductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subproduction subproduction = db.subproductions.Find(id);
            db.subproductions.Remove(subproduction);
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
