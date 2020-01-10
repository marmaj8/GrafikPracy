using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy
{
    public class StatystkiStanowiska
    {
        public int id { get; }

        List<Models.StanowiskoMiejsca> wymagania;

        int naStanowisku;
        public int niedobor { get; private set; }
        public int nadmiar { get; private set; }

        DateTime poprzedni;

        public StatystkiStanowiska(int id, List<Models.StanowiskoMiejsca> wymagania)
        {
            this.id = id;
            this.wymagania = wymagania;

            this.naStanowisku = 0;
            this.niedobor = 0;
            this.nadmiar = 0;
        }

        public StatystkiStanowiska(StatystkiStanowiska obj)
            :this(obj.id, obj.wymagania)
        {
        }

        public void dodajPracownika()
        {
            naStanowisku++;
        }

        public void zmianaGodziny(DateTime data)
        {
            if (poprzedni == new DateTime())
            {
                poprzedni = data;
            }
            else if (data.CompareTo(poprzedni) != 0)
            {
                Models.StanowiskoMiejsca wy = wymagania
                    .FirstOrDefault(w => w.Godzina.DzienTygodnia.Id == (int)data.DayOfWeek && w.Godzina.Poczatek.Hour <= data.Hour && w.Godzina.Koniec.Hour >= data.Hour);
                if (wy != null)
                {
                    if (naStanowisku > wy.Maksimum)
                    {
                        nadmiar += naStanowisku - wy.Maksimum;
                    }
                    else if (naStanowisku < wy.Minimum)
                    {
                        niedobor += wy.Minimum - naStanowisku;
                    }
                }
                else
                {
                    nadmiar += naStanowisku;
                }
                poprzedni = data;
                naStanowisku = 0;
            }
        }
    }
}