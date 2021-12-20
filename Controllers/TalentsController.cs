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
    public class TalentsController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        // GET: talents
        public ActionResult Index()
        {
            if (Session["aid"] != null)
            {

                return View(db.talents.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
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
                if(Search.ToLower() == "all" || Search == "")
                {
                    return View(db.talents.ToList());
                }
                List<talent> talent = db.talents.Where(p => p.ttype.Contains(Search) || Search == null).ToList();
                if (talent.Count() == 0)
                {
                    TempData["tNotFound"] = "Talent Not Found";
                }

                return View(talent.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: talents/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                talent talent = db.talents.Find(id);
                if (talent == null)
                {
                    return HttpNotFound();
                }
                return View(talent);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: talents/Create
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

        // POST: talents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tid,ttype")] talentv talentv)
        {
            if (Session["aid"] != null)
            {
                if (ModelState.IsValid)
                {
                    List<talent> talent = db.talents.Where(p => p.ttype == talentv.ttype).ToList();
                    if(talent.Count() == 0)
                    {
                        talent Talent = new talent();
                        AutoMapper.Mapper.Map(talentv, Talent);

                        db.talents.Add(Talent);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    TempData["AlreadyExists"] = "Talent Already Exists";
                }

                return View(talentv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: talents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                talent talent = db.talents.Find(id);

                talentv talentv = new talentv();
                AutoMapper.Mapper.Map(talent, talentv);

                if (talent == null)
                {
                    return HttpNotFound();
                }
                return View(talentv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // POST: talents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tid,ttype")] talentv talentv)
        {
            if (Session["aid"] != null)
            {
                if (ModelState.IsValid)
                {
                    talent talent = new talent();
                    AutoMapper.Mapper.Map(talentv, talent);

                    db.Entry(talent).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(talentv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: talents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                talent talent = db.talents.Find(id);
                if (talent != null)
                {
                    db.talents.Remove(talent);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch(Exception e)
                    {
                        TempData["talenterr"] = "Talent cannot be deleted becasue users are available with this talents";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
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
