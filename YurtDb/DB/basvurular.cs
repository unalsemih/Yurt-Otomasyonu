//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YurtDb.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class basvurular
    {
        public int basvuruID { get; set; }
        public string ogrenciNo { get; set; }
        public Nullable<int> odaTipiID { get; set; }
        public Nullable<System.DateTime> baslangicTarihi { get; set; }
        public Nullable<System.DateTime> bitisTarihi { get; set; }
        public Nullable<int> durum { get; set; }
        public string cinsiyet { get; set; }
        public string IpAdresi { get; set; }
    }
}