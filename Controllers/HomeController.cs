using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YurtDb.Models;
using YurtDb.DB;
using Newtonsoft.Json;
using System.IO;
using System.Data.Entity.Core.Objects;

namespace YurtDb.Controllers
{
    public class HomeController : Controller
    {
        DB.YurtDBEntities context;
     //   static BasvuruFormu basvuruFormu;
        int basvuruAdimi=0;
        static int sayfaYonlendirme = -1; // Default olarak -1 olacak..
        public HomeController()
        {
            context = new YurtDBEntities();
        }
        

        [HttpGet]
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("deneme");
            if (Session["basvuruFormu"] == null)
            {
                Session["basvuruFormu"] = new BasvuruFormu();
                ((BasvuruFormu)(Session["basvuruFormu"])).basvuruAdimi = 0;
            }
            System.Diagnostics.Debug.WriteLine("Sayfa Adimi ilk : " + sayfaYonlendirme);
            if (sayfaYonlendirme != -1)
            {
                ((BasvuruFormu)(Session["basvuruFormu"])).sayfaYonlendirme = sayfaYonlendirme;
                sayfaYonlendirme = -1;
            }
            else
                basvuruAdimiGuncelle();



            System.Diagnostics.Debug.WriteLine("Basvuru Sayfa Yönlendirme : " + sayfaYonlendirme);
            return View(((BasvuruFormu)(Session["basvuruFormu"])));

        }


        [HttpGet]
        public ActionResult BasvuruBilgileri()
        {

            return View(((BasvuruFormu)(Session["basvuruFormu"])));

        }




        [HttpGet]
        public ActionResult changeForm(int number)
        {
            sayfaYonlendirme = number;
            if (number == 2)
                OdaBilgileriniListele();
            System.Diagnostics.Debug.WriteLine("nuumber" + sayfaYonlendirme);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult islemleriGeriAl(int numAdim)
        {
            try
            {
                var basvuru = context.basvurular.Find(((BasvuruFormu)(Session["basvuruFormu"])).basvuruTabloBilgisi.basvuruID);
                if (basvuru != null)
                    basvuru.durum = -1;
                context.SaveChanges();
            }
            catch
            {
                numAdim = 0;
            }
            
            
            if (numAdim ==0)
            {
                Session.Remove("basvuruFormu");
                RedirectToAction("Index");
            }
            else if (numAdim == 1)
            {
                ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileri = null;
                ((BasvuruFormu)(Session["basvuruFormu"])).odaBilgileri = null;
               // ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri = new ogrenci();
                basvuruAdimiGuncelle();

               
            }
            else if(numAdim == 2)
            {
                ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileri = null;
                ((BasvuruFormu)(Session["basvuruFormu"])).odaBilgileri = new odaFiyatlari();
                basvuruAdimiGuncelle();

                return RedirectToAction("changeForm", new { number = numAdim });
            }

            //return RedirectToAction("changeForm", new { number = numAdim });
            return RedirectToAction("Index");
        }


        public FileResult evrakGoster(int evrakTipiID)
        {
            // byte[] fileBytes = System.IO.File.ReadAllBytes(@""+ Server.MapPath("~/App-Data/uploads/"+ ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList.Find(x => x.evrakTipiID == evrakTipiID).evrakAdi));
            byte[] fileBytes = System.IO.File.ReadAllBytes(((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList.Find(x => x.evrakTipiID == evrakTipiID).link);
            string fileName = ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList.Find(x => x.evrakTipiID == evrakTipiID).evrakAdi;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        /*    public ActionResult evrakGoster(int evrakTipiID)
            {
               // String file = Server.MapPath("~/App-Data/uploads/" + ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList.Find(x=>x.evrakTipiID==evrakTipiID).evrakAdi);
              //  String mimeType = MimeMapping.GetMimeMapping(((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList.Find(x => x.evrakTipiID == evrakTipiID).link);

              //  byte[] stream = System.IO.File.ReadAllBytes(file);
              //  return File(stream, mimeType);
            }
            */




        [HttpPost]
        public ActionResult ogrenciBelgesiUpload(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                //var fileName = Path.GetFileName(file.FileName);
                var fileName = ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.tcNo + ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.adi + ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.soyadi + "OgrenciBelgesi";
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult evrakUpload(int evrakTipiID, HttpPostedFileBase file)
        {
            if (((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList == null)
                ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList = new List<evrakBilgileri>();
            
            string error = "";
            string link = "";
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = "";
                    string evrakAdi = context.evrakTipi.First(p => p.evrakTipiID == evrakTipiID).adi;
                    if (evrakAdi != null)
                        fileName = ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.ogrenciNo + "-" 
                        + ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.adi + "-" + 
                        ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.soyadi + "-" + evrakAdi;
                    else
                        fileName = ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.ogrenciNo + "-" 
                        + ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.adi + "-" + ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.soyadi + "-" + "--";
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName + "" + Path.GetExtension(file.FileName));
                    link = path;
                    file.SaveAs(path);

                    evrakBilgileri evrakBilgileri = new evrakBilgileri();
                    evrakBilgileri.ogrenciNo = ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.ogrenciNo;
                    evrakBilgileri.link = link;
                    evrakBilgileri.evrakAdi = fileName + Path.GetExtension(file.FileName);
                    evrakBilgileri.evrakTipiID = evrakTipiID;
                    ((BasvuruFormu)(Session["basvuruFormu"])).yuklenenEvraklar.Add(evrakTipiID);
                    ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList.Add(evrakBilgileri);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return Json(new { evrakTipiID = "" + evrakTipiID, kalanEvrakSayisi = ((BasvuruFormu)(Session["basvuruFormu"])).evrakTipi.Count() - ((BasvuruFormu)(Session["basvuruFormu"])).yuklenenEvraklar.Count(), error = "" + error }, JsonRequestBehavior.AllowGet);
        }










        [HttpPost]
        public ActionResult kayitTamamla(BasvuruFormu basvuru)
        {
            ogrenci ogrenci = ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri;
            context.ogrenci.Add(ogrenci);

            Kayıt kayit = new Kayıt();
            kayit.donemTipiID = 1;//dönem tipi id daha sonra düzenlenecek
            kayit.odaFiyatlariID = ((BasvuruFormu)(Session["basvuruFormu"])).odaFiyatlariId;
            kayit.ogrenciNo = ogrenci.ogrenciNo;
            context.Kayıt.Add(kayit);
            for (int i = 0; i < ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList.Count(); i++)
                context.evrakBilgileri.Add(((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileriList[i]);
            var basvurularBilgi = context.basvurular.Find(((BasvuruFormu)(Session["basvuruFormu"])).basvuruTabloBilgisi.basvuruID);
            if (basvurularBilgi != null && basvurularBilgi.durum == 0)
            {
                basvurularBilgi.durum = 1;
                context.SaveChanges();
                return RedirectToAction("BasvuruBilgileri");

            }
            else
            {
                //basvurular tablosundaki bilgi null dönerse ya da durum = 0 değilse yani 30 dk süresini aştıysa
                return RedirectToAction("Index");
            }
        }



        [HttpPost]
        public ActionResult girisKontrol(BasvuruFormu basvuru)
        {
            Session["basvuruFormu"] = new BasvuruFormu();
            ((BasvuruFormu)(Session["basvuruFormu"])).basvuruAdimi = 0;

            //Kullanıcı TC NO girdiğinde...
            System.Diagnostics.Debug.WriteLine("giris" + basvuru.tcNo + "egitimDurumu:" + basvuru.egitimDurumu);
            ((BasvuruFormu)(Session["basvuruFormu"])).tcNo = basvuru.tcNo;
            ((BasvuruFormu)(Session["basvuruFormu"])).egitimDurumu = basvuru.egitimDurumu;//Egitim Durumu ve tc no alındı. statik değişkene...

            if (((BasvuruFormu)(Session["basvuruFormu"])).tcNo != 0)//TCNO girildiyse
            {
                System.Diagnostics.Debug.WriteLine("tcno" + ((BasvuruFormu)(Session["basvuruFormu"])).tcNo);
                DB.ogrenciApi ogrenci = context.ogrenciApi.FirstOrDefault(o => o.tcNo == basvuru.tcNo && o.egitimDurumu == basvuru.egitimDurumu);
                if (ogrenci != null)
                {   //apiden veriler alınacak ve burada statiğe geçirilecek...
                    ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri = new DB.ogrenci();
                    ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.adi = ogrenci.adi;
                    ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.soyadi = ogrenci.soyadi;
                    ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.ogrenciNo = ogrenci.ogrenciNo;
                    ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.cinsiyet = ogrenci.cinsiyet;
                    ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.tcNo = ogrenci.tcNo;

                    var ogrenciVarMi = context.ogrenci.Find(((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.ogrenciNo = ogrenci.ogrenciNo);
                    if (ogrenciVarMi != null)
                    {
                       // ViewBag.message = "Bu öğrenci numarası ile kayıt yapılmıştır!";
                        ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri = null;
                    }
                }
            }

            return RedirectToAction("Index");

        }




        [HttpPost]
        public ActionResult ogrenciBilgileriKontrol(BasvuruFormu basvuru)
        {
            //Ogrenci Bilgileri kısmında devam et butonu clicklendiğinde buraya gelinecek ... 

            System.Diagnostics.Debug.WriteLine("kontrol");


            //Kontrol aşaması bittikten sonra
            ((BasvuruFormu)(Session["basvuruFormu"])).odaBilgileri = new odaFiyatlari();
            if (((BasvuruFormu)(Session["basvuruFormu"])).odaBilgileri == null)
                System.Diagnostics.Debug.WriteLine("nulll");
            else
                System.Diagnostics.Debug.WriteLine("not null");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult odaBilgileriKontrol(BasvuruFormu basvuru)
        {
            //oda Bilgileri kısmında devam et butonu clicklendiğinde buraya gelinecek ... 

            System.Diagnostics.Debug.WriteLine("odakontrol + odatiipId" + basvuru.odaBilgileri.odaTipiId);
            System.Diagnostics.Debug.WriteLine("odakontrol + indirimId" + basvuru.odaBilgileri.indirimId);
            ((BasvuruFormu)(Session["basvuruFormu"])).odaBilgileri.odaTipiId = basvuru.odaBilgileri.odaTipiId;
            ((BasvuruFormu)(Session["basvuruFormu"])).odaBilgileri.indirimId = basvuru.odaBilgileri.indirimId;
            ((BasvuruFormu)(Session["basvuruFormu"])).odaBilgileri.indirimId = basvuru.odaBilgileri.indirimId;

            ((BasvuruFormu)(Session["basvuruFormu"])).odaFiyati = (double)context.odaFiyatlari.First(x => x.odaTipiId == basvuru.odaBilgileri.odaTipiId && x.indirimId == basvuru.odaBilgileri.indirimId).fiyat;
            ((BasvuruFormu)(Session["basvuruFormu"])).odaFiyatlariId = context.odaFiyatlari.First(x => x.odaTipiId == basvuru.odaBilgileri.odaTipiId && x.indirimId == basvuru.odaBilgileri.indirimId).odaFiyatlariID;
            //Kontrol aşaması bittikten sonra
            ////
            ///Başvur tablosuna ekleme yapılacak...
            ///System.Data.Objects.ObjectParameter identityParameter = 
            ObjectParameter islemDurumu =new ObjectParameter("islemDurumu", 0);
            System.Diagnostics.Debug.WriteLine("basvur procedure = ogrNo:"+ ((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.ogrenciNo + 
                " odaTipiId:" + (((BasvuruFormu)Session["basvuruFormu"])).odaBilgileri.odaTipiId+" cinsiyet:" + (((BasvuruFormu)Session["basvuruFormu"])).ogrenciBilgileri.cinsiyet+
                " islemDurumu:" + islemDurumu);

            context.basvur(((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.ogrenciNo,
                           (((BasvuruFormu)Session["basvuruFormu"])).odaBilgileri.odaTipiId,
                           (((BasvuruFormu)Session["basvuruFormu"])).ogrenciBilgileri.cinsiyet,islemDurumu);
            System.Diagnostics.Debug.WriteLine("islemDurumu : " + islemDurumu.Value);

            /////
            if (Convert.ToInt32(islemDurumu.Value) >0)
            {
                int procedureId = Convert.ToInt32(islemDurumu.Value);
                ((BasvuruFormu)(Session["basvuruFormu"])).basvuruTabloBilgisi = context.basvurular.FirstOrDefault(p => p.basvuruID == procedureId);
                ((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileri = new evrakBilgileri();
                ((BasvuruFormu)(Session["basvuruFormu"])).evrakTipi = context.evrakTipi.Where(p => p.donemTipiID == 1).ToList();
                ((BasvuruFormu)(Session["basvuruFormu"])).yuklenenEvraklar = new List<int>();
            }
            return RedirectToAction("Index");
        }


        public void basvuruAdimiGuncelle()
        {
            ((BasvuruFormu)(Session["basvuruFormu"])).sayfaYonlendirme = -1;
            if (((BasvuruFormu)(Session["basvuruFormu"])).evrakBilgileri != null)
                ((BasvuruFormu)(Session["basvuruFormu"])).basvuruAdimi = 3;
            else if (((BasvuruFormu)(Session["basvuruFormu"])).odaBilgileri != null)
            {
                if (((BasvuruFormu)(Session["basvuruFormu"])).basvuruAdimi == 1)
                    OdaBilgileriniListele();
                ((BasvuruFormu)(Session["basvuruFormu"])).basvuruAdimi = 2;

            }
            else if (((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri != null)
                ((BasvuruFormu)(Session["basvuruFormu"])).basvuruAdimi = 1;
            else
                ((BasvuruFormu)(Session["basvuruFormu"])).basvuruAdimi = 0;


            

        }


        /*


        [HttpGet]
        public ActionResult getRooms()
        {





            // var odaFiyati = context.odaFiyatlari.Where(p => p.indirimId == 1);
            List<OdaKontenjan> odaKontenjanBilgileri = new List<OdaKontenjan>();
            List<odaTipiKontenjan> kontenjanBilgileri;
            if (((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.cinsiyet == "Erkek")
                kontenjanBilgileri = context.odaTipiKontenjan.Where(p => p.erkekKontenjan > 0).ToList();//donemid kontrolü eklenmeil.
            else
                kontenjanBilgileri = context.odaTipiKontenjan.Where(p => p.kizKontenjan > 0).ToList();



            for (int i = 0; i < kontenjanBilgileri.Count(); i++)
            {
                //  int 
                //   odaTipi odaTipi = context.odaTipis.FirstOrDefault(p => p.odaTipiID == (kontenjanBilgileri[i].odaTipiID));
                odaTipi odaTipi = kontenjanBilgileri[i].odaTipi;
                OdaKontenjan odaKontenjan = new OdaKontenjan();
                odaKontenjan.adi = odaTipi.adi;
                odaKontenjan.donemTipiID = (int)odaTipi.donemTipiID;
                odaKontenjan.odaTipiID = (int)odaTipi.odaTipiID;
                if (((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.cinsiyet == "Erkek")
                    odaKontenjan.kontenjan = (int)kontenjanBilgileri[i].erkekKontenjan;
                else
                    odaKontenjan.kontenjan = (int)kontenjanBilgileri[i].kizKontenjan;
                odaKontenjanBilgileri.Add(odaKontenjan);
            }


            var jsonData = Json(odaKontenjanBilgileri, JsonRequestBehavior.AllowGet);
            return jsonData;
        }
        */

        public List<OdaKontenjan> getOdaKontenjanList()
        {
            // var odaFiyati = context.odaFiyatlari.Where(p => p.indirimId == 1);
            List<OdaKontenjan> odaKontenjanBilgileri = new List<OdaKontenjan>();
            List<odaTipiKontenjan> kontenjanBilgileri;
            if (((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.cinsiyet == "Erkek")
                kontenjanBilgileri = context.odaTipiKontenjan.Where(p => p.erkekKontenjan > 0).ToList();//donemid kontrolü eklenmeil.
            else
                kontenjanBilgileri = context.odaTipiKontenjan.Where(p => p.kizKontenjan > 0).ToList();

            for (int i = 0; i < kontenjanBilgileri.Count(); i++)
            {
                //  int 
                //   odaTipi odaTipi = context.odaTipis.FirstOrDefault(p => p.odaTipiID == (kontenjanBilgileri[i].odaTipiID));
                odaTipi odaTipi = kontenjanBilgileri[i].odaTipi;
                OdaKontenjan odaKontenjan = new OdaKontenjan();
                odaKontenjan.adi = odaTipi.adi;
                odaKontenjan.donemTipiID = (int)odaTipi.donemTipiID;
                odaKontenjan.odaTipiID = (int)odaTipi.odaTipiID;
                if (((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.cinsiyet == "Erkek")
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
            var odaFiyatBilgileri = context.odaFiyatlari.Where(x => x.odaTipiId == id)
                           .Select(p => new { p.indirimId, p.aciklama, p.fiyat });


            var jsonData = Json(odaFiyatBilgileri, JsonRequestBehavior.AllowGet);
            return jsonData;
        }


        [HttpGet]
        public ActionResult indirimler()
        {
            var indirimler = context.indirimler.Select(p => new { p.indirimlerID, p.aciklama, p.indirimOrani });
            var jsonData = Json(indirimler, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        [HttpGet]
        public ActionResult getPrice(int odaTipiID, int indirimID)
        {
            var odaFiyatBilgisi = context.odaFiyatlari.Where(x => x.odaTipiId == odaTipiID && x.indirimId == indirimID)
                           .Select(p => new { p.indirimId, p.aciklama, p.fiyat });


            var jsonData = Json(odaFiyatBilgisi, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        [HttpGet]
        public ActionResult tcOgrenciSorgulama(long tcNo)
        {

            System.Diagnostics.Debug.WriteLine("" + tcNo);
            var ogrenci = context.ogrenciApi.Where(x => x.tcNo == tcNo)
                           .Select(p => new { p.egitimDurumu, p.ogrenciNo });


            var jsonData = Json(ogrenci, JsonRequestBehavior.AllowGet);
            return jsonData;
        }


        public void OdaBilgileriniListele()
        {
          //   ((BasvuruFormu)(Session["basvuruFormu"])).odaKontenjanList = getOdaKontenjanList();
            //   basvuruFormu.odaTipleri = new List<SelectListItem>();
            //   basvuruFormu.odaTipleri = context.odaTipis.Select(p => new SelectListItem { Value=p.odaTipiID.ToString(),Text=p.adi }).ToList();

            List<SelectListItem> odaBilgilerilistesi = new List<SelectListItem>();

            var kontenjanBilgileri = context.kontenjanSayisi(((BasvuruFormu)(Session["basvuruFormu"])).ogrenciBilgileri.cinsiyet);
            foreach(var item in kontenjanBilgileri)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = item.adi + " (" + item.bosKontenjan + " Boş )";
                selectListItem.Value = item.odaTipiID.ToString();
                odaBilgilerilistesi.Add(selectListItem);
            }

            /*((BasvuruFormu)(Session["basvuruFormu"])).odaTipleri = ((BasvuruFormu)(Session["basvuruFormu"])).odaKontenjanList.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.adi + " (" + a.kontenjan + ")",
                    Value = a.odaTipiID.ToString()
                };
            });*/
            ((BasvuruFormu)(Session["basvuruFormu"])).odaTipleri = odaBilgilerilistesi;
            ((BasvuruFormu)(Session["basvuruFormu"])).indirimler = context.indirimler.Select(p => new SelectListItem { Value = p.indirimlerID.ToString(), Text = p.aciklama }).ToList();
        }


        [HttpGet]
        public ActionResult ogrenciGetir(long tcNo)
        {

            System.Diagnostics.Debug.WriteLine("" + tcNo);
            var ogrenci = context.ogrenciApi.Where(x => x.tcNo == tcNo);
            var jsonData = Json(ogrenci, JsonRequestBehavior.AllowGet);
            return jsonData;
        }
    }
}