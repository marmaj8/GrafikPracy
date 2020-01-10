using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    [MetadataType(typeof(PracownikExtension))]
    partial class Pracownik
    {
    }

    public class PracownikExtension
    {
        [Required]
        [StringLength(50, ErrorMessage = "Nie wprowadzono imienia.", MinimumLength = 1)]
        [RegularExpression("[a-zA-Z]{2,}", ErrorMessage = "Imię może składać się tylko z liter")]
        public string Imie { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Nazwisko musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 2)]
        [RegularExpression("[a-zA-Z]{2,}", ErrorMessage = "Nazwisko może składać się tylko z liter")]
        public string Nazwisko { get; set; }

        [Required]
        [EmailAddress (ErrorMessage ="Email jest niepoprawny")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać conajmniej {2} znaków.", MinimumLength = 5)]
        public string Haslo { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Ilość godzin w umowie musi mieścić się w przedziale od 0 do 220")]
        public int GodzinWUmowie { get; set; }
    }
}