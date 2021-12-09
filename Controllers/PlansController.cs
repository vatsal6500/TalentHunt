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
    public class PlansController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        // GET: Plans
        public ActionResult Index()
        {
            if (Session["aid"] != null)
            {
                return View(db.plans.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        public ActionResult PlanSelect()
        {
            return View(db.plans.ToList());
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.plans.ToList());
                }


                List<plan> planTS = db.plans.Where(p => p.plantype.Contains(Search)).ToList();
                if (planTS.Count() == 0)
                {
                    List<plan> planLnS = db.plans.Where(p => p.price.ToString().Contains(Search)).ToList();
                    if (planLnS.Count() == 0)
                    {
                        TempData["pNotFound"] = "Plan Not Found";
                    }
                    else
                    {
                        return View(planLnS.ToList());
                    }
                }
                else
                {
                    return View(planTS.ToList());
                }

                return View(planTS.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Plans/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                plan plan = db.plans.Find(id);
                if (plan == null)
                {
                    return HttpNotFound();
                }
                return View(plan);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Plans/Create
        public ActionResult Create()
        {
            if (Session["aid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "planid,plantype,duration,price,description,benefits")] planv planv)
        {
            if (Session["aid"] != null)
            {
                if (ModelState.IsValid)
                {
                    plan plan = new plan();
                    AutoMapper.Mapper.Map(planv, plan);
                    //CloneObjects.CopyPropertiesTo(planv, plan);

                    db.plans.Add(plan);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(planv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Plans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                plan plan = db.plans.Find(id);
                planv planv = new planv();
                AutoMapper.Mapper.Map(plan, planv);

                if(planv == null)
                {
                    return HttpNotFound();
                }
                return View(planv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "planid,plantype,duration,price,description,benefits")] planv planv)
        {
            if (Session["aid"] != null)
            {
                if (ModelState.IsValid)
                {
                    plan plan = new plan();
                    AutoMapper.Mapper.Map(planv, plan);

                    db.Entry(plan).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(planv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Plans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                plan plan = db.plans.Find(id);
                if (plan != null)
                {
                    db.plans.Remove(plan);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // POST: Plans/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    plan plan = db.plans.Find(id);
        //    db.plans.Remove(plan);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
