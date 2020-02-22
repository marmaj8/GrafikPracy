using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(UrlopExtension))]
    partial class Urlop
    {
    }

    public class UrlopExtension
    {
        [StringLength(1000, ErrorMessage = "Powód urlopu jest zbyt długi!")]
        public string Powod { get; set; }
        /*
        [Required]
        [MinLength(1, ErrorMessage = "Urlup musi zawierać conajmniej 1 dzień!")]
        public List<Models.DzienUrlopu> DzienUrlopu;*/

        [Required (ErrorMessage ="Urlop musi byc przypisany do pracownika!")]
        public Models.Pracownik Pracownik;
    }
}