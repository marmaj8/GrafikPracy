using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(StanowiskoPracownikaExtension))]
    partial class StanowiskoPracownika
    {
    }

    public class StanowiskoPracownikaExtension
    {
        [Required (ErrorMessage ="Stanowisko pracownika musi być przypisane do pracownika")]
        public string Pracownik { get; set; }

        [Required(ErrorMessage = "Stanowisko pracownika musi być przypisane do stanowiska")]
        public string Stanowisko { get; set; }
    }
}