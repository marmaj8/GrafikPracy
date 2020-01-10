using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(StanowiskoMiejscaExtension))]
    partial class StanowiskoMiejsca
    {
    }

    public class StanowiskoMiejscaExtension
    {
        [Required]
        [Range(0, 999999, ErrorMessage = "Minimalna ilość osób na stanowisku nie może być ujemna!")]
        public int Minimum { get; set; }

        [Required]
        [Range(0, 999999, ErrorMessage = "Maksymalna ilość osób na stanowisku nie może być ujemna!")]
        public int Maksimum { get; set; }

        [Required (ErrorMessage ="Wymogi ilości osób na stanowisku muszą być przypisane do stanowiska!")]
        public Models.Stanowisko Stanowisko { get; set; }
    }
}