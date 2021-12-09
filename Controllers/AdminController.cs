﻿using System;
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
    public class AdminController : Controller
    {
        private huntdbEntities db = new huntdbEntities();

        // GET: Admin
        public ActionResult Index()
        {
            if(Session["aid"] != null)
            {
                return View(db.admins.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                admin admin = db.admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Admin/Create
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

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "aid,aname,age,gender,email,username,password")] adminv adminv)
        {
            if (Session["aid"] != null)
            {
                if (ModelState.IsValid)
                {
                    admin email = db.admins.Where(p => p.email.Equals(adminv.email)).FirstOrDefault();
                    admin username = db.admins.Where(p => p.username.Equals(adminv.username)).FirstOrDefault();
                    

                    if (email != null)
                    {
                        ViewBag.emailerr = "Email already in use";
                    }
                    else if(username != null)
                    {
                        ViewBag.usererr = "UserName already in use";
                    }
                    else
                    {
                        admin admin = new admin();
                        AutoMapper.Mapper.Map(adminv,adminv);
                        db.admins.Add(admin);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                return View(adminv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                admin admin = db.admins.Find(id);
                
                adminv adminv = new adminv();
                AutoMapper.Mapper.Map(admin,adminv);

                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(adminv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "aid,aname,age,gender,email,username,password")] adminv adminv)
        {
            if (Session["aid"] != null)
            {
                if (ModelState.IsValid)
                {
                    admin admin = new admin();
                    AutoMapper.Mapper.Map(adminv,admin);
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(adminv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                admin admin = db.admins.Find(id);
                if (admin != null)
                {
                    db.admins.Remove(admin);
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
