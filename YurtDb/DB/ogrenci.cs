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
    
    public partial class ogrenci
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ogrenci()
        {
            this.evrakBilgileris = new HashSet<evrakBilgileri>();
        }
    
        public int ogrenciNo { get; set; }
        public Nullable<long> tcNo { get; set; }
        public string adi { get; set; }
        public string soyadi { get; set; }
        public string egitimDurumu { get; set; }
        public string fakulteNo { get; set; }
        public string bolumNo { get; set; }
        public Nullable<int> donemTipiID { get; set; }
        public string cinsiyet { get; set; }
    
        public virtual donemTipi donemTipi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evrakBilgileri> evrakBilgileris { get; set; }
    }
}
