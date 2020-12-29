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
    public class SliderController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Slider
        public ActionResult Index()
        {
            var slider = db.Sliders.ToList();
            return View(slider);
        }
        // GET: Hakkimizda/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Slider slider, HttpPostedFileBase ResimURL)
        {


            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {

                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imgInfo = new FileInfo(ResimURL.FileName);

                    string logoname = Guid.NewGuid().ToString() + imgInfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Slider/" + logoname);
                    slider.ResimURL = "/Uploads/Slider/" + logoname;
                }
                db.Sliders.Add(slider);
                // k.LogoUrl = kimlik.LogoUrl;
                db.SaveChanges();
                return RedirectToAction("index");

            }
            return View(slider);

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Uyari = "Güncellenecek Hizmet Bulunamadı";
            }
            var slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int? id, Slider slider, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var h = db.Sliders.Where(x => x.SliderId == id).SingleOrDefault();
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(h.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(h.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string slidername = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Kimlik/" + slidername);
                    h.ResimURL = "/Uploads/Kimlik/" + slidername;

                }
                //db.Entry(hizmet).State = EntityState.Modified;
                h.Baslik = slider.Baslik;
                h.Aciklama = slider.Aciklama;
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
            var h = db.Sliders.Find(id);
            if (h == null)
            {
                return HttpNotFound();
            }
            db.Sliders.Remove(h);
            db.SaveChanges();
            return RedirectToAction("index");
        }

    }
}