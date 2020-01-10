using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(CzasExtension))]
    partial class Czas
    {
    }

    public class CzasExtension
    {
        [Required (ErrorMessage ="Czas pracy musi mieć wskazany początek!")]
        public DateTime Poczatek { get; set; }

        [Required(ErrorMessage = "Czas pracy musi mieć wskazany koniec!")]
        public DateTime Koniec { get; set; }

        [Required(ErrorMessage = "Czas pracy musi być przypisany do grafiku!")]
        public Models.Grafik Grafik { get; set; }
    }
}