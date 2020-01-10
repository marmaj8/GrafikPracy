using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    public class GrafikToSend
    {
        public int Id { get; set; }
        public DateTime Poczatek { get; set; }
        public DateTime Koniec { get; set; }
        public DateTime? Zatwierdzony { get; set; }
        public List<NaStanowiskuToSend> NaStanowisku { get; set; }

        public GrafikToSend(Grafik g)
            : this(g, true)
        {
        }

        public GrafikToSend(Grafik g, Boolean czyPlan)
        {
            Id = g.Id;
            Poczatek = g.Poczatek;
            Koniec = g.Koniec;
            Zatwierdzony = g.Zatwierdzony;

            if (czyPlan)
            {
                NaStanowisku = new List<NaStanowiskuToSend>();

                foreach(Czas cz in g.Czas)
                {
                    foreach(PracownikNaStanowisku pns in cz.PracownikNaStanowisku)
                    {
                        NaStanowisku.Add(new NaStanowiskuToSend(pns));
                    }
                }
            }
            else
                NaStanowisku = null;
        }

        public GrafikToSend(Grafik g, int id)
        {
            Id = g.Id;
            Poczatek = g.Poczatek;
            Koniec = g.Koniec;
            Zatwierdzony = g.Zatwierdzony;

            NaStanowisku = new List<NaStanowiskuToSend>();

            foreach (Czas cz in g.Czas)

            {
                foreach (PracownikNaStanowisku pns in cz.PracownikNaStanowisku)
                {
                    if (pns.Pracownik.Id == id)
                    {
                        NaStanowisku.Add( new NaStanowiskuToSend(pns) );
                    }
                }
            }
        }
    }
}