using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(StanowiskoExtension))]
    partial class Stanowisko
    {
    }

    public class StanowiskoExtension
    {
        [Required]
        [StringLength(100, ErrorMessage = "Nazwa stanowiska musi składać się conajmniej z {2} znaków.", MinimumLength = 2)]
        public string Nazwa { get; set; }
    }
}