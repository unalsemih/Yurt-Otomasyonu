using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YurtDb.Models
{
    public class BasvuruFormu
    {
        public int basvuruAdimi { get; set; }
        public long tcNo { get; set; }
        public DB.ogrenci ogrenciBilgileri { get; set; }
        public DB.odaFiyatlari odaBilgileri { get; set; }
        public DB.evrakBilgileri evrakBilgileri { get; set; }
        public List<DB.evrakTipi> evrakTipi { get; set; }

        public BasvuruFormu()
        {
            tcNo = 0;
            basvuruAdimi = 0;
            sayfaYonlendirme = -1;
            odaFiyati = 0;
        }
       public string egitimDurumu { get; set; }
        public int sayfaYonlendirme { get; set; }
        public List<SelectListItem> odaTipleri { get; set; }
        public List<SelectListItem> indirimler { get; set; }
        public List<int> yuklenenEvraklar { get; set; }
        public double odaFiyati { get; set; }
        public List<OdaKontenjan> odaKontenjanList { get; set; }
    }
}