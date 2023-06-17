using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MyFilesController : Controller
    {
        private demoEntities db = new demoEntities();

        // GET: MyFiles
        public ActionResult Index()
        {
            return View(db.MyFiles.ToList());
        }

        // GET: MyFiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyFile myFile = db.MyFiles.Find(id);
            if (myFile == null)
            {
                return HttpNotFound();
            }
            return View(myFile);
        }

        // GET: MyFiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ContentType,Data")] MyFile myFile)
        {
            if (ModelState.IsValid)
            {
                db.MyFiles.Add(myFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myFile);
        }

        // GET: MyFiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyFile myFile = db.MyFiles.Find(id);
            if (myFile == null)
            {
                return HttpNotFound();
            }
            return View(myFile);
        }

        // POST: MyFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ContentType,Data")] MyFile myFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myFile);
        }

        // GET: MyFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyFile myFile = db.MyFiles.Find(id);
            if (myFile == null)
            {
                return HttpNotFound();
            }
            return View(myFile);
        }

        // POST: MyFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyFile myFile = db.MyFiles.Find(id);
            db.MyFiles.Remove(myFile);
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
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);

                var myFile = new MyFile
                {
                    Name = file.FileName,
                    ContentType = file.ContentType,
                    Data = data
                };

                db.MyFiles.Add(myFile);
                db.SaveChanges();

                ViewBag.Message = "File uploaded successfully";
            }
            else
            {
                ViewBag.Message = "Please select a file";
            }
            return RedirectToAction("Index", "MyFiles");
        }
        public ActionResult Download(int id)
        {
            var myFile = db.MyFiles.Find(id);

            if (myFile != null)
            {
                return File(myFile.Data, myFile.ContentType, myFile.Name);
            }

            return HttpNotFound();
        }
    }
}
