using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    public class NaStanowiskuToSend
    {
        public int IdPracownika { get; set; }
        public DateTime Poczatek { get; set; }
        public DateTime Koniec { get; set; }
        public int IdStanowiska { get; set; }

        public NaStanowiskuToSend(PracownikNaStanowisku p)
        {
            IdPracownika = p.Pracownik.Id;
            Poczatek = p.Czas.Poczatek;
            Koniec = p.Czas.Koniec;
            IdStanowiska = p.Stanowisko.Id;
        }
    }
}