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
    
    public partial class indirimler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public indirimler()
        {
            this.odaFiyatlaris = new HashSet<odaFiyatlari>();
        }
    
        public int indirimlerID { get; set; }
        public string aciklama { get; set; }
        public Nullable<double> indirimOrani { get; set; }
        public Nullable<int> donemTipiID { get; set; }
    
        public virtual donemTipi donemTipi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<odaFiyatlari> odaFiyatlaris { get; set; }
    }
}
