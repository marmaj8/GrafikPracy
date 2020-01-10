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
    public class StanowiskoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult List()
        {
            List<Models.StanowiskoToSend> lista = new List<Models.StanowiskoToSend>();

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                foreach(Models.Stanowisko st in db.Stanowisko)
                {
                    /*
                    st.PracownikNaStanowisku = null;
                    st.StanowiskoPracownika = null;
                    st.StanowiskoMiejsca = null;
                    lista.Add(st);
                    */
                    lista.Add(new Models.StanowiskoToSend(st, false));
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
            Models.StanowiskoToSend stanowisko = null;

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                stanowisko = new Models.StanowiskoToSend(db.Stanowisko.First(s => s.Id == id));
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.NotFound, "Nie znaleziono stanowiska o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(stanowisko);
        }

        [HttpPut]
        public IHttpActionResult Put(Models.StanowiskoToSend stanowisko)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin");
            if (((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Admin").Value == "false")
            {
                return Content(HttpStatusCode.Forbidden, "Brak uprawnień do wykonania zadania!");
            }

            Models.Stanowisko st = null;

            try
            {
                Models.DataBaseEntities db = new Models.DataBaseEntities();

                st = new Models.Stanowisko();

                st.Nazwa = stanowisko.Nazwa;
                st.StanowiskoMiejsca = new List<Models.StanowiskoMiejsca>();

                db.Stanowisko.Add(st);

                foreach (Models.StanowiskoMiejscaToSend smts in stanowisko.Miejsca)
                {
                    Models.Godzina go = db.Godzina.FirstOrDefault(g => g.Poczatek == smts.Pocatek && g.Koniec == smts.Koniec && g.DzienTygodnia.Id == smts.Dzien);
                    if (go == null)
                    {
                        go = new Models.Godzina();
                        go.Poczatek = smts.Pocatek;
                        go.Koniec = smts.Koniec;
                        go.DzienTygodnia = db.DzienTygodnia.First(d => d.Id == smts.Dzien % 7);

                        db.Godzina.Add(go);
                    }

                    Models.StanowiskoMiejsca sm = new Models.StanowiskoMiejsca();
                    sm.Godzina = go;
                    sm.Maksimum = smts.Max;
                    sm.Minimum = smts.Min;
                    sm.Stanowisko = st;

                    db.StanowiskoMiejsca.Add(sm);
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
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok(st.Id);
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
                Models.Stanowisko stanowisko = db.Stanowisko.First(s => s.Id == id);

                stanowisko.StanowiskoPracownika.Clear();
                stanowisko.StanowiskoMiejsca.Clear();

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
                return Content(HttpStatusCode.NotFound, "Nie znaleziono stanowiska o id " + id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok();
        }

        [HttpPatch]
        public IHttpActionResult Patch(Models.StanowiskoToSend stanowisko)
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
                Models.Stanowisko st = db.Stanowisko.First(s => s.Id == stanowisko.Id);

                st.Nazwa = stanowisko.Nazwa;

                st.StanowiskoMiejsca.Clear();

                foreach(Models.StanowiskoMiejscaToSend smts in stanowisko.Miejsca)
                {
                    Models.Godzina go = db.Godzina.FirstOrDefault(g => g.Poczatek == smts.Pocatek && g.Koniec == smts.Koniec && g.DzienTygodnia.Id == smts.Dzien);
                    if (go == null)
                    {
                        go = new Models.Godzina();
                        go.Poczatek = smts.Pocatek;
                        go.Koniec = smts.Koniec;
                        go.DzienTygodnia = db.DzienTygodnia.First(d => d.Id == smts.Dzien % 7);

                        db.Godzina.Add(go);
                    }

                    Models.StanowiskoMiejsca sm = new Models.StanowiskoMiejsca();
                    sm.Godzina = go;
                    sm.Maksimum = smts.Max;
                    sm.Minimum = smts.Min;
                    sm.Stanowisko = st;

                    db.StanowiskoMiejsca.Add(sm);
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
                return Content(HttpStatusCode.NotFound, "Nie znaleziono stanowiska o id " + stanowisko.Id);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Błąd serwera");
            }

            return Ok();
        }
    }
}
