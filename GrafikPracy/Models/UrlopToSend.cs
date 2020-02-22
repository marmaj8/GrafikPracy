using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    public class UrlopToSend
    {
        public int pracownik;
        public DateTime poczatek;
        public DateTime koniec;
        public int dni;
        public Boolean zatwierdzony;
        public String powod;
        public int id;

        public UrlopToSend()
        {

        }

        public UrlopToSend(Urlop urlop)
        {
            id = urlop.Id;
            pracownik = urlop.Pracownik.Id;
            zatwierdzony = urlop.Zatwierdzony;
            powod = urlop.Powod;
            dni = 0;

            foreach (DzienUrlopu du in urlop.DzienUrlopu)
            {
                if (urlop.Pracownik.DzienRoboczyPracownika.FirstOrDefault( d => d.DzienTygodnia.Id == (int)du.Dzien.Data.DayOfWeek) != null)
                {
                    dni++;
                }
            }

            poczatek = urlop.DzienUrlopu.OrderBy(u => u.Dzien.Data).First().Dzien.Data;
            koniec = urlop.DzienUrlopu.OrderBy(u => u.Dzien.Data).Last().Dzien.Data;
        }
    }
}