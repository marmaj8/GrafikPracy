using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    public class StanowiskoMiejscaToSend
    {
        public int Stanowisko { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        
        public DateTime Pocatek { get; set; }
        public DateTime Koniec { get; set; }
        public int Dzien { get; set; }

        public StanowiskoMiejscaToSend(StanowiskoMiejsca stanowiskoMiejsca)
        {
            Stanowisko = stanowiskoMiejsca.Stanowisko.Id;
            Min = stanowiskoMiejsca.Minimum;
            Max = stanowiskoMiejsca.Maksimum;

            Pocatek = stanowiskoMiejsca.Godzina.Poczatek;
            Koniec = stanowiskoMiejsca.Godzina.Koniec;
            Dzien = stanowiskoMiejsca.Godzina.DzienTygodnia.Id;
        }
    }
}