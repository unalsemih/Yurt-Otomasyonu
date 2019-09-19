using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YurtDb.Models
{
    public class DonemOdaEvrakBilgileri
    {
        public DB.donemTipi donemTipi { get; set; }

        public DB.Donem donemBilgisi { get; set; }


        public DB.odaTipiKontenjan odaTipiBilgisi{get; set;}

        public DB.odaTipi odaTipi { get; set; }
        public int id { get; set; }

        public DonemOdaEvrakBilgileri()
        {
            donemTipi = new DB.donemTipi();
            donemBilgisi = new DB.Donem();

            odaTipiBilgisi = new DB.odaTipiKontenjan();
            odaTipi = new DB.odaTipi();
        }

    }
}