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
    public class BlogController : Controller
    {
        private KurumsalDBEntities db = new KurumsalDBEntities();
        // GET: Blog
        public ActionResult Index()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return View(db.Blogs/*.Include("Kategori")*/.ToList().OrderByDescending(x=>x.BlogId));
        }
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategoris,"KategoriId","KategoriAd");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Blog blog, HttpPostedFileBase ResimURL)
        {
            if (ResimURL != null)
            {
                /* if (System.IO.File.Exists(Server.MapPath(k.LogoUrl)))
                 {
                     System.IO.File.Delete(Server.MapPath(k.LogoUrl));
                 }*/
                WebImage img = new WebImage(ResimURL.InputStream);
                FileInfo imgInfo = new FileInfo(ResimURL.FileName);

                string blogname = Guid.NewGuid().ToString() + imgInfo.Extension;
                img.Resize(600, 400);
                img.Save("~/Uploads/Blog/" + blogname);
                blog.ResimUrl = "/Uploads/Blog/" + blogname;
            }
            db.Blogs.Add(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if(id==null)
            {
                return HttpNotFound();
            }
            var b = db.Blogs.Where(x => x.BlogId == id).SingleOrDefault();
            if(b==null)
            {
                return HttpNotFound();
            }
            ViewBag.kategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAd",  b.KategoriId);
            return View(b);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Blog blog, HttpPostedFileBase ResimURL)
        {


            if (ModelState.IsValid)
            {
                var b = db.Blogs.Where(x => x.BlogId == id).SingleOrDefault();
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(b.ResimUrl)))
                    {
                        System.IO.File.Delete(Server.MapPath(b.ResimUrl));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imgInfo = new FileInfo(ResimURL.FileName);

                    string blogname = ResimURL.FileName + imgInfo.Extension;
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Blog/" + blogname);
                    b.ResimUrl = "/Uploads/Blog/" + blogname;
                }
                b.Baslik = blog.Baslik;
                b.Icerik = blog.Icerik;
                b.KategoriId = blog.KategoriId;
                db.SaveChanges();
                return RedirectToAction("index");

            }
            return View(blog);

        }
        //[HttpPost]
        public ActionResult Delete(int? id)
        {
           
            var b = db.Blogs.Find(id);
            if(b==null)
            {
                return HttpNotFound();
            }
            if(System.IO.File.Exists(Server.MapPath(b.ResimUrl)))
            {
                System.IO.File.Delete(Server.MapPath(b.ResimUrl));
            }
            db.Blogs.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}