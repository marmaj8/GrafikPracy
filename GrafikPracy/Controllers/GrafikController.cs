using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;

namespace GrafikPracy.Controllers
{
    [System.Web.Mvc.RequireHttps]
    [Authorize]
    public class GrafikController : ApiController
    {
        private static Boolean TRWA_GENEROWANIE = false;

        [HttpGet]
        public IHttpActionResult List(Boolean czyWszystkie)
        {
            List<Models.GrafikToSend> lista = new List<Models.GrafikToSend>();

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                if (czyWszystkie)
                {
                    foreach (Models.Grafik gr in db.Grafik)
                    {
                        lista.Add(new Models.GrafikToSend(gr, false));
                    }
                }
                else
                {
                    foreach (Models.Grafik gr in db.Grafik.Where(g => g.Zatwierdzony != null))
                    {
                        lista.Add(new Models.GrafikToSend(gr, false));
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(lista);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Models.GrafikToSend grafik = null;

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                grafik = new Models.GrafikToSend(db.Grafik.First(g => g.Id == id));
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono grafiku o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(grafik);
        }

        [HttpGet]
        public IHttpActionResult GrafikPracownika(int id, DateTime dzien)
        {
            Models.GrafikToSend grafik = null;

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Grafik gr = db.Grafik.Where(g => g.Zatwierdzony != null && g.Poczatek <= dzien && g.Koniec > dzien).OrderBy(g => g.Poczatek).First();

                grafik = new Models.GrafikToSend(gr, id);
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono zatwierdzonego grafiku na dzień " + dzien.ToString("dd'-'MM'-'yyyy"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(grafik);
        }

        [HttpPatch]
        public IHttpActionResult Zatwierdz(int id, Boolean zatwierdzone)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            Models.DataBaseEntities db = new Models.DataBaseEntities();

            Models.Grafik gr = null;
            try
            {
                gr = db.Grafik.First(g => g.Id == id);

                if (zatwierdzone)
                {
                    gr.Zatwierdzony = DateTime.Now;
                }
                else
                {
                    gr.Zatwierdzony = null;
                }
                db.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono grafiku o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(gr.Zatwierdzony);
        }

        [HttpPost]
        public IHttpActionResult Generuj(DateTime poczatek, DateTime koniec, int naGodzine)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            Models.Grafik grafik = null;

            try
            {
                if (TRWA_GENEROWANIE)
                {
                    return Content(HttpStatusCode.Conflict, "Trwa Generowanie");
                }
                TRWA_GENEROWANIE = true;

                Models.DataBaseEntities db = new Models.DataBaseEntities();

                List<Models.Pracownik> pracownicy = db.Pracownik.Where(p => p.GodzinWUmowie > 0 && p.StanowiskoPracownika.Count() > 0).ToList();
                List<Models.Stanowisko> stanowiska = db.Stanowisko.Where(s => s.StanowiskoPracownika.Count() > 0).ToList();

                // 1 bit dla czy w pracy
                // pozostale numer stanowiska
                int dlugoscKodonu = (int)Math.Round(Math.Log(stanowiska.Count(), 2), 0, MidpointRounding.AwayFromZero);

                int iloscPrzedzialow = (int)Math.Round(koniec.Subtract(poczatek).TotalHours * naGodzine, 0, MidpointRounding.AwayFromZero) * naGodzine;
                double przedzial = 60 / naGodzine;

                List<double> genMin = new List<double>();
                List<double> genMax = new List<double>();
                List<int> genBits = new List<int>();
                List<int> genDecimal = new List<int>();

                for (int i = 0; i < iloscPrzedzialow * pracownicy.Count(); i++)
                {
                    genMin.Add(0);
                    genMin.Add(0);
                    genMax.Add(1);
                    genMax.Add(stanowiska.Count() - 1);
                    genBits.Add(1);
                    genBits.Add(dlugoscKodonu);
                    genDecimal.Add(0);
                    genDecimal.Add(0);
                }

                var selection = new EliteSelection();
                //var selection = new TournamentSelection(100, true);
                var crossover = new UniformCrossover(0.5f);
                //var crossover = new ThreeParentCrossover();
                //var mutation = new ReverseSequenceMutation();
                var mutation = new FlipBitMutation();
                //var mutation = new UniformMutation(false);
                //var fitness = new OcenaGrafiku(db, naGodzine, poczatek, koniec);
                var fitness = new OcenaGrafiku(pracownicy,stanowiska, naGodzine, poczatek, koniec);

                var termination = new GeneticSharp.Domain.Terminations.OrTermination(
                    new FitnessStagnationTermination(100) , new FitnessThresholdTermination(0), new TimeEvolvingTermination(new TimeSpan(0, 5, 0)));
                
                var chromosome = new FloatingPointChromosome(genMin.ToArray(), genMax.ToArray(), genBits.ToArray(), genDecimal.ToArray());
                var population = new Population(1000, 10000, chromosome);


                var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
                ga.Termination = termination;
                ga.MutationProbability = 1.0f;

                ga.Start();

                var wynik = (ga.BestChromosome as FloatingPointChromosome).ToFloatingPoints();
                double? x = (ga.BestChromosome as FloatingPointChromosome).Fitness;

                DateTime czas = poczatek;

                grafik = new Models.Grafik();
                grafik.Poczatek = poczatek;
                grafik.Koniec = koniec;

                db.Grafik.Add(grafik);

                int j = 0;
                while (czas < koniec)
                {
                    Models.Czas cz = new Models.Czas();

                    foreach (Models.Pracownik pr in pracownicy)
                    {
                        if (wynik[j] == 1)
                        {
                            Models.PracownikNaStanowisku pns = new Models.PracownikNaStanowisku();
                            pns.Pracownik = pr;
                            pns.Stanowisko = stanowiska[(int)wynik[j + 1] % stanowiska.Count()];
                            pns.Czas = cz;

                            db.PracownikNaStanowisku.Add(pns);
                        }
                        j += 2;
                    }
                    if (cz.PracownikNaStanowisku.Count() > 0)
                    {
                        cz.Grafik = grafik;
                        cz.Poczatek = czas;
                        cz.Koniec = czas.AddMinutes(przedzial);
                        db.Czas.Add(cz);
                    }
                    czas = czas.AddMinutes(przedzial);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.ToString());
            }
            TRWA_GENEROWANIE = false;

            return Ok(grafik.Id);
        }

        [HttpGet]
        public IHttpActionResult CzyGeneruje()
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania żądania!");
            }

            return Ok(TRWA_GENEROWANIE);
        }
        [HttpDelete]
        public IHttpActionResult SkasujPozycje(int id)
        {
            User = System.Web.HttpContext.Current.User;
            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.PracownikNaStanowisku pns = db.PracownikNaStanowisku.FirstOrDefault(p => p.Id == id);

                if (pns == null)
                {
                    return Content(HttpStatusCode.NotFound, "Nie znaleziono pozycji o id " + id);
                }

                db.PracownikNaStanowisku.Remove(pns);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.ToString());
            }

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult DodajPozycje(Models.NaStanowiskuToSend naStanowisku, int grafik)
        {
            User = System.Web.HttpContext.Current.User;
            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                Models.Grafik gr = db.Grafik.FirstOrDefault(g => g.Id == grafik);
                if (gr == null)
                {
                    return Content(HttpStatusCode.NotFound, "Nie znaleziono grafiku o id " + grafik);
                }

                Models.Pracownik pracownik = db.Pracownik.FirstOrDefault(p => p.Id == naStanowisku.IdPracownika);
                if (pracownik == null)
                {
                    return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + naStanowisku.IdPracownika);
                }

                Models.Stanowisko stanowisko = db.Stanowisko.FirstOrDefault(s => s.Id == naStanowisku.IdStanowiska);
                if (stanowisko == null)
                {
                    return Content(HttpStatusCode.NotFound, "Nie znaleziono stanowiska o id " + naStanowisku.IdStanowiska);
                }

                Models.Czas godzina = gr.Czas.FirstOrDefault(g => g.Poczatek == naStanowisku.Poczatek && g.Koniec == naStanowisku.Koniec);
                if (godzina == null)
                {
                    godzina = new Models.Czas();
                    godzina.Poczatek = naStanowisku.Poczatek;
                    godzina.Koniec = naStanowisku.Koniec;
                    godzina.Grafik = gr;

                    db.Czas.Add(godzina);
                }
                else
                {
                    foreach(Models.PracownikNaStanowisku pns in godzina.PracownikNaStanowisku)
                    {
                        if (pns.Pracownik.Id == naStanowisku.IdPracownika)
                        {
                            return Content(HttpStatusCode.BadRequest, "Pracownik znajduje się już na stanowisku");
                        }
                    }
                }

                Models.PracownikNaStanowisku prns = new Models.PracownikNaStanowisku();
                prns.Pracownik = pracownik;
                prns.Stanowisko = stanowisko;
                prns.Czas = godzina;

                db.PracownikNaStanowisku.Add(prns);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.ToString());
            }

            return Ok();
        }
    }


}
