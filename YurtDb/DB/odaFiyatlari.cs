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
    
    public partial class odaFiyatlari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public odaFiyatlari()
        {
            this.Kayıt = new HashSet<Kayıt>();
        }
    
        public int odaFiyatlariID { get; set; }
        public string aciklama { get; set; }
        public Nullable<int> odaTipiId { get; set; }
        public Nullable<int> indirimId { get; set; }
        public Nullable<double> fiyat { get; set; }
        public Nullable<int> donemTipiID { get; set; }
    
        public virtual donemTipi donemTipi { get; set; }
        public virtual indirimler indirimler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kayıt> Kayıt { get; set; }
        public virtual odaTipi odaTipi { get; set; }
    }
}
