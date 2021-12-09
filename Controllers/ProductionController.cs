using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentHunt.Models;
using TalentHunt.ModelView;

namespace TalentHunt.Controllers
{
    public class ProductionController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        // GET: Production
        public ActionResult Index()
        {
            if(Session["pid"] != null)
            {
                int pid = Convert.ToInt32(HttpContext.Session["pid"]);
                var result = db.productions.Where(x => x.pid.Equals(pid));
                TempData["pdata"] = result;
                return View(db.productions.ToList());
            }
            else
            {
                return RedirectToAction("Login","User");
            }
            
        }

        public ActionResult PrShortView(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["aid"] != null)
            {
                production production = db.productions.Find(id);
                if (production == null)
                {
                    return HttpNotFound();
                }
                return View(production);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

        }

        public ActionResult ProductionView()
        {
            return View(db.productions.ToList());
        }
        [HttpPost]
        public ActionResult ProductionView(string Search)
        {
            if (Session["uid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.productions.ToList());
                }


                List<production> eventNS = db.productions.Where(p => p.pname.Contains(Search)).ToList();
                if (eventNS.Count() == 0)
                {
                    List<production> eventDS = db.productions.Where(p => p.description.Contains(Search)).ToList();
                    if(eventDS.Count() == 0)
                    {
                        List<production> eventHS = db.productions.Where(p => p.phead.Contains(Search)).ToList();
                        if(eventHS.Count() == 0)
                        {
                            TempData["NotFound"] = "Data Not Found";
                        }
                        else
                        {
                            return View(eventHS.ToList());
                        }
                    }
                    else
                    {
                        return View(eventDS.ToList());
                    }
                }
                else
                {
                    return View(eventNS.ToList());
                }

                return View(eventNS.ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult ProductionList()
        {
            if (Session["aid"] != null)
            {
                return View(db.productions.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult ProductionList(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.productions.ToList());
                }
                List<production> productions = db.productions.Where(p => p.pname.Contains(Search) || Search == null).ToList();
                if (productions.Count() == 0)
                {
                    TempData["NotFound"] = "Data Not Found";
                }

                return View(productions.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }


        // GET: Production/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                string pname = "";
                int pid = Convert.ToInt32(id);
                var result = db.productionevents.Where(p => p.peid.Equals(pid));
                var reqs = db.eventrequires.Where(p => p.peid.Equals(pid));
                TempData["reqs"] = reqs;

                if (Session["uid"] != null)
                {
                    var userbids = db.userapplies.Where(x => x.peid.Equals(pid));
                    if (userbids.Count() == 0)
                    {
                        TempData["NotFoundUserBids"] = "No one has bid on this event";
                    }
                    TempData["bids"] = userbids;
                }

                var selects = db.userselects.Where(x => x.peid.Equals(pid));
                if (selects.Count() == 0)
                {
                    TempData["selected"] = "No";
                }
                else
                {
                    TempData["selected"] = "Yes";
                    TempData["userselected"] = selects;
                }

                if (Session["pid"] != null)
                {
                    int prid = Convert.ToInt32(HttpContext.Session["pid"]);
                    var bids = db.userapplies.Where(x => x.pid.Equals(prid) && x.peid.Equals(pid));
                    if (bids.Count() == 0)
                    {
                        TempData["NotFound"] = "No one has bid on this event";
                    }
                    TempData["pdata"] = bids;


                }

                foreach (productionevent pe in result)
                {
                    var pdata = db.productions.Where(p => p.pid.Equals(pe.pid)).SingleOrDefault();
                    pname += pdata.pname.ToString();
                }

                TempData["name"] = pname;
            }
            productionevent productionevent = db.productionevents.Find(id);
            if (productionevent == null)
            {
                return HttpNotFound();
            }
            return View(productionevent);
        }

        // GET: Production/Create
        public ActionResult Create(int? id)
        {
            if(id == 0 || id == null)
            {
                return RedirectToAction("PlanSelect","Plans");
            }
            else
            {
                string name = db.plans.Where(x => x.planid == id).SingleOrDefault()?.plantype;
                string duration = db.plans.Where(x => x.planid == id).SingleOrDefault()?.duration;
                int price = db.plans.Where(x => x.planid == id).SingleOrDefault().price;

                ViewBag.id = id;
                ViewBag.name = name;
                ViewBag.duration = duration;
                ViewBag.price = price;
            }

            //ViewBag.planid = new SelectList(db.plans, "planid", "plantype");
            return View();
        }

        // POST: Production/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pid,pname,pimage,phead,address,contactno,email,username,password,cpassword,description,ImageFile,status")] productionv productionv,subproductionv subproductionv,int planid)
        {
            if (ModelState.IsValid)
            {
                if (productionv.ImageFile == null)
                {
                    ViewBag.emptyerr = "*";
                }
                else if (productionv.ImageFile.ContentType == "image/jpeg" || productionv.ImageFile.ContentType == "image/png" || productionv.ImageFile.ContentType == "image/jpg")
                {
                    string fileName = Path.GetFileNameWithoutExtension(productionv.ImageFile.FileName);
                    string extension = Path.GetExtension(productionv.ImageFile.FileName);
                    fileName = fileName + extension;
                    productionv.pimage = "~/Images/Production/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Production/"), fileName);
                    productionv.ImageFile.SaveAs(fileName);
                    productionv.status = "active";

                    int Duration = Convert.ToInt32(db.plans.Where(x => x.planid == planid).SingleOrDefault()?.duration);

                    subproductionv.planid = planid;
                    subproductionv.pid = productionv.pid;
                    subproductionv.startdate = DateTime.Now;
                    subproductionv.enddate = DateTime.Now.AddMonths(Duration);

                    subproduction subproduction = new subproduction();
                    production production = new production();

                    AutoMapper.Mapper.Map(subproductionv, subproduction);
                    AutoMapper.Mapper.Map(productionv, production);

                    db.subproductions.Add(subproduction);
                    db.productions.Add(production);
                    db.SaveChanges();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ViewBag.picformat = "Invalid Format";
                }
            }
            string name = db.plans.Where(x => x.planid == planid).SingleOrDefault()?.plantype;
            string duration = db.plans.Where(x => x.planid == planid).SingleOrDefault()?.duration;
            int price = db.plans.Where(x => x.planid == planid).SingleOrDefault().price;

            ViewBag.id = planid;
            ViewBag.name = name;
            ViewBag.duration = duration;
            ViewBag.price = price;
            return View(productionv);
        }

        // GET: Production/Edit/5
        public ActionResult Edit(int? id,string status)
        {
            if (id == null || status == null)
            {
                return RedirectToAction("ProductionList","Production");
            }
            production production = db.productions.Find(id);
            if(status == "blocked")
            {
                production.status = status;
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductionList", "Production");
            }
            if(status == "active")
            {
                production.status = status;
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductionList", "Production");
            }
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Production/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pid,pname,pimage,phead,address,contactno,email,username,password,description")] productionv productionv)
        {
            if (ModelState.IsValid)
            {
                production production = new production();
                AutoMapper.Mapper.Map(productionv,production);
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productionv);
        }

        // GET: Production/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production production = db.productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Production/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            production production = db.productions.Find(id);
            db.productions.Remove(production);
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
