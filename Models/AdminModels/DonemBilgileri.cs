using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YurtDb.Models.AdminModels
{
    public class DonemBilgileri
    {
        public int donemId { get; set; }
        public string donemTipiAdi { get; set; }
        public string donemAdi { get; set; }
        public DateTime baslangicTarihi { get; set; }
        public DateTime bitisTarihi { get; set; }

    }
}