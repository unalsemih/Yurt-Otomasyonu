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
    
    public partial class evrakTipi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public evrakTipi()
        {
            this.evrakBilgileri = new HashSet<evrakBilgileri>();
        }
    
        public int evrakTipiID { get; set; }
        public string adi { get; set; }
        public Nullable<int> zorunluluk { get; set; }
        public Nullable<int> arsiv { get; set; }
        public Nullable<int> gorunum { get; set; }
        public Nullable<int> donemTipiID { get; set; }
    
        public virtual donemTipi donemTipi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evrakBilgileri> evrakBilgileri { get; set; }
    }
}
