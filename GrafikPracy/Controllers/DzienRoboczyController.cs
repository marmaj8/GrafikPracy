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
    public class DzienRoboczyController : ApiController
    {
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

            List<Models.DzienRoboczyPracownikaToSend> lista = new List<Models.DzienRoboczyPracownikaToSend>();

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                foreach (Models.DzienTygodnia d in db.DzienTygodnia)
                {
                    foreach(Models.DzienRoboczyPracownika drp in d.DzienRoboczyPracownika)
                    {
                        lista.Add(new Models.DzienRoboczyPracownikaToSend(drp));
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
        public IHttpActionResult List(int id)
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

            List<Models.DzienRoboczyPracownikaToSend> lista = new List<Models.DzienRoboczyPracownikaToSend>();

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == id);

                foreach(Models.DzienRoboczyPracownika drp in pracownik.DzienRoboczyPracownika)
                {
                    lista.Add(new Models.DzienRoboczyPracownikaToSend(drp));
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

            return Ok(lista);
        }

        [HttpPost]
        public IHttpActionResult Put(Models.DzienRoboczyPracownikaToSend dzienRoboczy)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            Models.DzienRoboczyPracownika drp = null;
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == dzienRoboczy.pracownik);

                drp = pracownik.DzienRoboczyPracownika.FirstOrDefault(d => d.DzienTygodnia.Id == dzienRoboczy.dzien);
                if (drp != null)
                {
                    drp.Poczatek = dzienRoboczy.poczatek;
                    drp.Koniec = dzienRoboczy.koniec;
                }
                else
                {
                    drp = new Models.DzienRoboczyPracownika();
                    drp.DzienTygodnia = db.DzienTygodnia.First(d => d.Id == dzienRoboczy.dzien);
                    drp.Koniec = dzienRoboczy.koniec;
                    drp.Poczatek = dzienRoboczy.poczatek;
                    drp.Pracownik = pracownik;

                    db.DzienRoboczyPracownika.Add(drp);
                }

                db.SaveChanges();

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
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + dzienRoboczy.pracownik);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(drp.Id);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int pracownik, int dzien)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            Models.DzienRoboczyPracownika drp = null;
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Pracownik pr = db.Pracownik.First(p => p.Id == pracownik);
                if (dzien < 0 || dzien > 6)
                    throw new ArgumentException();

                drp = pr.DzienRoboczyPracownika.FirstOrDefault(d => d.DzienTygodnia.Id == dzien);
                
                db.DzienRoboczyPracownika.Remove(drp);

                db.SaveChanges();

            }
            catch (ArgumentException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie ma dnia tygodnia nr " + dzien);
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + pracownik);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(drp.Id);
        }
    }
}
