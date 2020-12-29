using KurumsalProje.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalProje.Controllers
{
    public class KimlikController : Controller
    {
        // GET: Kimlik
        KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Hizmet
        public ActionResult Index()
        {

            var kimlik = db.Kimliks.ToList();
            return View(kimlik);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Kimlik kimlik, HttpPostedFileBase LogoURL)
        {


            if (ModelState.IsValid)
            {
                if (LogoURL != null)
                {

                    WebImage img = new WebImage(LogoURL.InputStream);
                    FileInfo imgInfo = new FileInfo(LogoURL.FileName);

                    string logoname = Guid.NewGuid().ToString() + imgInfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Kimlik/" + logoname);
                    kimlik.LogoUrl = "/Uploads/Kimlik/" + logoname;
                }
                db.Kimliks.Add(kimlik);
                // k.LogoUrl = kimlik.LogoUrl;
                db.SaveChanges();
                return RedirectToAction("index");

            }
            return View(kimlik);

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Uyari = "Güncellenecek Hizmet Bulunamadı";
            }
            var kimlik = db.Kimliks.Find(id);
            if (kimlik == null)
            {
                return HttpNotFound();
            }
            return View(kimlik);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int? id, Kimlik kimlik, HttpPostedFileBase LogoURL)
        {
            if (ModelState.IsValid)
            {
                var h = db.Kimliks.Where(x => x.KimlikId == id).SingleOrDefault();
                if (LogoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(h.LogoUrl)))
                    {
                        System.IO.File.Delete(Server.MapPath(h.LogoUrl));
                    }
                    WebImage img = new WebImage(LogoURL.InputStream);
                    FileInfo imginfo = new FileInfo(LogoURL.FileName);

                    string logoname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Kimlik/" + logoname);
                    h.LogoUrl = "/Uploads/Kimlik/" + logoname;

                }
                //db.Entry(hizmet).State = EntityState.Modified;
                h.Title = kimlik.Title;
                h.Keywords = kimlik.Keywords;
                h.Description = kimlik.Description;
                h.Unvan = kimlik.Unvan;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var h = db.Kimliks.Find(id);
            if (h == null)
            {
                return HttpNotFound();
            }
            db.Kimliks.Remove(h);
            db.SaveChanges();
            return RedirectToAction("index");
        }

    }
}