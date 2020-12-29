using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KurumsalProje.Models;

namespace KurumsalProje.Controllers
{
    public class IletisimController : Controller
    {
        private KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Iletisim
        public ActionResult Index()
        {
            return View(db.Iletisims.ToList());
        }

        // GET: Iletisim/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iletisim iletisim = db.Iletisims.Find(id);
            if (iletisim == null)
            {
                return HttpNotFound();
            }
            return View(iletisim);
        }

        // GET: Iletisim/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Iletisim/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Iletisim iletisim)
        {
            try
            {
                db.Iletisims.Add(iletisim);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Uyari = "Güncellenecek Hizmet Bulunamadı";
            }
            var iletisim = db.Iletisims.Find(id);
            if (iletisim == null)
            {
                return HttpNotFound();
            }
            return View(iletisim);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int? id, Iletisim iletisim, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var h = db.Iletisims.Where(x => x.IletisimId == id).SingleOrDefault();
                
                //db.Entry(hizmet).State = EntityState.Modified;
                h.Adres = iletisim.Adres;
                h.Tel = iletisim.Tel;
                h.Fax = iletisim.Fax;
                h.Whatsapp = iletisim.Whatsapp;
                h.Facebook = iletisim.Facebook;
                h.Twitter = iletisim.Twitter;
                h.Instagram = iletisim.Instagram;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View();
        }
        // GET: Iletisim/Delete/5
        /* public ActionResult Delete(int? id)
         {
             if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             Iletisim iletisim = db.Iletisims.Find(id);
             if (iletisim == null)
             {
                 return HttpNotFound();
             }
             return View(iletisim);
         }

         // POST: Iletisim/Delete/5
         [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public ActionResult DeleteConfirmed(int id)
         {
             Iletisim iletisim = db.Iletisims.Find(id);
             db.Iletisims.Remove(iletisim);
             db.SaveChanges();
             return RedirectToAction("Index");
         }*/
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var h = db.Iletisims.Find(id);
            if (h == null)
            {
                return HttpNotFound();
            }
            db.Iletisims.Remove(h);
            db.SaveChanges();
            return RedirectToAction("index");
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
