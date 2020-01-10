using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace GrafikPracy.Providers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    //[RequireHttps]
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var db = new Models.DataBaseEntities())
            {
                if (db != null)
                {
                    var users = db.Pracownik.ToList();
                    if (users != null)
                    {
                        var user = users.Find(u => u.Email == context.UserName && u.Haslo == context.Password && u.Haslo != "");
                        if (user != null)
                        {
                            identity.AddClaim(new Claim("Id", user.Id.ToString()));
                            identity.AddClaim(new Claim("Admin", user.Administrator.ToString()));

                            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {
                                    "userdisplayname", user.Imie + " " + user.Nazwisko
                                }
                             });

                            var ticket = new AuthenticationTicket(identity, props);
                            context.Validated(ticket);
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Nieprawidłowy Email lub Hasło");
                            context.Rejected();
                        }
                    }
                }
                else
                {
                    context.SetError("invalid_grant", "Nieprawidłowy Email lub Hasło");
                    context.Rejected();
                }
                return;
            }
        }
    }
}