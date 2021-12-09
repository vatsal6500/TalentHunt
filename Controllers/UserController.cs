using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TalentHunt.Models;
using TalentHunt.ModelView;

namespace TalentHunt.Controllers
{
    public class UserController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        //GET: User in admin
        public ActionResult AdminUserView()
        {
            if (Session["aid"] != null)
            {
                return View(db.users.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        //POST: User Search in admin
        public ActionResult AdminUserView(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.users.ToList());
                }


                List<user> userFnS = db.users.Where(p => p.fname.Contains(Search)).ToList();
                if (userFnS.Count() == 0)
                {
                    List<user> userLnS = db.users.Where(p => p.lname.Contains(Search)).ToList();
                    if(userLnS.Count() == 0)
                    {
                        //List<user>
                        //if()
                        //{
                        //    TempData["uNotFound"] = "User Not Found";
                        //}
                        //else
                        //{

                        //}
                    }
                    else
                    {
                        return View(userLnS.ToList());
                    }
                }
                else
                {
                    return View(userFnS.ToList());
                }

                return View(userFnS.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: User
        public ActionResult Index(user users, talent talents, userprofile userprofiles, image images)
        {
            if(Session["uid"] != null)
            {
                String output = "";

                int usrid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.userprofiles.Where(p => p.userid.Equals(usrid));

                var elists = db.images.Where(p => p.userid.Equals(usrid));
                TempData["piclists"] = elists;

                var videolists = db.videos.Where(p => p.userid.Equals(usrid));
                TempData["videolists"] = videolists;

                foreach (userprofile u in result)
                {
                    var tlists = db.talents.Where(p => p.tid.Equals(u.tid)).SingleOrDefault();

                    output += "<h5 style='color:indianred'>" + tlists.ttype.ToString() + "</h5>";
                    output += "<p class='mb-0'>Experience : " + u.experience + " years</p>";
                    output += "<p class='mb-0'>Portfolio : " + u.portfolio;
                    output += "</p><br/>";
                }
                TempData["tlist"] = output;

                var usr = db.users.Where(p => p.userid.Equals(usrid)).FirstOrDefault();
                if (result != null)
                {
                    TempData["mail"] = usr.email.ToString();
                    TempData["gender"] = usr.gender.ToString();
                    TempData["age"] = usr.age.ToString();
                }
                return View(db.users.ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult UserView()
        {
            var talents = db.talents.ToList();
            TempData["talents"] = talents;
            return View(db.userprofiles.ToList());
        }

        [HttpPost]
        public ActionResult UserView(int tid)
        {
            if (Session["pid"] != null)
            {
                if (tid.Equals(null) || tid == 0)
                {
                    var talent = db.talents.ToList();
                    TempData["talents"] = talent;
                    return View(db.userprofiles.ToList());
                }
                List<userprofile> plan = db.userprofiles.Where(p => p.tid.Equals(tid)).ToList();
                if (plan.Count() == 0)
                {
                    TempData["NotFounduv"] = "No such talent found";
                }
                var talents = db.talents.ToList();
                TempData["talents"] = talents;
                return View(plan.ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(login login,string options)
        {

            if (options=="user")
            {
                user usr = db.users.Where(p => p.username.Equals(login.username) && p.password.Equals(login.password)).FirstOrDefault();
                if (usr != null)
                {
                    Session["uid"] = usr.userid.ToString();
                    Session["photo"] = usr.photo.ToString();
                    Session["uname"] = usr.fname.ToString() + " " + usr.lname.ToString();
                    TempData["gender"] = usr.gender;
                    TempData["age"] = usr.age;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.err = "Invalid User Credentials";
                    TempData["register"] = "<div class='font-16 weight-600 pt-10 pb-10 text-center' data-color='#707373'>OR</div><div class='input-group mb-0'><a class='btn btn-outline-primary btn-lg btn-block' href='/User/Create'>Register To Create User Account</a></div>";
                }
            }
            else if(options=="production")
            {
                production pro = db.productions.Where(p => p.username.Equals(login.username) && p.password.Equals(login.password)).FirstOrDefault();
                if(pro != null)
                {
                    Session["pid"] = pro.pid.ToString();
                    Session["pimage"] = pro.pimage.ToString();
                    Session["pname"] = pro.pname.ToString();
                    return RedirectToAction("Index", "Production");
                }
                else
                {
                    ViewBag.err = "Invalid Production Credentials";
                    TempData["register"] = "<div class='font-16 weight-600 pt-10 pb-10 text-center' data-color='#707373'>OR</div><div class='input-group mb-0'><a class='btn btn-outline-primary btn-lg btn-block' href='/Production/Create'>Register To Create Production Account</a></div>";
                }
            }
            else
            {
                ViewBag.select = "Select User or Production";
            }
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int? id, user users, talent talents, userprofile userprofiles, image images)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String output = "";

            int usrid = Convert.ToInt32(id);
            var result = db.userprofiles.Where(p => p.userid.Equals(usrid));

            var elists = db.images.Where(p => p.userid.Equals(usrid));
            TempData["piclists"] = elists;

            var videolists = db.videos.Where(p => p.userid.Equals(usrid));
            TempData["videolists"] = videolists;


            foreach (userprofile u in result)
            {
                var tlists = db.talents.Where(p => p.tid.Equals(u.tid)).SingleOrDefault();

                output += "<h5 style='color:indianred'>" + tlists.ttype.ToString() + "</h5>";
                output += "<p class='mb-0'>Experience : " + u.experience + " years</p>";
                output += "<p class='mb-0'>Portfolio : " + u.portfolio;
                output += "</p><br/>";
            }
            TempData["tlist"] = output;

            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,fname,lname,gender,age,address,city,state,pincode,photo,email,username,password,cpassword,ImageFile")] userv userv)
        {
            if (ModelState.IsValid)
            {
                if (userv.ImageFile == null)
                {
                    ViewBag.emptyerr = "*";
                }
                else if (userv.ImageFile.ContentType == "image/jpeg" || userv.ImageFile.ContentType == "image/png" || userv.ImageFile.ContentType == "image/jpg")
                {
                    string fileName = Path.GetFileNameWithoutExtension(userv.ImageFile.FileName);
                    string extension = Path.GetExtension(userv.ImageFile.FileName);
                    fileName = fileName + extension;
                    userv.photo = "~/Images/User/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/User/"), fileName);
                    userv.ImageFile.SaveAs(fileName);

                    user user = new user();
                    AutoMapper.Mapper.Map(userv, user);

                    db.users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.picformat = "Invalid Format";
                }
            }
            return View(userv);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            userv userv = new userv();
            AutoMapper.Mapper.Map(user, userv);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,fname,lname,gender,age,address,city,state,pincode,photo,email,username,password")] userv userv)
        {
            if (ModelState.IsValid)
            {
                user user = new user();
                AutoMapper.Mapper.Map(userv,user);

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userv);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            if (Session["uid"] != null || Session["pid"] != null)
            {
                //Session.Clear();
                Session.Abandon();
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Index");
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
