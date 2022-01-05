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
                        TempData["uNotFound"] = "User Not Found";
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
                int usrid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.userprofiles.Where(p => p.userid.Equals(usrid));
                TempData["tlist"] = result;

                var elists = db.images.Where(p => p.userid.Equals(usrid));
                TempData["piclists"] = elists;

                var videolists = db.videos.Where(p => p.userid.Equals(usrid));
                TempData["videolists"] = videolists;

                var rates = db.ratings.Where(p => p.userid.Equals(usrid));
                if (rates.Count() != 0)
                {
                    double sum = 0;
                    double finalrate = 0;
                    double total = rates.Count();
                    foreach (var rt in rates)
                    {
                        sum += rt.rating1;
                    }
                    finalrate = sum / total;
                    TempData["ratings"] = finalrate;
                }
                else
                {
                    TempData["ratings"] = 0;
                }

                var usr = db.users.Where(p => p.userid.Equals(usrid)).FirstOrDefault();
                if (result != null)
                {
                    TempData["fname"] = usr.fname.ToString();
                    TempData["lname"] = usr.lname.ToString();
                    TempData["age"] = usr.age.ToString();
                    TempData["gender"] = usr.gender.ToString();
                    TempData["mail"] = usr.email.ToString();
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

        public ActionResult UserList()
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
        [HttpPost]
        public ActionResult UserList(string Search)
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

                List<user> users = db.users.Where(p => p.fname.Contains(Search) || Search == null).ToList();
                if (users.Count() == 0)
                {
                    List<user> gender = db.users.Where(p => p.gender.Equals(Search)).ToList();
                    if(gender.Count() == 0)
                    {
                        try
                        {
                            int s = Convert.ToInt32(Search);
                            List<user> ages = db.users.Where(p => p.age <= s).ToList();
                            if (ages.Count() == 0)
                            {
                                TempData["NotFound"] = "User Not Found";
                            }
                            else
                            {
                                return View(ages.ToList());
                            }
                        }
                        catch
                        {
                            TempData["NotFound"] = "User Not Found";
                        }
                    }
                    else
                    {
                        return View(gender.ToList());
                    }
                }
                else
                {
                    return View(users.ToList());
                }

                return View(users.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        public ActionResult ShortView(int? id, user users, talent talents, userprofile userprofiles, image images)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["aid"] != null)
            {
                int usrid = Convert.ToInt32(id);
                var result = db.userprofiles.Where(p => p.userid.Equals(usrid));
                TempData["tlist"] = result;

                var elists = db.images.Where(p => p.userid.Equals(usrid));
                TempData["piclists"] = elists;

                var videolists = db.videos.Where(p => p.userid.Equals(usrid));
                TempData["videolists"] = videolists;

                var rates = db.ratings.Where(p => p.userid.Equals(usrid));
                if (rates.Count() != 0)
                {
                    double sum = 0;
                    double finalrate = 0;
                    double total = rates.Count();
                    foreach (var rt in rates)
                    {
                        sum += rt.rating1;
                    }
                    finalrate = sum / total;
                    TempData["ratings"] = finalrate;
                }
                else
                {
                    TempData["ratings"] = 0;
                }
                user user = db.users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
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
            try
            {
                if (options == "user")
                {
                    user usr = db.users.Where(p => p.username.Equals(login.username) && p.password.Equals(login.password)).FirstOrDefault();
                    if (usr != null)
                    {
                        if (usr.status == "active")
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
                            ViewBag.err = "You are banned from using TalentHunt or email not verified";
                        }
                    }
                    else
                    {
                        ViewBag.err = "Invalid User Credentials";
                        TempData["register"] = "<div class='font-16 weight-600 pt-10 pb-10 text-center' data-color='#707373'>OR</div><div class='input-group mb-0'><a class='btn btn-outline-primary btn-lg btn-block' href='/User/Create'>Register To Create User Account</a></div>";
                    }
                }
                else if (options == "production")
                {
                    production pro = db.productions.Where(p => p.username.Equals(login.username) && p.password.Equals(login.password)).FirstOrDefault();
                    if (pro != null)
                    {
                        if (pro.status == "active")
                        {
                            Session["pid"] = pro.pid.ToString();
                            Session["pimage"] = pro.pimage.ToString();
                            Session["pname"] = pro.pname.ToString();
                            return RedirectToAction("Index", "Production");
                        }
                        else
                        {
                            ViewBag.err = "You are banned from using Talent hunt";
                        }
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
            }
            catch(Exception ex)
            {
                ViewBag.err = "There is Some problem try after some time";
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

            int usrid = Convert.ToInt32(id);
            var result = db.userprofiles.Where(p => p.userid.Equals(usrid));
            TempData["tlist"] = result;

            var elists = db.images.Where(p => p.userid.Equals(usrid));
            TempData["piclists"] = elists;

            var videolists = db.videos.Where(p => p.userid.Equals(usrid));
            TempData["videolists"] = videolists;

            //int id = db.productionevents.

            var rates = db.ratings.Where(p => p.userid.Equals(usrid));
            if (rates.Count() != 0)
            {
                double sum = 0;
                double finalrate = 0;
                double total = rates.Count();
                foreach (var rt in rates)
                {
                    sum += rt.rating1;
                }
                finalrate = sum / total;
                TempData["ratings"] = finalrate;
            }
            else
            {
                TempData["ratings"] = 0;
            }


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
        public ActionResult Create([Bind(Include = "userid,fname,lname,gender,age,address,city,state,pincode,photo,email,username,password,cpassword,ImageFile,active")] userv userv)
        {
            if (ModelState.IsValid)
            {
                user email = db.users.Where(p => p.email.Equals(userv.email)).FirstOrDefault();
                user username = db.users.Where(p => p.username.Equals(userv.username)).FirstOrDefault();
                if(email.email != null && username.username != null)
                {
                    ViewBag.uemail = "email already registered";
                    ViewBag.uuname = "username already registered";
                }
                else if (email.email != null)
                {
                    ViewBag.uemail = "email already registered";
                }
                else
                {
                    if (username.username != null)
                    {
                        ViewBag.uuname = "username already registered";
                    }
                    else
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
                            userv.status = "blocked";

                            user user = new user();
                            AutoMapper.Mapper.Map(userv, user);

                            db.users.Add(user);
                            db.SaveChanges();

                            TempData["ucre"] = true;

                            //string FilePath = "E:\\TalentHunt\\EmailTemplates\\index1.html";
                            //StreamReader str = new StreamReader(FilePath);
                            //String MailText = str.ReadToEnd();
                            //str.Close();

                            int id = user.userid;

                            string MailText = $"<a href = 'https://localhost:44327/User/Activite/{id}'>Active Email</a>";

                            email emailu = new email(user.email, "Verify Email", MailText);

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.picformat = "Invalid Format";
                        }
                    }
                }

            }
            return View(userv);
        }

        
        public ActionResult Activite(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Login","User");
            }

            user user = db.users.Find(id);
            
            if(user.status == "blocked")
            {
                user.status = "active";
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                TempData["everi"] = true;
            }

            return RedirectToAction("Login","User");
        }

        public ActionResult EditStatus(int? id,string status)
        {
            if (id == null || status == null)
            {
                return RedirectToAction("UserList","User");
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return RedirectToAction("UserList", "User");
            }
            if (status == "blocked")
            {
                user.status = status;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserList", "User");
            }
            else if (status == "active")
            {
                user.status = status;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserList", "User");
            }
            else
            {
                return RedirectToAction("UserList", "User");
            }
        }


        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            useredit useredit = new useredit();
            AutoMapper.Mapper.Map(user, useredit);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(useredit);
        }


        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,fname,lname,gender,age,address,city,state,pincode,photo,email,username,password,status")] useredit useredit)
        {
            if (ModelState.IsValid)
            {
                user user = new user();

                user.userid = useredit.userid;
                user.fname = useredit.fname;
                user.lname = useredit.lname;
                user.gender = useredit.gender;
                user.age = useredit.age;
                user.address = useredit.address;
                user.city = useredit.city;
                user.state = useredit.state;
                user.pincode = useredit.pincode;
                user.photo = useredit.photo;
                user.email = useredit.email;
                user.username = useredit.username;
                user.password = useredit.password;
                user.status = useredit.status;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(useredit);
        }


        public ActionResult Reset(int? id)
        {
            if(Session["uid"] != null)
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "User");
                }

                ViewBag.uid = id;

                return View();
            }
            else
            {
                return RedirectToAction("Login","User");
            }
        }

        [HttpPost]
        public ActionResult Reset(checkpass checkpass,int? uid)
        {
            if (uid == null)
            {
                return RedirectToAction("Reset", "User");
            }

            if (ModelState.IsValid)
            {
                user user = db.users.Where(p => p.userid == uid).FirstOrDefault();
                if(user == null)
                {
                    return RedirectToAction("Reset", "User");
                }
                user.password = checkpass.password;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","User");
            }

            return View(checkpass);
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
