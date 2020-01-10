using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    public class StanowiskoPracownikaToSend
    {
        public int PracownikId { get; set; }
        public String Pracownik { get; set; }
        public int StanowiskoId { get; set; }
        public String Stanowisko { get; set; }

        public StanowiskoPracownikaToSend()
        {

        }

        public StanowiskoPracownikaToSend(StanowiskoPracownika stanowiskoPracownika)
        {
            Pracownik = stanowiskoPracownika.Pracownik.Imie + " " + stanowiskoPracownika.Pracownik.Nazwisko;
            PracownikId = stanowiskoPracownika.Pracownik.Id;
            Stanowisko = stanowiskoPracownika.Stanowisko.Nazwa;
            StanowiskoId = stanowiskoPracownika.Stanowisko.Id;
        }
    }
}