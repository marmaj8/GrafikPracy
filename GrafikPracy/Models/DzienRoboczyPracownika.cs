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
    
    public partial class DzienRoboczyPracownika
    {
        public int Id { get; set; }
        public System.DateTime Poczatek { get; set; }
        public System.DateTime Koniec { get; set; }
        public int Pracownik_Id { get; set; }
        public int DzienTygodnia_Id { get; set; }
    
        public virtual DzienTygodnia DzienTygodnia { get; set; }
        public virtual Pracownik Pracownik { get; set; }
    }
}