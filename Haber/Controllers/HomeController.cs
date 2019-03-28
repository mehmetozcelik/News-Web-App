using Haber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Haber.App_Class;

namespace Haber.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        HaberDataContext ctx = new HaberDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult AnaSliderWidget()
        {
            ViewBag.Anahaber = ctx.Habers.OrderByDescending(x=>x.id).ToList().Take(5);
            
            return PartialView();
        }

        public PartialViewResult SagSlider()
        {
            return PartialView();
        }

        public PartialViewResult SagSliderEncokTiklanan()
        {
            ViewBag.haber = ctx.Habers.Where(x => x.EklenmeTarihi == DateTime.Now).OrderByDescending(x => x.OkunmaSayisi).Take(5).ToList();
            return PartialView();
        }

     
        public PartialViewResult AltKategoriSlider()
        {
            
            List<AnaKategori> anakategori = ctx.AnaKategoris.OrderBy(x=>x.id).ToList();
            List<AltKategori> altkategori = ctx.AltKategoris.OrderBy(x => x.id).ToList();
            List<Haber.Models.Haber> haberler = ctx.Habers.OrderByDescending(x => x.id).ToList();
            ViewBag.anakategoriler = anakategori;
            ViewBag.altkategoriler = altkategori;
            ViewBag.haberler = haberler;
            return PartialView();
        }
        

        public PartialViewResult UstMenu()
        {
            List<AnaKategori> anakategori = ctx.AnaKategoris.ToList();
            List<AltKategori> altkategori = ctx.AltKategoris.ToList();

            List<Haber.Models.Haber> haberler = ctx.Habers.OrderByDescending(x => x.id).ToList();
            ViewBag.Populer = ctx.Habers.OrderByDescending(x => x.OkunmaSayisi).Take(5).ToList();
            ViewBag.anakategoriler = anakategori;
            ViewBag.altkategoriler = altkategori;
            ViewBag.haberler = haberler;
            return PartialView();
        }


     

        public ActionResult HaberDetay(int id)
        {
            Haber.Models.Haber hbr = ctx.Habers.FirstOrDefault(x => x.id == id);
            hbr.OkunmaSayisi = hbr.OkunmaSayisi + 1; 
            ctx.SubmitChanges();
            ViewBag.yorumlar = ctx.Yorums.Where(x => x.HaberID == id && x.Onay==true).Take(50).ToList();

            return View(hbr);
        }

        public ActionResult KategoriDetay(int id)
        {
            AnaKategori k = ctx.AnaKategoris.FirstOrDefault(x => x.id == id);
            
           List<Haber.Models.Haber> hbr = ctx.Habers.Where(x => x.AltKategori.AnaKategoriID == id).ToList();
            ViewBag.haberler = hbr;
            return View(k);
        }
            
            [HttpPost]
        public ActionResult YorumYap(int id,string comment ,string author)
        {
            Yorum yrm = new Yorum();
            yrm.HaberID = id;
            yrm.Icerik = comment;
            if(author==null || author=="")
            {
                yrm.Adi = "Gizli Kullanıcı";
            }
            else
            {
                yrm.Adi = author;
            }
            yrm.EklenmeTarihi = DateTime.Now;
            yrm.Onay = false;
            ctx.Yorums.InsertOnSubmit(yrm);
            ctx.SubmitChanges();
            return RedirectToAction("HaberDetay", "Home", new { @id = id });
        }



        [HttpPost]
        public ActionResult AramaSonuclari(ara aranan)
        {
            if (!String.IsNullOrEmpty(aranan.ArananString))
            {
                List<Haber.Models.Haber> Sonuclar = ctx.Habers.Where(x => x.Icerik.Contains(aranan.ArananString) || x.Baslik.Contains(aranan.ArananString) ||x.KisaAciklama.Contains(aranan.ArananString)).ToList();
                ViewBag.sonuclar = Sonuclar;
            }
            else
            {
            }
            return View(aranan);
        }
    }
}