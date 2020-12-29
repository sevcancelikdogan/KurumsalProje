using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KurumsalProje.Models;
namespace KurumsalProje.Controllers
{
    public class AdminController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Admin
        //[Route("yonetimpaneli")]
        //[Route("yonetimpaneli")]
        public ActionResult Index()
        {
            ViewBag.BlogSay = db.Blogs.Count().ToString();
            ViewBag.HizmetSay = db.Hizmets.Count().ToString();
            ViewBag.KategoriSay = db.Kategoris.Count().ToString();
            ViewBag.YorumSay = db.Yorums.Count().ToString();
            ViewBag.YorumOnay = db.Yorums.Where(x => x.Onay == false).Count();
            return View();
        }
        //[Route("yonetimpaneli/giris")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var login = db.Admins.Where(x => x.Eposta == admin.Eposta).SingleOrDefault();
            if(login.Eposta==admin.Eposta && login.Sifre==Crypto.Hash(admin.Sifre,"MD5"))
            {
                Session["adminId"] = login.AdminId;
                Session["eposta"] = login.Eposta;
                Session["yetki"] = login.Yetki;
                return RedirectToAction("Index","Admin");
            }
            ViewBag.Uyari = "Kullanici adı yada şifre yanlış";
            return View();
        }
        public ActionResult Logout()
        {
            Session["adminId"] = null;
            Session["eposta"] = null;
            Session.Abandon();
            return RedirectToAction("Login","Admin");
        }
        public ActionResult Adminler()
        {
            return View(db.Admins.ToList());
        }
         
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Admin admin, string sifre, string eposta)
        {
            if(ModelState.IsValid)
            {
                admin.Sifre = Crypto.Hash(sifre, "MD5");
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }
        public ActionResult Edit(int id)
        {
            var a = db.Admins.Where(x => x.AdminId == id).SingleOrDefault();
            return View(a);
        }
        [HttpPost]
        public ActionResult Edit(int id, Admin admin, string sifre, string eposta)
        {
            if (ModelState.IsValid)
            {
                var a = db.Admins.Where(x => x.AdminId == id).SingleOrDefault();
                a.Sifre = Crypto.Hash(sifre, "MD5");
                a.Eposta = admin.Eposta;
                a.Yetki = admin.Yetki;
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var h = db.Admins.Find(id);
            if (h == null)
            {
                return HttpNotFound();
            }
            db.Admins.Remove(h);
            db.SaveChanges();
            return RedirectToAction("Adminler");
        }
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifremiUnuttum(string email)
        {
            var mail = db.Admins.Where(x => x.Eposta == email).SingleOrDefault();
            if ( mail != null)
            {
                Random rnd = new Random();
                int yenisifre = rnd.Next();

                Admin sifre = new Admin();
                mail.Sifre = Crypto.Hash(Convert.ToString(yenisifre), "MD5");
                db.SaveChanges();
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "sevcan.celikdogan9@gmail.com";
                WebMail.Password = "15112016....";
                WebMail.SmtpPort = 587;
                WebMail.Send(email,"Admin Panel Giriş Şifreniz", "Şifreniz :" + yenisifre);
                ViewBag.Uyari = "Şifreniz gönderilmiştir, Giriş yaptıktan sonra lütfen şifrenizi değiştiriniz!";
            }
            else
            {
                ViewBag.Uyari = "Hata Oluşmuştur Lütfen Tekrar Deneyiniz!";


            }
            return View();
        }
    }
}