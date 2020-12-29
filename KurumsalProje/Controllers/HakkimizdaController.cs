using KurumsalProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KurumsalProje.Controllers
{
    
    public class HakkimizdaController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Hakkimizda
        public ActionResult Index()
        {
            var hakkimizda = db.Hakkimizdas.ToList();
            return View(hakkimizda);
        }

        // GET: Hakkimizda/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Hakkimizda/Create
        public ActionResult Create()
        {
            var hakkimizda = new Hakkimizda();
            return View(hakkimizda);
        }

        // POST: Hakkimizda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Hakkimizda Hakkimizda)
        {
            try
            {
                db.Hakkimizdas.Add(Hakkimizda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
        // GET: Hakkimizda/Edit/5
        public ActionResult Edit(int? id)
        {
            var hakkimizda = db.Hakkimizdas.Where(x => x.HakkimizdaId == id).SingleOrDefault();

            return View(hakkimizda);



        }

        // POST: Hakkimizda/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Hakkimizda hakkimizda)
        {
            if (ModelState.IsValid)
            {
                var k = db.Hakkimizdas.Where(x => x.HakkimizdaId == id).SingleOrDefault();
                
                
                k.Aciklama = hakkimizda.Aciklama;
              
                db.SaveChanges();
                return RedirectToAction("index");

            }
            return View(hakkimizda);
        }

        // GET: Hakkimizda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hakkimizda hakkimizda  = db.Hakkimizdas.Find(id);
            if (hakkimizda == null)
            {
                return HttpNotFound();
            }
            return View(hakkimizda);
        }

        // POST: Hakkimizda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hakkimizda hakkimizda = db.Hakkimizdas.Find(id);
            db.Hakkimizdas.Remove(hakkimizda);
            db.SaveChanges();
            return RedirectToAction("Index");
        
        }
    }
}
