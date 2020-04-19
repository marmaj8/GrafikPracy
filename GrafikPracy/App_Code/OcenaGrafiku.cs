using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GrafikPracy
{
    public class OcenaGrafiku : IFitness
    {
        static double KARA_NADGODZIN = 5;
        static double KARA_PONADNADGODZIN = 100;
        static double KARA_OKIENKO = 20;
        static double KARA_PONADUMOWA = 100;
        static double KARA_PONIZEJUMOWA = 5;
        static double KARA_URLOP = 200;
        static double KARA_BRAKPRACOWNIKA = 10;
        static double KARA_NADMIARPRACOWNIKA = 5;
        static double KARA_ZLESTANOWISKO = 50;
        static double KARA_ZLAGODZINA = 50;

        List<StatystykiPracownika> statyPracWzor;
        List<StatystkiStanowiska> statyStanWzor;

        DateTime poczatek;
        DateTime koniec;

        int przedzial;

        //public OcenaGrafiku(Models.DataBaseEntities db, int zmianNaGodzine, DateTime poczatek, DateTime koniec)
        public OcenaGrafiku(List<Models.Pracownik> pracownicy, List<Models.Stanowisko> stanowiska, int zmianNaGodzine, DateTime poczatek, DateTime koniec)
            : base()
        {
            statyPracWzor = new List<StatystykiPracownika>();
            statyStanWzor = new List<StatystkiStanowiska>();
            this.przedzial = 60 / zmianNaGodzine;
            this.poczatek = poczatek;
            this.koniec = koniec;

            double czescMiesiaca =  koniec.Subtract(poczatek).TotalDays / 22;

            //foreach(Models.Pracownik p in db.Pracownik.Where(p => p.GodzinWUmowie > 0 && p.PracownikNaStanowisku.Count() > 0))
            foreach (Models.Pracownik p in pracownicy)
            {
                List<Models.Dzien> urlop = new List<Models.Dzien>();
                foreach(Models.Urlop u in p.Urlop.Where(ur => ur.Zatwierdzony == true))
                {
                    foreach(Models.DzienUrlopu d in u.DzienUrlopu)
                    {
                        urlop.Add(d.Dzien);
                    }
                }
                List<Models.Stanowisko> stanowiskaP = new List<Models.Stanowisko>();
                foreach(Models.StanowiskoPracownika sp in p.StanowiskoPracownika)
                {
                    stanowiskaP.Add(sp.Stanowisko);
                }
                List<Models.DzienRoboczyPracownika> dniRobocze = p.DzienRoboczyPracownika.ToList();

                statyPracWzor.Add( new StatystykiPracownika(p.Id, p.GodzinWUmowie * czescMiesiaca, urlop, stanowiskaP, dniRobocze) );
            }

            //foreach(Models.Stanowisko s in db.Stanowisko.Where(s => s.StanowiskoPracownika.Count() > 0))
            foreach (Models.Stanowisko s in stanowiska)
            {
                statyStanWzor.Add(new StatystkiStanowiska(s.Id, s.StanowiskoMiejsca.ToList()));
            }
        }


        public double Evaluate(IChromosome chromosome)
        {
            DateTime czas = poczatek;
            //var geny = chromosome.GetGenes();
            var geny = (chromosome as FloatingPointChromosome).ToFloatingPoints();

            List<StatystykiPracownika> statyPrac = new List<StatystykiPracownika>();
            List<StatystkiStanowiska> statyStan = new List<StatystkiStanowiska>();

            foreach(StatystykiPracownika sp in statyPracWzor)
            {
                statyPrac.Add(new StatystykiPracownika(sp));
            }
            foreach (StatystkiStanowiska ss in statyStanWzor)
            {
                statyStan.Add(new StatystkiStanowiska(ss));
            }

            int i = 0;
            while( czas.CompareTo(koniec) < 0)
            {
                foreach (StatystkiStanowiska ss in statyStan)
                {
                    ss.zmianaGodziny(czas);
                }
                foreach (StatystykiPracownika sp in statyPrac)
                {
                    if (geny[i] == 1)
                    {// w pracy
                        int stanowisko = ((int)geny[i + 1]) % statyStan.Count();

                        if(!sp.praca(czas, czas.AddMinutes(przedzial), statyStanWzor[stanowisko].id))
                        {
                            chromosome.ReplaceGene(i, new Gene(0));
                        }
                        else if (stanowisko <= statyPrac.Count())
                        {
                            statyStan[stanowisko].dodajPracownika();
                        }
                    }
                    i += 2;
                }
                czas = czas.AddMinutes(przedzial);
            }

            double kara = 0;
            foreach(StatystykiPracownika sp in statyPrac)
            {
                kara += sp.nadgodzin * KARA_NADGODZIN;
                kara += sp.ponadNadgodzin * KARA_PONADNADGODZIN;
                kara += sp.ponadUmowa() * KARA_PONADUMOWA;
                kara += sp.ponizejUmowa() * KARA_PONIZEJUMOWA;
                kara += sp.okienek * KARA_OKIENKO;
                kara += sp.pracaUrlop * KARA_URLOP;
                kara += sp.zleStanowisko * KARA_ZLESTANOWISKO;
                kara += sp.zlaGodzina * KARA_ZLAGODZINA;
            }
            foreach(StatystkiStanowiska ss in statyStan)
            {
                kara += ss.nadmiar * KARA_NADMIARPRACOWNIKA;
                kara += ss.niedobor * KARA_BRAKPRACOWNIKA;
            }


            return -kara;
        }
    }
}