using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace GrafikPracy.Controllers
{
    [System.Web.Mvc.RequireHttps]
    [Authorize]
    public class PracownikController : ApiController
    {

        public string HashPassword(string haslo)
        {
            return haslo;
            if (String.IsNullOrEmpty(haslo))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(haslo);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        [HttpGet]
        public IHttpActionResult List()
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            List<Models.PracownikToSend> pracownicy = new List<Models.PracownikToSend>();

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();


                foreach (Models.Pracownik p in db.Pracownik)
                {
                    /*
                    p.Haslo = "";
                    p.Urlop = null;
                    p.StanowiskoPracownika = null;
                    p.DzienRoboczyPracownika = null;

                    pracownicy.Add(p);
                    */
                    pracownicy.Add(new Models.PracownikToSend(p));
                }

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(pracownicy);
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                int user = int.Parse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value);
                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == user);

                return Ok(new Models.PracownikToSend(pracownik, true));
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie istnieje pracownik odpowiadający przesłanemu tokenowi");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false"
                && user != id)
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == id);

                return Ok(new Models.PracownikToSend(pracownik, true));
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

        }

        [HttpPut]
        public IHttpActionResult Put(Models.PracownikToSend pracownik)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }
            Models.Pracownik pr = new Models.Pracownik();
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                pr.Imie = pracownik.Imie;
                pr.Nazwisko = pracownik.Nazwisko;
                pr.Haslo = HashPassword(pracownik.Haslo);
                pr.Administrator = false;
                pr.GodzinWUmowie = pracownik.GodzinWUmowie;
                pr.Email = pracownik.Email;

                if (pracownik.DniRobocze != null)
                {
                    foreach (Models.DzienRoboczyPracownikaToSend drpts in pracownik.DniRobocze)
                    {
                        Models.DzienRoboczyPracownika drp = new Models.DzienRoboczyPracownika();
                        drp.DzienTygodnia = db.DzienTygodnia.First(d => d.Id == drpts.dzien);
                        drp.Pracownik = pr;
                        drp.Poczatek = drpts.poczatek;
                        drp.Koniec = drpts.koniec;

                        db.DzienRoboczyPracownika.Add(drp);
                    }
                }

                if (pracownik.Stanowiska != null)
                {
                    foreach (Models.StanowiskoPracownikaToSend spts in pracownik.Stanowiska)
                    {
                        Models.StanowiskoPracownika sp = new Models.StanowiskoPracownika();
                        sp.Pracownik = pr;
                        sp.Stanowisko = db.Stanowisko.First(s => s.Id == spts.StanowiskoId);

                        db.StanowiskoPracownika.Add(sp);
                    }
                }
                
                db.Pracownik.Add(pr);

                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string wiadomosc = "";
                foreach(DbValidationError er in e.EntityValidationErrors.First().ValidationErrors)
                {
                    wiadomosc += er.ErrorMessage + "\n";
                }
                return Content(HttpStatusCode.BadRequest, wiadomosc);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(pr.Id);
        }

        [HttpPatch]
        public IHttpActionResult Patch(Models.PracownikToSend pracownik)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false"
                && user != pracownik.Id)
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                Models.Pracownik pr = db.Pracownik.First(p => p.Id == pracownik.Id);
                if (pracownik.Imie != null && pracownik.Imie.Length > 0)
                    pr.Imie = pracownik.Imie;
                if (pracownik.Nazwisko != null && pracownik.Nazwisko.Length > 0)
                    pr.Nazwisko = pracownik.Nazwisko;
                if (pracownik.Haslo != null && pracownik.Haslo.Length > 0)
                    pr.Haslo = HashPassword(pracownik.Haslo);
                if (pracownik.Email != null && pracownik.Email.Length > 0)
                    pr.Email = pracownik.Email;
                if (pracownik.GodzinWUmowie != null)
                    pr.GodzinWUmowie = pracownik.GodzinWUmowie;
                pr.Administrator = pracownik.Administrator;

                if (pracownik.DniRobocze != null)
                {
                    db.DzienRoboczyPracownika.RemoveRange(pr.DzienRoboczyPracownika);
                    pr.DzienRoboczyPracownika.Clear();

                    foreach(Models.DzienRoboczyPracownikaToSend drpts in pracownik.DniRobocze)
                    {
                        if (drpts.poczatek >= drpts.koniec)
                            return Content(HttpStatusCode.BadRequest, "Godzina zakończenai musi być po godzinie rozpoczęcia!");
                        Models.DzienRoboczyPracownika drp = new Models.DzienRoboczyPracownika();
                        drp.DzienTygodnia = db.DzienTygodnia.First(d => d.Id == drpts.dzien);
                        drp.Pracownik = pr;
                        drp.Poczatek = drpts.poczatek;
                        drp.Koniec = drpts.koniec;

                        db.DzienRoboczyPracownika.Add(drp);
                    }
                }

                if (pracownik.Stanowiska != null)
                {
                    db.StanowiskoPracownika.RemoveRange(pr.StanowiskoPracownika);
                    pr.StanowiskoPracownika.Clear();

                    foreach (Models.StanowiskoPracownikaToSend spts in pracownik.Stanowiska)
                    {
                        Models.StanowiskoPracownika sp = new Models.StanowiskoPracownika();
                        sp.Pracownik = pr;
                        sp.Stanowisko = db.Stanowisko.First(s => s.Id == spts.StanowiskoId);

                        db.StanowiskoPracownika.Add(sp);
                    }
                }

                db.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + pracownik.Id);
            }
            catch (DbEntityValidationException e)
            {
                string wiadomosc = "";
                foreach (DbValidationError er in e.EntityValidationErrors.First().ValidationErrors)
                {
                    wiadomosc += er.ErrorMessage + "\n";
                }
                return Content(HttpStatusCode.BadRequest, wiadomosc);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }


            return Ok();
        }

        [HttpPatch]
        public IHttpActionResult UstawAdministratora(int id, Boolean ustawic)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == id);
                pracownik.Administrator = ustawic;

                db.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == id);

                pracownik.Administrator = false;
                pracownik.GodzinWUmowie = 0;
                pracownik.StanowiskoPracownika.Clear();
                pracownik.Urlop.Clear();
                pracownik.DzienRoboczyPracownika.Clear();

                db.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Stanowiska(int id)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false"
                && user != id)
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            List<Models.StanowiskoPracownikaToSend> stanowiska = new List<Models.StanowiskoPracownikaToSend>();

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == id);

                foreach(Models.StanowiskoPracownika sp in pracownik.StanowiskoPracownika)
                {
                    stanowiska.Add(new Models.StanowiskoPracownikaToSend(sp));
                }
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(stanowiska);
        }

        [HttpDelete]
        public IHttpActionResult StanowiskoDelete(int pracownik, int stanowisko)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false"
                && user != pracownik)
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            List<Models.StanowiskoPracownikaToSend> stanowiska = new List<Models.StanowiskoPracownikaToSend>();

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.StanowiskoPracownika sp = db.StanowiskoPracownika.First(s => s.Pracownik.Id == pracownik && s.Stanowisko.Id == stanowisko);

                db.StanowiskoPracownika.Remove(sp);
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id" + pracownik + " pracującego na stanowisku o id" + stanowisko);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(stanowiska);
        }

        [HttpPut]
        public IHttpActionResult StanowiskoAdd(int pracownik, int stanowisko)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false"
                && user != pracownik)
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.StanowiskoPracownika sp = db.StanowiskoPracownika.FirstOrDefault(s => s.Pracownik.Id == pracownik && s.Stanowisko.Id == stanowisko);

                if (sp == null)
                {
                    sp = new Models.StanowiskoPracownika();
                    sp.Pracownik = db.Pracownik.FirstOrDefault( p => p.Id == pracownik);
                    if (sp.Pracownik == null)
                    {
                        return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id" + pracownik);
                    }
                    sp.Stanowisko = db.Stanowisko.FirstOrDefault(s => s.Id == stanowisko);

                    if (sp.Stanowisko == null)
                    {
                        return Content(HttpStatusCode.NotFound, "Nie znaleziono stanowiska o id" + stanowisko);
                    }

                    db.StanowiskoPracownika.Add(sp);
                }
            }
            catch (DbEntityValidationException e)
            {
                string wiadomosc = "";
                foreach (DbValidationError er in e.EntityValidationErrors.First().ValidationErrors)
                {
                    wiadomosc += er.ErrorMessage + "\n";
                }
                return Content(HttpStatusCode.BadRequest, wiadomosc);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok();
        }
    }
}
