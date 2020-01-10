using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(PracownikNaStanowiskuExtension))]
    partial class PracownikNaStanowisku
    {
    }

    public class PracownikNaStanowiskuExtension
    {
        [Required (ErrorMessage ="Obecność pracownika na stanowisku musi być przypisana do czasu!")]
        Models.Czas Czas { get; set; }

        [Required(ErrorMessage = "Obecność pracownika na stanowisku musi być przypisana do pracownika!")]
        Models.Pracownik Pracownik { get; set; }

        [Required(ErrorMessage = "Obecność pracownika na stanowisku musi być przypisana do stanowiska!")]
        Models.Stanowisko Stanowisko { get; set; }
    }
}