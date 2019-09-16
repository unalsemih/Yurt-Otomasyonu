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
                donemTipi=context.donemTipi.Add(donemTipi);
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
        public ActionResult odaTipiEkleme(OdaBilgileriModel[] odaTipleri)
        {
            
            try
            {
               foreach(var item in odaTipleri)
                {
                    System.Diagnostics.Debug.WriteLine("odaTipiId : " + item.odaTipiId.ToString());
                    System.Diagnostics.Debug.WriteLine("odaTipiKontenjanId : " + item.odaTipiKontenjanId.ToString());
                    System.Diagnostics.Debug.WriteLine("adi  : " + item.odaAdi);
                    System.Diagnostics.Debug.WriteLine("erkekKOntenjan : " + item.erkekKontenjan.ToString());
                    System.Diagnostics.Debug.WriteLine("kizKOntenjan : " + item.kizKontenjan.ToString());
                    if(item.odaTipiId==-1)
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
                        var odaTipi = context.odaTipi.SingleOrDefault(p=>p.odaTipiID==item.odaTipiId);
                        if(odaTipi!=null)
                        {
                            odaTipi.adi = item.odaAdi;
                        }
                        var odaTipiKontenjan = context.odaTipiKontenjan.SingleOrDefault(p => p.odaTipiKontenjanID == item.odaTipiKontenjanId);
                        if(odaTipiKontenjan != null)
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


        [HttpGet]
        public ActionResult donemBilgileriGetir()
        {
            var donemBilgileri = (from p in context.donemTipi
                         join e in context.Donem
                         on p.donemTipiID equals e.donemTipiID
                         select new
                         {
                             donemTipiID = p.donemTipiID,
                             adi = p.adi,
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

    }
}