using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YurtDb.Models.AdminModels
{
    public class EvrakTipleriModel
    {
        public int evrakTipiId { get; set; }
        public string evrakTipiAdi { get; set; }
        public string zorunluluk { get; set; }
        public int donemTipiId { get; set; }
    }
}