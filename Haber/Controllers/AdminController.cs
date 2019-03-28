using Haber.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Haber.App_Class;
using System.Web.Security;
using System.Web.Helpers;

namespace Haber.Controllers
{
    [Authorize] 
    public class AdminController : Controller
    {
        // GET: Admin
        HaberDataContext db = new HaberDataContext();
                        
        public ActionResult Index()                                 
        {
            ViewBag.toplamokunma = db.Habers.Sum(x => x.OkunmaSayisi);
            ViewBag.toplamyorum = db.Yorums.Count();
            ViewBag.toplamhaber = db.Habers.Count();
            ViewBag.tplamyazar = db.aspnet_Memberships.Count();
            return View();
        }

      
        public ActionResult AnaKategoriListesi()
        {
            ViewBag.Kategoriler = db.AnaKategoris.ToList();
            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult AnaKategoriSil(int id)
        {
            string mesaj = "Silmek İstediğiniz Kategoriye Ait alt kategori Bulunmaktadır.";
            AnaKategori k = db.AnaKategoris.FirstOrDefault(x=>x.id==id);
            db.AnaKategoris.DeleteOnSubmit(k);
            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                TempData["a"]=mesaj;
                return RedirectToAction("AnaKategoriListesi");
            }
       
            return RedirectToAction("AnaKategoriListesi");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AnaKategoriGuncelle(int id)
        {
            AnaKategori kat = db.AnaKategoris.FirstOrDefault(x => x.id == id);
            return View(kat);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AnaKategoriGuncelle(AnaKategori ka)
        {

            AnaKategori k = db.AnaKategoris.FirstOrDefault(x => x.id == ka.id);
            k.Adi = ka.Adi;
            k.Aciklama = ka.Aciklama;
            k.Renk = ka.Renk;
            db.SubmitChanges();

            return RedirectToAction("AnaKategoriListesi");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AnaKategoriEkle()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AnaKategoriEkle(AnaKategori kat)
        {
            db.AnaKategoris.InsertOnSubmit(kat);
            db.SubmitChanges();
            return RedirectToAction("AnaKategoriListesi");
        }


        public ActionResult AltKategoriListesi()
        {
            ViewBag.Kategoriler = db.AltKategoris.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AltKategoriSil(int id)
        {
            AltKategori k = db.AltKategoris.FirstOrDefault(x => x.id == id);
            db.AltKategoris.DeleteOnSubmit(k);
            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                TempData["b"] = "Silmek istediğiniz alt kategoriye ait haber bulunmaktadır.";
                return RedirectToAction("AltKategoriListesi");
            }
      
            return RedirectToAction("AltKategoriListesi");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AltKategoriGuncelle(int id)
        {
            AltKategori kat = db.AltKategoris.FirstOrDefault(x => x.id == id);
            ViewBag.AnaKategoriler = db.AnaKategoris.ToList();
            return View(kat);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AltKategoriGuncelle(AltKategori ka)
        {

            AltKategori k = db.AltKategoris.FirstOrDefault(x => x.id == ka.id);
            k.Adi = ka.Adi;
            k.Aciklama = ka.Aciklama;
            k.AnaKategoriID = ka.AnaKategoriID;
            db.SubmitChanges();

            return RedirectToAction("AltKategoriListesi");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult altKategoriEkle()
        {
            ViewBag.anakategoriler = db.AnaKategoris.ToList();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AltKategoriEkle(AltKategori kat)
        {
            db.AltKategoris.InsertOnSubmit(kat);
            db.SubmitChanges();
            return RedirectToAction("AltKategoriListesi");
        }


        public ActionResult HaberListesi()
        {
            ViewBag.Haberlistesi = db.Habers.OrderByDescending(x=>x.id).ToList();
            return View();
        }

        

        [Authorize(Roles = "Admin,Editör,Yazar")]
        public ActionResult HaberSil(int id)
        {
            Haber.Models.Haber k = db.Habers.FirstOrDefault(x => x.id == id);
            List<Yorum> hbryrm = db.Yorums.Where(x => x.HaberID == k.id).ToList();
            foreach (Yorum item in hbryrm)
            {
                db.Yorums.DeleteOnSubmit(item);
            }
            db.Habers.DeleteOnSubmit(k);
            db.SubmitChanges();
            return RedirectToAction("HaberListesi");
        }
        [Authorize(Roles = "Admin,Editör,Yazar")]
        public ActionResult HaberGüncelle(int id)
        {
            Haber.Models.Haber kat = db.Habers.FirstOrDefault(x => x.id == id);
            ViewBag.haberler = db.Habers.ToList();
            ViewBag.altkategoriler = db.AltKategoris.ToList();
            return View(kat);
        }

        [Authorize(Roles = "Admin,Editör,Yazar")]
        [HttpPost]
        public ActionResult HaberGüncelle(Haber.Models.Haber ha)
        {
            Haber.Models.Haber k = db.Habers.FirstOrDefault(x => x.id == ha.id);
            k.Baslik = ha.Baslik;
            k.KisaAciklama = ha.KisaAciklama;
            k.Icerik = ha.Icerik;
            k.KategoriID = ha.KategoriID;
            db.SubmitChanges();

            return RedirectToAction("HaberListesi");
        }
        [Authorize(Roles = "Admin,Editör,Yazar")]
        public ActionResult HaberEkle()
        {
            ViewBag.altkategoriler = db.AltKategoris.ToList();
            return View();
        }
        [Authorize(Roles = "Admin,Editör,Yazar")]
        [HttpPost]
        public ActionResult HaberEkle(Haber.Models.Haber ha,HttpPostedFileBase file)
        {

            int resimId = 0;

            if (file!=null)
            {
                Image img = Image.FromStream(file.InputStream);

                string url = "/Content/Admin/img/Haber/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                img.Save(Server.MapPath(url));
          

                Haber.Models.Haber hbr = new Models.Haber();
                hbr.Baslik = ha.Baslik;
                hbr.KisaAciklama = ha.KisaAciklama;
                hbr.Icerik = ha.Icerik;
                hbr.EklenmeTarihi = DateTime.Now;
                hbr.KategoriID = ha.KategoriID;
                hbr.OkunmaSayisi = 0;
                hbr.Yazar = User.Identity.Name;
                db.Habers.InsertOnSubmit(hbr);
                db.SubmitChanges();

                if (hbr.id!=null)
                {
                    Resim rsm = new Resim();
                    rsm.Url = url;
                  
                    
                    db.Resims.InsertOnSubmit(rsm);
                    db.SubmitChanges();
                    if (rsm.id!=null)
                    {
                        hbr.ResımID = rsm.id;
                    }
                    db.SubmitChanges();

                }

            }
            return RedirectToAction("HaberListesi");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciEkle()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult KullaniciEkle(Kullanici k)
        {
            MembershipCreateStatus durum; //yeni bir kullanıcı oluşturma girişiminin durumunu gösterir. CreateUser işleminin durumuna göre değer döndürür.
            Membership.CreateUser(k.KulaniciAdi, k.Sifre, k.Mail, "kimsin", "benim", true, out durum);
            string mesaj = "";
            switch (durum)
            {
                case MembershipCreateStatus.Success:
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    mesaj += "Gecersiz kullanıcı adı girildi.";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    mesaj += "Gecersiz parola girildi.";
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    mesaj += "Gecersiz gizli soru girildi.";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    mesaj += "Gecersiz gizli cevap girildi.";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    mesaj += "Gecersiz mail girildi.";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    mesaj += "Kullanılmış kullanıcı girildi.";
                    break;
                case MembershipCreateStatus.DuplicateEmail:
                    mesaj += "Kullanılmış Mail Adresi girildi.";
                    break;
                case MembershipCreateStatus.UserRejected:
                    mesaj += "Bu kullanıcı engellenmiştir.";
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                    mesaj += "Gecersiz kullanıcı key hatası girildi.";
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    mesaj += "Kullanılmış kullanıcı key girildi.";
                    break;
                case MembershipCreateStatus.ProviderError:
                    mesaj += "Üye yöneticisi hatası";
                    break;
                default:
                    break;
            }

            if (durum==MembershipCreateStatus.Success)
            {
                aspnet_Membership ms = db.aspnet_Memberships.FirstOrDefault(x => x.aspnet_User.UserName == k.KulaniciAdi);
                ms.Name = k.Name;
                ms.Surname = k.Surname;
                db.SubmitChanges();
                return RedirectToAction("KullaniciListesi");
            }
            else
            {
                ViewBag.mesaj = mesaj;
            }

            return View();
        }

        public ActionResult KullaniciListesi()
        {
            ViewBag.kullanicilar = db.aspnet_Memberships.ToList();
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciSil(Guid id)
        {
          
            aspnet_Membership us = db.aspnet_Memberships.FirstOrDefault(x => x.UserId == id);

            db.aspnet_Users.DeleteOnSubmit(us.aspnet_User);
            db.aspnet_Memberships.DeleteOnSubmit(us);
            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                TempData["f"] = "Silmek istediğiniz kullanıcı bir profile dahildir. Bu kullanıcıyı silmek için veritabanı yöneticinize başvurunuz.";
            }
        

           return RedirectToAction("KullaniciListesi");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciGuncelle(Guid id)
        {
            aspnet_Membership us = db.aspnet_Memberships.FirstOrDefault(x => x.UserId == id);
            ViewBag.kid = us.UserId;
            return View(us);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult KullaniciGuncelle(Kullanici u)
        {
            aspnet_Membership ap = db.aspnet_Memberships.FirstOrDefault(x => x.UserId == u.id);

            ap.Name = u.Name;
            ap.Surname = u.Surname;
            ap.Email = u.Mail;
            ap.aspnet_User.UserName = u.KulaniciAdi;
            ap.aspnet_User.LoweredUserName = u.KulaniciAdi;
            db.SubmitChanges();
            return RedirectToAction("KullaniciListesi");
        }

        [AllowAnonymous]
        public ActionResult GirisYap()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GirisYap(Kullanici k)
        {
            bool sonuc = Membership.ValidateUser(k.KulaniciAdi, k.Sifre);

            if (sonuc)
            {
                FormsAuthentication.RedirectFromLoginPage(k.KulaniciAdi, true);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["c"] = "Kullanıcı adınız veya şifreniz hatalı";
                return View();
            }
           
        }

        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("GirisYap");
        }


        public ActionResult Profiller()
        {
            List<string> TumRoller = Roles.GetAllRoles().ToList();
            return View(TumRoller);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ProfilEkle()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ProfilEkle(string roladi)
        {
            Roles.CreateRole(roladi);
            return RedirectToAction("Profiller");
        }

        [Authorize(Roles = "Admin")]
     
        public ActionResult ProfilAta()
        {
            ViewBag.Kullanicilar = db.aspnet_Memberships.ToList();
            ViewBag.Roller = db.aspnet_Roles.ToList();
            return View();
        }
         [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ProfilAta(string kullaniciadi,string roladi)
        {
            Roles.AddUserToRole(kullaniciadi,roladi);
            return RedirectToAction("Profiller");
        }



        //public string UyeRolleri(string kullaniciadi)
        //{
        //    List<string> list = Roles.GetRolesForUser(kullaniciadi).ToList();

        //    string roller = "";
        //    foreach (string item in list)
        //    {
        //        roller += item + " \n";
        //    }

        //    return roller;
        //}

        public ActionResult YorumListesi()
        {
            ViewBag.TumYorumlar = db.Yorums.OrderByDescending(x=>x.id).ToList();
            return View();
        }

        [Authorize(Roles = "Admin,Editör")]
        public ActionResult YorumSil(int id)
        {
            Yorum y = db.Yorums.FirstOrDefault(x => x.id == id);
            db.Yorums.DeleteOnSubmit(y);
            db.SubmitChanges();
            return RedirectToAction("YorumListesi");
        }
        [Authorize(Roles = "Admin,Editör")]
        public ActionResult YorumOnay(int id)
        {
            Yorum y = db.Yorums.FirstOrDefault(x => x.id == id);
            y.Onay = true;
            db.SubmitChanges();
            return RedirectToAction("YorumListesi");
        }
        [Authorize(Roles = "Admin,Editör")]
        public ActionResult YorumOnayGeri(int id)
        {
            Yorum y = db.Yorums.FirstOrDefault(x => x.id == id);
            y.Onay = false;
            db.SubmitChanges();
            return RedirectToAction("YorumListesi");
        }


        public  ActionResult SifreDegistir()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifreDegistir(aspnet_User user,string EskiSifre,string YeniSifre)
        {
            bool x= Membership.GetUser(user.UserName).ChangePassword(EskiSifre, YeniSifre);
            if (x)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("GirisYap");
            }
            else
            {
                TempData["d"]= "Eski şifrenizi yanlış girdiniz !";
                return View();
            }
        
        }

    }
}