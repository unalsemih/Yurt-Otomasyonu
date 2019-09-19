using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YurtDb.Models.AdminModels
{
    public class OdaBilgileriModel
    {
        public int donemTipiId { get; set; }
        public int odaTipiId { get; set; }
        public int odaTipiKontenjanId { get; set; }
        public int erkekKontenjan { get; set; }
        public int kizKontenjan { get; set; }
        public string odaAdi { get; set; }

    }
}