using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace GrafikPracy.Controllers
{
    [System.Web.Mvc.RequireHttps]
    [Authorize]
    public class UrlopController : ApiController
    {
        /*
        [HttpGet]
        public IHttpActionResult List(DateTime data, Boolean zatwierdzone)
        {
            User = System.Web.HttpContext.Current.User;
            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            HashSet<Models.UrlopToSend> lista = new HashSet<Models.UrlopToSend>();
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Dzien dzien = db.Dzien.First(d => d.Data.Date == data.Date);


                if (zatwierdzone)
                {
                    foreach (Models.DzienUrlopu du in dzien.DzienUrlopu.Where( d => d.Urlop.Zatwierdzony != true))
                    {
                        lista.Add(new Models.UrlopToSend(du.Urlop));
                    }
                }
                else
                {
                    foreach (Models.DzienUrlopu du in dzien.DzienUrlopu)
                    {
                        lista.Add(new Models.UrlopToSend(du.Urlop));
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono dnia " + data);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(lista);
        }
        */

        [HttpGet]
        public IHttpActionResult List(DateTime data, Boolean zatwierdzone)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            List<Models.UrlopToSend> lista = new List<Models.UrlopToSend>();
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                if (zatwierdzone)
                {

                    foreach (Models.Urlop u in db.Urlop.Where(u => u.Zatwierdzony && u.DzienUrlopu.OrderBy(dd => dd.Dzien_Data).FirstOrDefault().Dzien_Data >= data))
                    {
                        lista.Add(new Models.UrlopToSend(u));
                    }
                }
                else
                {
                    foreach (Models.Urlop u in db.Urlop.Where(u => u.DzienUrlopu.OrderBy(dd => dd.Dzien_Data).FirstOrDefault().Dzien_Data >= data))
                    {
                        lista.Add(new Models.UrlopToSend(u));
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
        public IHttpActionResult List(int id, Boolean zatwierdzone)
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

            List<Models.UrlopToSend> lista = new List<Models.UrlopToSend>();
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == id);

                if (zatwierdzone)
                {
                    foreach (Models.Urlop u in pracownik.Urlop.Where(ur => ur.Zatwierdzony == true))
                    {
                        lista.Add(new Models.UrlopToSend(u));
                    }
                }
                else
                {
                    foreach (Models.Urlop u in pracownik.Urlop)
                    {
                        lista.Add(new Models.UrlopToSend(u));
                    }
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

        [HttpPut]
        public IHttpActionResult Put(Models.UrlopToSend urlop)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false"
                && user != urlop.pracownik)
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            Models.Urlop ur = null;
            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();
                Models.Pracownik pracownik = db.Pracownik.First(p => p.Id == urlop.pracownik);

                DateTime dzis = new DateTime();
                if (urlop.poczatek < dzis.Date || urlop.koniec < dzis.Date)
                {
                    return Content(HttpStatusCode.BadRequest, "Obie daty muszą być w przyszłości!");
                }

                ur = new Models.Urlop();
                ur.Powod = urlop.powod;
                ur.Pracownik = pracownik;
                //ur.DzienUrlopu = new List<Models.DzienUrlopu>();

                db.Urlop.Add(ur);
                
                for (DateTime dt = urlop.poczatek; dt <= urlop.koniec; dt = dt.AddDays(1))
                {
                    if (pracownik.DzienRoboczyPracownika.FirstOrDefault(d => d.DzienTygodnia.Id == (int)dt.DayOfWeek) != null)
                    {
                        Models.Dzien dzien = db.Dzien.FirstOrDefault(d => d.Data == dt);
                        if (dzien == null)
                        {
                            dzien = new Models.Dzien();
                            dzien.Data = dt;
                            db.Dzien.Add(dzien);
                        }
                        Models.DzienUrlopu du = new Models.DzienUrlopu();
                        du.Dzien = dzien;
                        du.Urlop = ur;
                        db.DzienUrlopu.Add(du);
                    }
                }
                

                db.SaveChanges();
                
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono pracownika o id " + urlop.pracownik);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(ur.Id);
        }

        [HttpPatch]
        public IHttpActionResult Zatwierdz(int id)
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

                Models.Urlop ur = db.Urlop.First(u => u.Id == id);
                ur.Zatwierdzony = true;

                db.SaveChanges();

            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono urlopu o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(id);
        }
    }
}
