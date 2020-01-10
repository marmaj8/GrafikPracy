using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    public class DzienRoboczyPracownikaToSend
    {
        public int dzien;
        public int pracownik;
        public DateTime poczatek;
        public DateTime koniec;

        public DzienRoboczyPracownikaToSend()
        {

        }

        public DzienRoboczyPracownikaToSend(DzienRoboczyPracownika drp)
        {
            dzien = drp.DzienTygodnia.Id;
            pracownik = drp.Pracownik.Id;
            poczatek = drp.Poczatek;
            koniec = drp.Koniec;
        }
    }
}