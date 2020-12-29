using KurumsalProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Net.Mail;
using PagedList;
using PagedList.Mvc;

namespace KurumsalProje.Controllers
{
    public class HomeController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Home
        [Route("")]
        [Route("AnaSayfa")]
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();

            //var hz = db.Hizmets.ToList().OrderByDescending(x => x.HizmetId);
            //using (var context = new KurumsalDBEntities())
            //{

            //    var hizmet = context.Hizmets.ToList();
            //    return View(hizmet);
            //}
            ViewModel viewModel = new ViewModel();
            viewModel.Hizmet = db.Hizmets.ToList();
            viewModel.Hakkimizda = db.Hakkimizdas.ToList();
            viewModel.Iletisim = db.Iletisims.ToList();
            return View(viewModel);


        }

        public ActionResult SliderPartial()
        {
            var sp = db.Sliders.ToList().OrderByDescending(x=>x.SliderId);
            return View(sp);
        }
        [Route("Hizmet")]
        public ActionResult Hizmet()
        {
            var hz = db.Hizmets.ToList().OrderByDescending(x=>x.HizmetId);
            return View(hz);
        }
        [Route("BizeUlas")]
        public ActionResult Iletisim()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();

            return View(db.Iletisims.SingleOrDefault());

        }
        [HttpPost]
        public ActionResult Iletisim(string adsoyad=null, string email=null, string konu=null, string mesaj=null)
        {
            if(adsoyad!=null && email!=null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "sevcan.celikdogan9@gmail.com";
                WebMail.Password = "15112016....";
                WebMail.SmtpPort = 587;
                WebMail.Send("sevcan.celikdogan9@gmail.com", konu, email, mesaj);
                ViewBag.Uyari = "Mesajınız gönderilmiştir!";
            }
            else
            {
                ViewBag.Uyari = "Hata Oluşmuştur Lütfen Tekrar Deneyiniz!";


            }
            return View();
            //var iletisim = db.Iletisims.ToList().OrderByDescending(x=>x.IletisimId);
            //return View(iletisim);
        }
        [Route("Hakkimizda")]
        public ActionResult Hakkimizda()
        {
            var hz = db.Hakkimizdas.ToList().OrderByDescending(x => x.HakkimizdaId);
            return View(hz);
        }
        [Route("BlogPost")]
        public ActionResult Blog(int Sayfa = 1)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();

            return View(db.Blogs.Include("Kategori").OrderByDescending(x => x.BlogId).ToPagedList(Sayfa,5));

        }
        [Route("BlogPost/{KatgoriAd}/{id:int}")]
        public ActionResult KategoriBlog(int id, int Sayfa=1)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();

            var b = db.Blogs.Include("Kategori").OrderByDescending(x=>x.BlogId).Where(x => x.Kategori.KategoriId == id).ToPagedList(Sayfa,5);
            return View(b);
        }
        public ActionResult BlogKategoriPartial()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();

            //db.Configuration.LazyLoadingEnabled = false;
            return PartialView(db.Kategoris.Include("Blogs").ToList().OrderBy(x => x.KategoriAd));
        }
        public ActionResult BlogKayitPartial()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            return PartialView(db.Blogs.ToList().OrderByDescending(x => x.BlogId).Take(5));
        }
        [Route("BlogPost/{Baslik}-{id:int}")]
        public ActionResult BlogDetay(int id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();

            var b = db.Blogs.Include("Kategori").Include("Yorums").Where(x => x.BlogId == id).SingleOrDefault();
            //db.Configuration.LazyLoadingEnabled = false;
            return View(b);
        }
        [HttpPost]
        public JsonResult YorumYap(string adsoyad, string eposta,string icerik,int blogid)
        {

            if(icerik==null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Yorums.Add(new Yorum { AdSoyad = adsoyad, Eposta = eposta, İcerik = icerik, BlogId = blogid, Onay=false });
            var sonuc = db.SaveChanges();
            if (sonuc==1)
            {
                return Json(new { sonuc = "1", mesaj = "Yorumunuz Eklendi Kontrol Edildikten Sonra Onaylanacaktır" });

            }
            else
            {
                return Json(new { sonuc = "0", mesaj = "Yorumunuz Eklenemedi" });

            }
            //Response.Redirect("/Home/BlogDetay/" + blogid);
            //return View();
        }
        public ActionResult YorumDetay(int id, int Sayfa = 1)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();

            var b = db.Yorums.OrderByDescending(x => x.YorumId).Where(x => x.YorumId == id).ToPagedList(Sayfa, 5);
            return View(b);
        }
        [HttpPost]
        public ActionResult arama(string p)
        {
            var blog = from d in db.Blogs select d;
            if(!string.IsNullOrEmpty(p))
            {
                blog = blog.Where(b => b.Baslik.Contains(p));
            }
            return View(blog.ToList());
        }
        public ActionResult Vizyon()
        {
            return View();
        }
        public ActionResult Misyon()
        {
            return View();
        }

    }
}