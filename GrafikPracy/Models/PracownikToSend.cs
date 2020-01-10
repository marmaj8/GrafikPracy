using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    public class PracownikToSend
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Haslo { get; set; }
        public string Email { get; set; }
        public Boolean Administrator { get; set; }
        public int GodzinWUmowie { get; set; }

        public List<StanowiskoPracownikaToSend> Stanowiska { get; set; }
        public List<DzienRoboczyPracownikaToSend> DniRobocze { get; set; }

        public PracownikToSend()
        {

        }

        public PracownikToSend(Pracownik pracownik, Boolean czySzczegoly)
        {
            Id = pracownik.Id;
            Imie = pracownik.Imie;
            Nazwisko = pracownik.Nazwisko;
            Haslo = "";
            Email = pracownik.Email;
            Administrator = pracownik.Administrator;
            GodzinWUmowie = pracownik.GodzinWUmowie;

            if (czySzczegoly)
            {
                Stanowiska = new List<StanowiskoPracownikaToSend>();
                DniRobocze = new List<DzienRoboczyPracownikaToSend>();

                foreach(StanowiskoPracownika sp in pracownik.StanowiskoPracownika)
                {
                    Stanowiska.Add(new StanowiskoPracownikaToSend(sp));
                }
                foreach(DzienRoboczyPracownika drp in pracownik.DzienRoboczyPracownika)
                {
                    DniRobocze.Add(new DzienRoboczyPracownikaToSend(drp));
                }
            }
        }

        public PracownikToSend(Pracownik pracownik)
            : this(pracownik, false)
        {

        }
    }
}