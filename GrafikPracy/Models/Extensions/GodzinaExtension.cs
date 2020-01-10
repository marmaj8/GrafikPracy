using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(GodzinaExtension))]
    partial class Godzina
    {
    }

    public class GodzinaExtension
    {
        [Required(ErrorMessage = "Przedział pracy stanowiska musi mieć sprecyzowane poczatek!")]
        public DateTime Poczatek { get; set; }

        [Required(ErrorMessage = "Przedział pracy stanowiska musi mieć sprecyzowane koniec!")]
        public DateTime Koniec { get; set; }

        [Required(ErrorMessage = "Przedział pracy stanowiska musi być przypisany do dnia tygodnia")]
        public Models.DzienTygodnia DzienTygodnia{ get; set; }

        [Required(ErrorMessage = "Przedział pracy stanowiska musi byc przypisany do stanowiska!")]
        public Models.StanowiskoMiejsca StanowiskoMiejsca { get; set; }
    }
}