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
    
    public partial class evrakBilgileri
    {
        public int id { get; set; }
        public Nullable<int> evrakTipiID { get; set; }
        public string link { get; set; }
        public Nullable<int> ogrenciNo { get; set; }
        public Nullable<int> donemTipiID { get; set; }
    
        public virtual donemTipi donemTipi { get; set; }
        public virtual evrakTipi evrakTipi { get; set; }
        public virtual ogrenci ogrenci { get; set; }
    }
}
