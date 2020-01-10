using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(DzienRoboczyPracownikaExtension))]
    partial class DzienRoboczyPracownika
    {
    }
    public class DzienRoboczyPracownikaExtension
    {
        [Required (ErrorMessage ="Dzień roboczy musi miec ustawioną możliwą godzinę rozpoczęcia!")]
        public DateTime Poczatek { get; set; }

        [Required(ErrorMessage = "Dzień roboczy musi miec ustawioną możliwą godzinę zakończenia!")]
        public DateTime Koniec { get; set; }

        [Required(ErrorMessage = "Dzień roboczy musi być przypisany do pracownika!")]
        public Models.Pracownik Pracownik { get; set; }
        
        [Required(ErrorMessage = "Dzień roboczy musi mieć przypisany dzień tygodnia!")]
        public Models.Pracownik DzienTygodnia { get; set; }
    }
}