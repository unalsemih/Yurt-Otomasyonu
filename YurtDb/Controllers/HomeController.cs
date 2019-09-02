using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YurtDb.Models;
using YurtDb.DB;
using Newtonsoft.Json;

namespace YurtDb.Controllers
{
    public class HomeController : Controller
    {
        DB.YurtDBEntities context;
        static BasvuruFormu basvuruFormu;
        int basvuruAdimi=0;
        public HomeController()
        {
            context = new YurtDBEntities();
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
            System.Diagnostics.Debug.WriteLine("Basvuru Adimi ilk : " + basvuruFormu.basvuruAdimi);
            basvuruAdimiGuncelle();
            System.Diagnostics.Debug.WriteLine("Basvuru Adimi Son : " + basvuruFormu.basvuruAdimi);
            return View(basvuruFormu);
            
        }

        [HttpPost]
        public ActionResult girisKontrol(BasvuruFormu basvuru)
        {
            //Kullanıcı TC NO girdiğinde...
            System.Diagnostics.Debug.WriteLine("giris"+basvuru.tcNo);
            basvuruFormu.tcNo = basvuru.tcNo;

            //Tc 
            if (basvuruFormu.tcNo != 0)
            {
                DB.ogrenci ogrenci = context.ogrenci.First(o => o.tcNo == basvuruFormu.tcNo);
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

            System.Diagnostics.Debug.WriteLine("odakontrol");


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
            var odaTipleri = context.odaTipi
                            .Select(p => new { p.odaTipiID, p.adi});
            
            var jsonData = Json(odaTipleri, JsonRequestBehavior.AllowGet);
            return jsonData;
        }



        [HttpGet]
        public ActionResult getPrices(int id)
        {
             var odaFiyatBilgileri = context.odaFiyatlari.Where(x=>x.odaTipiId == id)
                            .Select(p => new { p.indirimId, p.aciklama, p.fiyat });
           

            var jsonData = Json(odaFiyatBilgileri, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        
        [HttpGet]
        public ActionResult indirimler()
        {
            var indirimler = context.indirimler.Select(p => new { p.indirimlerID, p.aciklama, p.indirimOrani});
            var jsonData = Json(indirimler, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        [HttpGet]
        public ActionResult getPrice(int odaTipiID,int indirimID)
        {
            var odaFiyatBilgisi = context.odaFiyatlari.Where(x => x.odaTipiId == odaTipiID && x.indirimId == indirimID)
                           .Select(p => new { p.indirimId, p.aciklama, p.fiyat });


            var jsonData = Json(odaFiyatBilgisi, JsonRequestBehavior.AllowGet);
            return jsonData;
        }



    }
}