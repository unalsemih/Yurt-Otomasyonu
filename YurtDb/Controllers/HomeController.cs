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
        static int sayfaYonlendirme = -1; // Default olarak -1 olacak..
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
               System.Diagnostics.Debug.WriteLine("Sayfa Adimi ilk : " + sayfaYonlendirme);
            if (sayfaYonlendirme != -1)
            {
                basvuruFormu.sayfaYonlendirme = sayfaYonlendirme;
                sayfaYonlendirme = -1;
            }
            else
                basvuruAdimiGuncelle();
                


                System.Diagnostics.Debug.WriteLine("Basvuru Sayfa Yönlendirme : " + sayfaYonlendirme);
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
        public ActionResult evrakUpload(int evrakTipiID, HttpPostedFileBase file)
        {
            string link="";
            System.Diagnostics.Debug.WriteLine(evrakTipiID);
          
                if (file.ContentLength > 0)
                {
                    var fileName = "";
                    string evrakAdi = context.evrakTipis.First(p => p.evrakTipiID == evrakTipiID).adi;
                    if (evrakAdi != null)
                        fileName = basvuruFormu.ogrenciBilgileri.ogrenciNo + "-" + basvuruFormu.ogrenciBilgileri.adi + "-" + basvuruFormu.ogrenciBilgileri.soyadi + "-" + evrakAdi;
                    else
                        fileName = basvuruFormu.ogrenciBilgileri.ogrenciNo + "-" + basvuruFormu.ogrenciBilgileri.adi + "-" + basvuruFormu.ogrenciBilgileri.soyadi + "-" + "--";
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName + "" + Path.GetExtension(file.FileName));
                    link = path;
                    file.SaveAs(path);
                }

                basvuruFormu.evrakBilgileri.ogrenciNo = basvuruFormu.ogrenciBilgileri.ogrenciNo;
                basvuruFormu.evrakBilgileri.link = link;
                basvuruFormu.evrakBilgileri.evrakTipiID = evrakTipiID;
                /// basvuruFormu.evrakBilgileri.donemTipiID = 0;
                /// 
                basvuruFormu.yuklenenEvraklar.Add(evrakTipiID);


            return Json(new { evrakTipiID = ""+evrakTipiID, kalanEvrakSayisi =basvuruFormu.evrakTipi.Count()-basvuruFormu.yuklenenEvraklar.Count() }, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        public ActionResult girisKontrol(BasvuruFormu basvuru)
        {
            basvuruFormu = new BasvuruFormu();
            basvuruFormu.basvuruAdimi = 0;

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
            basvuruFormu.odaFiyati = (double)context.odaFiyatlaris.First(x => x.odaTipiId == basvuru.odaBilgileri.odaTipiId && x.indirimId == basvuru.odaBilgileri.indirimId).fiyat;
                                           
            //Kontrol aşaması bittikten sonra


            basvuruFormu.evrakBilgileri = new evrakBilgileri();
            basvuruFormu.evrakTipi = context.evrakTipis.Where(p => p.donemTipiID == 1).ToList();
            basvuruFormu.yuklenenEvraklar = new List<int>();
            return RedirectToAction("Index");
        }


        public void basvuruAdimiGuncelle()
        {
            basvuruFormu.sayfaYonlendirme = -1;
            if (basvuruFormu.evrakBilgileri != null)
                basvuruFormu.basvuruAdimi = 3;
            else if (basvuruFormu.odaBilgileri != null)
            {
                if(basvuruFormu.basvuruAdimi == 1)
                OdaBilgileriniListele();
                basvuruFormu.basvuruAdimi = 2;

            }
            else if (basvuruFormu.ogrenciBilgileri != null)
                basvuruFormu.basvuruAdimi = 1;
            else
                basvuruFormu.basvuruAdimi = 0;
          
        }





        [HttpGet]
        public ActionResult getRooms()
        {
            // var odaFiyati = context.odaFiyatlari.Where(p => p.indirimId == 1);
            List<OdaKontenjan> odaKontenjanBilgileri = new List<OdaKontenjan>();
            List<odaTipiKontenjan> kontenjanBilgileri;
            if (basvuruFormu.ogrenciBilgileri.cinsiyet=="Erkek")
                kontenjanBilgileri = context.odaTipiKontenjans.Where(p => p.erkekKontenjan>0).ToList();//donemid kontrolü eklenmeil.
            else
                kontenjanBilgileri = context.odaTipiKontenjans.Where(p => p.kizKontenjan > 0).ToList();

            for (int i = 0; i < kontenjanBilgileri.Count(); i++)
            {
                //  int 
                //   odaTipi odaTipi = context.odaTipis.FirstOrDefault(p => p.odaTipiID == (kontenjanBilgileri[i].odaTipiID));
                odaTipi odaTipi = kontenjanBilgileri[i].odaTipi;
                OdaKontenjan odaKontenjan = new OdaKontenjan();
                odaKontenjan.adi = odaTipi.adi;
                odaKontenjan.donemTipiID = (int)odaTipi.donemTipiID;
                odaKontenjan.odaTipiID = (int)odaTipi.odaTipiID;
                if (basvuruFormu.ogrenciBilgileri.cinsiyet == "Erkek")
                    odaKontenjan.kontenjan = (int)kontenjanBilgileri[i].erkekKontenjan;
                else
                    odaKontenjan.kontenjan = (int)kontenjanBilgileri[i].kizKontenjan;
                odaKontenjanBilgileri.Add(odaKontenjan);
            }  
            

            var jsonData = Json(odaKontenjanBilgileri, JsonRequestBehavior.AllowGet);
            return jsonData;
        }


        public List<OdaKontenjan> getOdaKontenjanList()
        {
            // var odaFiyati = context.odaFiyatlari.Where(p => p.indirimId == 1);
            List<OdaKontenjan> odaKontenjanBilgileri = new List<OdaKontenjan>();
            List<odaTipiKontenjan> kontenjanBilgileri;
            if (basvuruFormu.ogrenciBilgileri.cinsiyet == "Erkek")
                kontenjanBilgileri = context.odaTipiKontenjans.Where(p => p.erkekKontenjan > 0).ToList();//donemid kontrolü eklenmeil.
            else
                kontenjanBilgileri = context.odaTipiKontenjans.Where(p => p.kizKontenjan > 0).ToList();

            for (int i = 0; i < kontenjanBilgileri.Count(); i++)
            {
                //  int 
                //   odaTipi odaTipi = context.odaTipis.FirstOrDefault(p => p.odaTipiID == (kontenjanBilgileri[i].odaTipiID));
                odaTipi odaTipi = kontenjanBilgileri[i].odaTipi;
                OdaKontenjan odaKontenjan = new OdaKontenjan();
                odaKontenjan.adi = odaTipi.adi;
                odaKontenjan.donemTipiID = (int)odaTipi.donemTipiID;
                odaKontenjan.odaTipiID = (int)odaTipi.odaTipiID;
                if (basvuruFormu.ogrenciBilgileri.cinsiyet == "Erkek")
                    odaKontenjan.kontenjan = (int)kontenjanBilgileri[i].erkekKontenjan;
                else
                    odaKontenjan.kontenjan = (int)kontenjanBilgileri[i].kizKontenjan;
                odaKontenjanBilgileri.Add(odaKontenjan);
            }
            
            return odaKontenjanBilgileri;
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


        public void OdaBilgileriniListele()
        {
            basvuruFormu.odaKontenjanList = getOdaKontenjanList();
         //   basvuruFormu.odaTipleri = new List<SelectListItem>();
         //   basvuruFormu.odaTipleri = context.odaTipis.Select(p => new SelectListItem { Value=p.odaTipiID.ToString(),Text=p.adi }).ToList();
            basvuruFormu.odaTipleri = basvuruFormu.odaKontenjanList.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.adi + " ("+a.kontenjan+")",
                    Value = a.odaTipiID.ToString()
                };
            });
            basvuruFormu.indirimler = context.indirimlers.Select(p => new SelectListItem { Value=p.indirimlerID.ToString(), Text=p.aciklama}).ToList();
        }

    }
}