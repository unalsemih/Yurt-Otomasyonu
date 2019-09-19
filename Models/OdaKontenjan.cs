using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YurtDb.Models
{
    public class OdaKontenjan
    {
        public int odaTipiID { get; set; }
        public string adi { get; set; }
        public int donemTipiID { get; set; }
        public int kontenjan { get; set; }
        public OdaKontenjan()
        {
            kontenjan = 0;
        }
    }
}