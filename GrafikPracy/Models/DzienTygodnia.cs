//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GrafikPracy.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DzienTygodnia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DzienTygodnia()
        {
            this.DzienRoboczyPracownika = new HashSet<DzienRoboczyPracownika>();
            this.Godzina = new HashSet<Godzina>();
        }
    
        public int Id { get; set; }
        public string Nazwa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DzienRoboczyPracownika> DzienRoboczyPracownika { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Godzina> Godzina { get; set; }
    }
}
