using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy
{
    public class StatystykiPracownika
    {
        static int MIN_ODSTEP = 11;
        static int MAX_NADGODZIN = 12;
        static int MAX_GODZIN = 8;


        public int id { get; }
        List<Models.Dzien> urlop;
        DateTime poprzedni;
        List<Models.Stanowisko> stanowiska;
        List<Models.DzienRoboczyPracownika> dniRobocze;
        double maxh;

        public double okienek { get; private set; }
        public double godzinDnia { get; private set; }
        public double godzin { get; private set; }
        public double nadgodzin { get; private set; }
        public double ponadNadgodzin { get; private set; }
        public double pracaUrlop { get; private set; }
        public double zleStanowisko { get; private set; }
        public double zlaGodzina { get; private set; }

        public StatystykiPracownika(int id, double maxh, List<Models.Dzien> urlop, List<Models.Stanowisko> stanowiska, List<Models.DzienRoboczyPracownika> dniRobocze)
        {
            this.id = id;
            this.maxh = maxh;
            this.urlop = urlop;
            this.stanowiska = stanowiska;
            this.dniRobocze = dniRobocze;

            this.okienek = 0;
            this.godzinDnia = 0;
            this.godzin = 0;
            this.nadgodzin = 0;
            this.ponadNadgodzin = 0;
            this.pracaUrlop = 0;
            this.zleStanowisko = 0;
        }

        public StatystykiPracownika(StatystykiPracownika obj)
            : this(obj.id, obj.maxh, obj.urlop, obj.stanowiska, obj.dniRobocze)
        {
        }

        public void praca(DateTime poczatek, DateTime koniec, int stanowisko)
        {
            TimeSpan time = koniec.Subtract(poczatek);
            if (poprzedni != new DateTime())
            {
                godzinDnia += time.TotalHours;
                godzin += time.TotalHours;

                TimeSpan diff = poczatek.Subtract(poprzedni);
                if (diff.TotalMinutes > 0 && diff.TotalHours < MIN_ODSTEP)
                {
                    okienek += Math.Pow(diff.TotalHours, diff.TotalHours);
                }
                else
                {
                    double nadg = godzinDnia - MAX_GODZIN;
                    if (nadg > 0)
                    {
                        nadgodzin += nadg;

                        nadg = nadg - MAX_NADGODZIN;
                        if (nadg > 0)
                        {
                            ponadNadgodzin += nadg;
                            nadgodzin -= nadg;
                        }
                    }
                }
            }

            Models.DzienRoboczyPracownika drp = dniRobocze.FirstOrDefault(d => d.DzienTygodnia.Id == (int)poczatek.DayOfWeek);
            if (drp == null || drp.Poczatek.TimeOfDay > poczatek.TimeOfDay || drp.Koniec.TimeOfDay < koniec.TimeOfDay)
            {
                zlaGodzina += time.TotalHours;
            }

            Models.Dzien ur = urlop.FirstOrDefault(u => u.Data.Date == poczatek.Date || (u.Data.AddMinutes(-1)).Date == koniec.Date);
            if (ur != null)
            {
                pracaUrlop += time.TotalHours;
            }

            Models.Stanowisko st = stanowiska.FirstOrDefault(s => s.Id == stanowisko);
            if (st == null)
            {
                zleStanowisko += time.TotalHours;
            }

            poprzedni = koniec;
        }

        public double ponadUmowa()
        {
            double h = godzin - maxh;

            if (h < 0)
            {
                h = 0;
            }

            return h;
        }

        public double ponizejUmowa()
        {
            double h = maxh - godzin;

            if (h < 0)
            {
                h = 0;
            }

            return h;
        }
    }


}