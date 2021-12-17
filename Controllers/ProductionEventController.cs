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
    public class ProductionEventController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        // GET: ProductionEvent
        public ActionResult Index()
        {
            if (Session["pid"] != null)
            {
                int pid = Convert.ToInt32(HttpContext.Session["pid"]);
                var result = db.productionevents.Where(x => x.pid.Equals(pid));
                TempData["pdata"] = result;
                return View(db.productionevents.ToList());
            }
            if (Session["uid"] != null)
            {

                var productionevents = db.productionevents.Include(p => p.production);

                return View(productionevents.ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            if (Session["uid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.productionevents.ToList());
                }

                List<productionevent> eventNS = db.productionevents.Where(p => p.ename.Contains(Search)).ToList();
                if(eventNS.Count() == 0)
                {
                    List<productionevent> eventDS = db.productionevents.Where(p => p.description.Contains(Search)).ToList();
                    if (eventDS.Count() == 0)
                    {
                        List<productionevent> eventDaS = db.productionevents.Where(p => p.startdate.ToString().Contains(Search)).ToList();
                        if (eventDaS.Count() == 0)
                        {
                            TempData["NotFoundpe"] = "No such event found";
                        }
                        else
                        {
                            return View(eventDaS.ToList());
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
            else if (Session["pid"] != null)
            {
                int pid = Convert.ToInt32(HttpContext.Session["pid"]);
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    var results = db.productionevents.Where(x => x.pid.Equals(pid));
                    TempData["pdata"] = results;
                    return View();
                }
                List<productionevent> eventNS = db.productionevents.Where(p => p.ename.Contains(Search) && p.pid.Equals(pid)).ToList();
                if (eventNS.Count() == 0)
                {
                    List<productionevent> eventDS = db.productionevents.Where(p => p.description.Contains(Search) && p.pid.Equals(pid)).ToList();
                    if(eventDS.Count() == 0)
                    {
                        List<productionevent> eventDaS = db.productionevents.Where(p => p.startdate.ToString().Contains(Search) && p.pid.Equals(pid)).ToList();
                        if(eventDaS.Count() == 0)
                        {
                            TempData["NotFoundp"] = "No such registered event";
                        }
                        else
                        {
                            TempData["pdata"] = eventDaS;
                            return View(eventDaS.ToList());
                        }
                    }
                    else
                    {
                        TempData["pdata"] = eventDS;
                        return View(eventDS.ToList());
                    }  
                }
                else
                {
                    TempData["pdata"] = eventNS;
                    return View(eventNS.ToList());
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult EventView()
        {
            if (Session["aid"] != null)
            {
                return View(db.productionevents.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult EventView(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.productionevents.ToList());
                }
                List<productionevent> events = db.productionevents.Where(p => p.ename.Contains(Search) || Search == null).ToList();
                if (events.Count() == 0)
                {
                    TempData["NotFound"] = "Data Not Found";
                }

                return View(events.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: ProductionEvent/Details/5
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
                    int uid = Convert.ToInt32(HttpContext.Session["uid"]);
                    var userbids = db.userapplies.Where(x => x.peid.Equals(pid));
                    if (userbids.Count() == 0)
                    {
                        TempData["NotFoundUserBids"] = "No one has bid on this event";
                    }
                    TempData["bids"] = userbids;

                    var applybid = db.userapplies.Where(x => x.peid.Equals(pid) && x.userid.Equals(uid));
                    TempData["status"] = applybid;
                    if (applybid.Count() == 0)
                    {
                        TempData["bidded"] = "No";
                    }
                    else
                    {
                        TempData["bidded"] = "Yes";
                    }
                }

                var selects = db.userselects.Where(x => x.peid.Equals(pid));
                TempData["userselected"] = selects;
                if (selects.Count() == 0)
                {
                    TempData["selected"] = "No";
                }
                else
                {
                    TempData["selected"] = "Yes";

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

        // GET: ProductionEvent/Create
        public ActionResult Create(int? id)
        {
            ViewBag.pid = id;
            return View();
        }

        // POST: ProductionEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "peid,pid,ename,etype,emanager,startdate,enddate,evenu,evisitors,appdeadline,description,image,ImageFile")] productioneventv productioneventv,int pid)
        {
            if (ModelState.IsValid)
            {
                if (productioneventv.ImageFile == null)
                {
                    ViewBag.emptyerr = "*";
                }
                else if (productioneventv.ImageFile.ContentType == "image/jpeg" || productioneventv.ImageFile.ContentType == "image/png" || productioneventv.ImageFile.ContentType == "image/jpg")
                {
                    string fileName = Path.GetFileNameWithoutExtension(productioneventv.ImageFile.FileName);
                    string extension = Path.GetExtension(productioneventv.ImageFile.FileName);
                    fileName = fileName + extension;
                    productioneventv.image = "~/Images/ProductionEvent/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/ProductionEvent/"), fileName);
                    productioneventv.ImageFile.SaveAs(fileName);

                    productioneventv.pid = pid;

                    productionevent productionevent = new productionevent();
                    AutoMapper.Mapper.Map(productioneventv, productionevent);

                    db.productionevents.Add(productionevent);
                    db.SaveChanges();
                    return RedirectToAction("Create", "EventRequire", new { peid = productionevent.peid });
                }
                else
                {
                    ViewBag.picformat = "Only jpeg/png/jpg format";
                }
            }

            ViewBag.pid = new SelectList(db.productions, "pid", "pname", productioneventv.pid);
            return View(productioneventv);
        }

        // GET: ProductionEvent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productionevent productionevent = db.productionevents.Find(id);
            productioneventv productioneventv = new productioneventv();
            AutoMapper.Mapper.Map(productionevent,productioneventv);
            if (productionevent == null)
            {
                return HttpNotFound();
            }
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", productioneventv.pid);
            return View(productioneventv);
        }

        // POST: ProductionEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "peid,pid,ename,etype,emanager,startdate,enddate,evenu,evisitors,appdeadline,description,image")] productioneventv productioneventv)
        {
            if (ModelState.IsValid)
            {
                productionevent productionevent = new productionevent();
                db.Entry(productionevent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", productioneventv.pid);
            return View(productioneventv);
        }

        // GET: ProductionEvent/Delete/5
        public ActionResult Delete(int? id)
        {
            if(Session["aid"] != null)
            {
                if (id == null)
                {
                    return RedirectToAction("EventView");
                }
                productionevent productionevent = db.productionevents.Find(id);
                if (productionevent != null)
                {
                    db.productionevents.Remove(productionevent);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        TempData["deleteerr"] = "Event cannot be deleted because bidders has applied on this event";
                    }
                    return RedirectToAction("EventView");
                }
                else
                {
                    return RedirectToAction("EventView");
                }
            }
            else if(Session["pid"] != null)
            {
                if(id == null)
                {
                    return RedirectToAction("Index","ProductionEvent");
                }
                productionevent productionevent = db.productionevents.Find(id);
                if(productionevent == null)
                {
                    return RedirectToAction("Index", "ProductionEvent");
                }
                db.productionevents.Remove(productionevent);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    TempData["fkerr"] = $"fk error {ex.Message}";
                }
                return RedirectToAction("Index","ProductionEvent");
            }
            else if(Session["pid"] == null)
            {
                return RedirectToAction("Login","User");
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        public ActionResult RequestUser(int? uid,int? pid)
        {
            if(uid == null)
            {
                RedirectToAction("UserView","User");
            }
            if(pid == null)
            {
                return RedirectToAction("Details","User",new { id = uid});
            }
            var eventList = db.productionevents.Where(p => p.pid == pid);
            TempData["EventList"] = eventList;
            ViewBag.uid = uid;

            return View();
        }

        [HttpPost]
        public ActionResult RequestUser(productionevent productionevent,string ename,int? uid)
        {
            if (uid == null)
            {
                RedirectToAction("UserView", "User");
            }
            if (productionevent.description == null && ename == null)
            {
                return RedirectToAction("Details","User",new { id = uid});
            }
            int pid = Convert.ToInt32(HttpContext.Session["pid"]);
            string fname = db.users.Where(p => p.userid == uid).SingleOrDefault().fname;
            string lname = db.users.Where(p => p.userid == uid).SingleOrDefault().lname;
            string uemail = db.users.Where(p => p.userid == uid).SingleOrDefault().email;
            string pname = db.productions.Where(p => p.pid == pid).FirstOrDefault().pname;
            
            string subject = $"Bidding request from {pname} Production";

            string msg = $"<u>{fname} {lname}</u> we want you to bid on our {ename} Event.<br/> We think you will be more suitable to host this event. <br/><br/> {productionevent.description} ";

            email email = new email(uemail, subject, msg);

            if (email != null)
            {
                TempData["true"] = true;
                //TempData["sent"] = "Your Email Sent";
                return RedirectToAction("Details","USer",new { id = uid});
            }

            return View();
        }


        //// POST: ProductionEvent/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    productionevent productionevent = db.productionevents.Find(id);
        //    db.productionevents.Remove(productionevent);
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
