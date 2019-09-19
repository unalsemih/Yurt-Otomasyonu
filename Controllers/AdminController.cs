using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YurtDb.Models;
using YurtDb.Models.AdminModels;

namespace YurtDb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        DB.YurtDBEntities context;

        public AdminController()
        {
            context = new DB.YurtDBEntities();
        }
        public ActionResult Index()
        {
            if (Session["basvuruFormu"] == null)
            {
                Session["donemBilgileri"] = new DonemOdaEvrakBilgileri();
            }
            return View((DonemOdaEvrakBilgileri)(Session["donemBilgileri"]));
        }


       

        [HttpPost]
        public ActionResult donemEkleme(DonemOdaEvrakBilgileri donemModel)
        {
            try
            {
                var donemTipi = new DB.donemTipi();
                donemTipi.adi = donemModel.donemTipi.adi;
                donemTipi = context.donemTipi.Add(donemTipi);
                var donem = new DB.Donem();
                donem.donemAdi = donemModel.donemBilgisi.donemAdi;
                donem.baslangicTarihi = donemModel.donemBilgisi.baslangicTarihi;
                donem.bitisTarihi = donemModel.donemBilgisi.bitisTarihi;
                donem.donemTipiID = donemTipi.donemTipiID;
                context.Donem.Add(donem);
                context.SaveChanges();
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult donemGuncelle(DonemBilgileri donemModel)
        {
            try
            {
                 var donem = context.Donem.SingleOrDefault(p=>p.id==donemModel.donemId);
                if (donem != null)
                {

                    donem.donemAdi = donemModel.donemAdi;
                    donem.baslangicTarihi = donemModel.baslangicTarihi;
                    donem.bitisTarihi = donemModel.bitisTarihi;
                    var donemTipi = context.donemTipi.SingleOrDefault(p=>p.donemTipiID==donem.donemTipiID);
                    if(donemTipi!=null)
                    {
                        donemTipi.adi = donemModel.donemTipiAdi;
                    }
         
                    context.SaveChanges();
                }
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult donemArsivle(int donemId)
        {
            try
            {
                var donem = context.Donem.SingleOrDefault(p => p.id == donemId);
                if (donem != null)
                {

                    donem.aktiflik = 0;
                    donem.arsiv = 1;
                    var donemTipi = context.donemTipi.SingleOrDefault(p => p.donemTipiID == donem.donemTipiID);
                    context.SaveChanges();
                }
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        public ActionResult odaTipiEkle(OdaBilgileriModel odaTipi)
        {

            try { 
                    System.Diagnostics.Debug.WriteLine("odaTipiId : " + odaTipi.odaTipiId.ToString());
                    System.Diagnostics.Debug.WriteLine("odaTipiKontenjanId : " + odaTipi.odaTipiKontenjanId.ToString());
                    System.Diagnostics.Debug.WriteLine("adi  : " + odaTipi.odaAdi);
                    System.Diagnostics.Debug.WriteLine("erkekKOntenjan : " + odaTipi.erkekKontenjan.ToString());
                    System.Diagnostics.Debug.WriteLine("kizKOntenjan : " + odaTipi.kizKontenjan.ToString());
                var yeniOdaTipi = new DB.odaTipi();
                yeniOdaTipi.adi = odaTipi.odaAdi;
                yeniOdaTipi.donemTipiID = odaTipi.donemTipiId;
                yeniOdaTipi = context.odaTipi.Add(yeniOdaTipi);
                context.SaveChanges();
                var odaTipiKontenjan = new DB.odaTipiKontenjan();
                odaTipiKontenjan.erkekKontenjan = odaTipi.erkekKontenjan;
                odaTipiKontenjan.kizKontenjan = odaTipi.kizKontenjan;
                odaTipiKontenjan.donemTipiID= odaTipi.donemTipiId;
                odaTipiKontenjan.odaTipiID= yeniOdaTipi.odaTipiID;
                odaTipiKontenjan.toplam = odaTipi.erkekKontenjan+ odaTipi.kizKontenjan;
                context.odaTipiKontenjan.Add(odaTipiKontenjan);
                context.SaveChanges();


                     
                return Json(new { durum = "true",odaTipiId=yeniOdaTipi.odaTipiID,odaTipiKontenjanId=odaTipiKontenjan.odaTipiKontenjanID }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult odaTipiEkleme(OdaBilgileriModel[] odaTipleri)
        {

            try
            {
                foreach (var item in odaTipleri)
                {
                    System.Diagnostics.Debug.WriteLine("odaTipiId : " + item.odaTipiId.ToString());
                    System.Diagnostics.Debug.WriteLine("odaTipiKontenjanId : " + item.odaTipiKontenjanId.ToString());
                    System.Diagnostics.Debug.WriteLine("adi  : " + item.odaAdi);
                    System.Diagnostics.Debug.WriteLine("erkekKOntenjan : " + item.erkekKontenjan.ToString());
                    System.Diagnostics.Debug.WriteLine("kizKOntenjan : " + item.kizKontenjan.ToString());
                    if (item.odaTipiId == -1)
                    {
                        var odaTipi = new DB.odaTipi();
                        var odaTipiKontenjan = new DB.odaTipiKontenjan();
                        odaTipi.adi = item.odaAdi;
                        odaTipi.donemTipiID = item.donemTipiId;
                        odaTipi = context.odaTipi.Add(odaTipi);
                        odaTipiKontenjan.erkekKontenjan = item.erkekKontenjan;
                        odaTipiKontenjan.kizKontenjan = item.kizKontenjan;
                        odaTipiKontenjan.toplam = item.erkekKontenjan + item.kizKontenjan;
                        odaTipiKontenjan.donemTipiID = item.donemTipiId;
                        odaTipiKontenjan.odaTipiID = odaTipi.odaTipiID;
                        context.odaTipiKontenjan.Add(odaTipiKontenjan);
                        context.SaveChanges();
                    }
                    else
                    {
                        var odaTipi = context.odaTipi.SingleOrDefault(p => p.odaTipiID == item.odaTipiId);
                        if (odaTipi != null)
                        {
                            odaTipi.adi = item.odaAdi;
                        }
                        var odaTipiKontenjan = context.odaTipiKontenjan.SingleOrDefault(p => p.odaTipiKontenjanID == item.odaTipiKontenjanId);
                        if (odaTipiKontenjan != null)
                        {
                            odaTipiKontenjan.erkekKontenjan = item.erkekKontenjan;
                            odaTipiKontenjan.kizKontenjan = item.kizKontenjan;
                            odaTipiKontenjan.toplam = item.erkekKontenjan + item.kizKontenjan;
                        }
                        context.SaveChanges();
                    }

                }
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }


        //evrakTipleri Ekleme
        [HttpPost]
        public ActionResult evrakTipiEkleme(EvrakTipleriModel[] evrakTipleri)
        {

            try
            {
                foreach (var item in evrakTipleri)
                {
                    System.Diagnostics.Debug.WriteLine("EvrakTipiAdi : " + item.evrakTipiAdi.ToString());
                    System.Diagnostics.Debug.WriteLine("evrakTipiId: " + item.evrakTipiId.ToString());
                    System.Diagnostics.Debug.WriteLine("ZOrunluluk  : " + item.zorunluluk);
                    if (item.evrakTipiId == -1)
                    {
                        var evrakTipi = new DB.evrakTipi();
                        evrakTipi.adi = item.evrakTipiAdi;
                        evrakTipi.zorunluluk = int.Parse(item.zorunluluk);
                        evrakTipi.donemTipiID = item.donemTipiId;
                        context.evrakTipi.Add(evrakTipi);
                    }
                    else
                    {
                        var evrakTipi = context.evrakTipi.SingleOrDefault(p => p.evrakTipiID == item.evrakTipiId);
                        if (evrakTipi != null)
                        {

                            evrakTipi.adi = item.evrakTipiAdi;
                            evrakTipi.zorunluluk = int.Parse(item.zorunluluk);
                            evrakTipi.donemTipiID = item.donemTipiId;

                        }


                    }
                    context.SaveChanges();
                }
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }





        [HttpGet]
        public ActionResult donemBilgisiGetir(int donemId)
        {
            var donemBilgileri = (from p in context.donemTipi
                                  join e in context.Donem
                                  on p.donemTipiID equals e.donemTipiID
                                  where e.id==donemId
                                  select new
                                  {
                                      donemTipiID = p.donemTipiID,
                                      adi = p.adi,
                                      donemID = e.id,
                                      aktiflik = e.aktiflik,
                                      donemAdi = e.donemAdi,
                                      baslangicTarihi = e.baslangicTarihi,
                                      bitisTarihi = e.bitisTarihi,
                                  }).ToList();





            var jsonData = Json(donemBilgileri, JsonRequestBehavior.AllowGet);
            return jsonData;


        }
        [HttpGet]
        public ActionResult donemBilgileriGetir()
        {
            var donemBilgileri = (from p in context.donemTipi
                                  join e in context.Donem
                                  on p.donemTipiID equals e.donemTipiID
                                  where e.arsiv!=1
                                  select new
                                  {
                                      donemTipiID = p.donemTipiID,
                                      adi = p.adi,
                                      donemID = e.id,
                                      aktiflik = e.aktiflik,
                                      donemAdi = e.donemAdi,
                                      baslangicTarihi = e.baslangicTarihi,
                                      bitisTarihi = e.bitisTarihi,
                                  }).ToList();





            var jsonData = Json(donemBilgileri, JsonRequestBehavior.AllowGet);
            return jsonData;


        }
        [HttpGet]
        public ActionResult odaBilgileriGetir(int donemTipiID)
        {
            var odaBilgileri = (from p in context.odaTipi
                                join e in context.odaTipiKontenjan
                                on p.odaTipiID equals e.odaTipiID
                                where p.donemTipiID == donemTipiID && e.donemTipiID == donemTipiID
                                select new
                                {
                                    donemTipiID = p.donemTipiID,
                                    odaTipiID = p.odaTipiID,
                                    odaTipiKontenjanId = e.odaTipiKontenjanID,
                                    erkekKontenjan = e.erkekKontenjan,
                                    kizKontenjan = e.kizKontenjan,
                                    odaAdi = p.adi,
                                }).ToList();





            var jsonData = Json(odaBilgileri, JsonRequestBehavior.AllowGet);
            return jsonData;


        }


        [HttpGet]
        public ActionResult evrakTipleriGetir(int donemTipiID)
        {
            var evrakTipleri = (from p in context.evrakTipi
                                where p.donemTipiID == donemTipiID
                                select new
                                {
                                    evrakTipiID = p.evrakTipiID,
                                    evrakTipiAdi = p.adi,
                                    zorunluluk = p.zorunluluk
                                }).ToList();




            var jsonData = Json(evrakTipleri, JsonRequestBehavior.AllowGet);
            return jsonData;


        }
      


        [HttpGet]
        public ActionResult odaTipiSil(int odaTipiKontenjanId)
        {

            try
            {
                var odaTipiKontenjan = context.odaTipiKontenjan.SingleOrDefault(p => p.odaTipiKontenjanID == odaTipiKontenjanId);
                if (odaTipiKontenjan != null)
                {
                    int odaTipiIdBilgisi = (int)odaTipiKontenjan.odaTipiID;
                    context.odaTipiKontenjan.Remove(odaTipiKontenjan);
                    var odaTipi = context.odaTipi.SingleOrDefault(p => p.odaTipiID == odaTipiIdBilgisi);
                    if (odaTipi != null)
                        context.odaTipi.Remove(odaTipi);
                }
                context.SaveChanges();
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult evrakTipiSil(int evrakTipiId)
        {

            try
            {
                var evrakTipi = context.evrakTipi.SingleOrDefault(p => p.evrakTipiID== evrakTipiId);
                if (evrakTipi != null)
                     context.evrakTipi.Remove(evrakTipi);
                   
                context.SaveChanges();
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult donemAktifYap(int donemId)
        {

            try
            {
                using (var db = new DB.YurtDBEntities())
                {
                    var donemler = db.Donem.ToList();
                    donemler.ForEach(a => a.aktiflik= 0);
                    var donem = db.Donem.SingleOrDefault(p => p.id == donemId);
                    if (donem != null)
                    {
                        donem.aktiflik = 1;
                    }
                    db.SaveChanges();
                }
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult donemPasifYap(int donemId)
        {

            try
            {
                using (var db = new DB.YurtDBEntities())
                {
                    var donem = db.Donem.SingleOrDefault(p => p.id == donemId);
                    if (donem != null)
                    {
                        donem.aktiflik = 0;
                    }
                    db.SaveChanges();
                }
                return Json(new { durum = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { durum = "false" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}