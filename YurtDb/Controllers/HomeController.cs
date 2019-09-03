using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YurtDb.Models;
using YurtDb.DB;
using Newtonsoft.Json;
using System.IO;

namespace YurtDb.Controllers
{
    public class HomeController : Controller
    {
        DB.YurtDatabaseEntities context;
        static BasvuruFormu basvuruFormu;
        int basvuruAdimi=0;
        int sayfaYonlendirme = -1; // Default olarak -1 olacak..
        public HomeController()
        {
            context = new YurtDatabaseEntities();
        }


        [HttpGet]
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("deneme");
            if (basvuruFormu == null)
            {
                basvuruFormu = new BasvuruFormu();
                basvuruFormu.basvuruAdimi = 0;
            }
            //   System.Diagnostics.Debug.WriteLine("Basvuru Adimi ilk : " + basvuruFormu.basvuruAdimi);
            if (sayfaYonlendirme != -1)
            {
                basvuruFormu.sayfaYonlendirme = sayfaYonlendirme;
                sayfaYonlendirme = -1;
            }
            else
                basvuruAdimiGuncelle();
                


            //    System.Diagnostics.Debug.WriteLine("Basvuru Adimi Son : " + basvuruFormu.basvuruAdimi);
            return View(basvuruFormu);
            
        }





        [HttpGet]
        public ActionResult changeForm(int number)
        {
            sayfaYonlendirme = number;
            System.Diagnostics.Debug.WriteLine("nuumber" + sayfaYonlendirme);
            return RedirectToAction("Index");
        }





        [HttpPost]
        public ActionResult ogrenciBelgesiUpload(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                //var fileName = Path.GetFileName(file.FileName);
                var fileName = basvuruFormu.ogrenciBilgileri.tcNo + basvuruFormu.ogrenciBilgileri.adi + basvuruFormu.ogrenciBilgileri.soyadi + "OgrenciBelgesi";
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }







        [HttpPost]
        public ActionResult girisKontrol(BasvuruFormu basvuru)
        {
            
            //Kullanıcı TC NO girdiğinde...
            System.Diagnostics.Debug.WriteLine("giris"+basvuru.tcNo + "egitimDurumu:"+basvuru.egitimDurumu);
            basvuruFormu.tcNo = basvuru.tcNo;
            basvuruFormu.egitimDurumu = basvuru.egitimDurumu;//Egitim Durumu ve tc no alındı. statik değişkene...
            
            if (basvuruFormu.tcNo != 0)
            {
                DB.ogrenci ogrenci = context.ogrencis.First(o => o.tcNo == basvuruFormu.tcNo);
                if (ogrenci != null)
                    basvuruFormu.ogrenciBilgileri = ogrenci;
            }
           
            return RedirectToAction("Index");
            
        }




        [HttpPost]
        public ActionResult ogrenciBilgileriKontrol(BasvuruFormu basvuru)
        {
            //Ogrenci Bilgileri kısmında devam et butonu clicklendiğinde buraya gelinecek ... 

            System.Diagnostics.Debug.WriteLine("kontrol");


            //Kontrol aşaması bittikten sonra
            basvuruFormu.odaBilgileri = new odaFiyatlari();
            if(basvuruFormu.odaBilgileri ==null)
                System.Diagnostics.Debug.WriteLine("nulll");
            else
                System.Diagnostics.Debug.WriteLine("not null");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult odaBilgileriKontrol(BasvuruFormu basvuru)
        {
            //Ogrenci Bilgileri kısmında devam et butonu clicklendiğinde buraya gelinecek ... 

            System.Diagnostics.Debug.WriteLine("odakontrol + odatiipId"+basvuru.odaBilgileri.odaTipiId);
            System.Diagnostics.Debug.WriteLine("odakontrol + indirimId" + basvuru.odaBilgileri.indirimId);
            basvuruFormu.odaBilgileri.odaTipiId = basvuru.odaBilgileri.odaTipiId;
            basvuruFormu.odaBilgileri.indirimId = basvuru.odaBilgileri.indirimId;
            //Kontrol aşaması bittikten sonra
            basvuruFormu.evrakBilgileri = new evrakBilgileri();
           

            return RedirectToAction("Index");
        }


        public void basvuruAdimiGuncelle()
        {
            if (basvuruFormu.evrakBilgileri != null)
                basvuruFormu.basvuruAdimi = 3;
            else if (basvuruFormu.odaBilgileri != null) 
                basvuruFormu.basvuruAdimi = 2;
            else if (basvuruFormu.ogrenciBilgileri != null)
                basvuruFormu.basvuruAdimi = 1;
            else
                basvuruFormu.basvuruAdimi = 0;

        }


        [HttpGet]
        public ActionResult getRooms()
        {
           // var odaFiyati = context.odaFiyatlari.Where(p => p.indirimId == 1);
            var odaTipleri = context.odaTipis
                            .Select(p => new { p.odaTipiID, p.adi});
            
            var jsonData = Json(odaTipleri, JsonRequestBehavior.AllowGet);
            return jsonData;
        }



        [HttpGet]
        public ActionResult getPrices(int id)
        {
             var odaFiyatBilgileri = context.odaFiyatlaris.Where(x=>x.odaTipiId == id)
                            .Select(p => new { p.indirimId, p.aciklama, p.fiyat });
           

            var jsonData = Json(odaFiyatBilgileri, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        
        [HttpGet]
        public ActionResult indirimler()
        {
            var indirimler = context.indirimlers.Select(p => new { p.indirimlerID, p.aciklama, p.indirimOrani});
            var jsonData = Json(indirimler, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        [HttpGet]
        public ActionResult getPrice(int odaTipiID,int indirimID)
        {
            var odaFiyatBilgisi = context.odaFiyatlaris.Where(x => x.odaTipiId == odaTipiID && x.indirimId == indirimID)
                           .Select(p => new { p.indirimId, p.aciklama, p.fiyat });


            var jsonData = Json(odaFiyatBilgisi, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        [HttpGet]
        public ActionResult tcOgrenciSorgulama(long tcNo)
        {

            System.Diagnostics.Debug.WriteLine(""+tcNo);
            var ogrenci = context.ogrencis.Where(x => x.tcNo == tcNo)
                           .Select(p => new { p.egitimDurumu,p.ogrenciNo});


            var jsonData = Json(ogrenci, JsonRequestBehavior.AllowGet);
            return jsonData;
        }


    }
}