//using System;
//using System.Threading.Tasks;
//using Microsoft.Owin;
//using Owin;
//using IdentityServer3.Core.Configuration;
//using System.Configuration;
//using System.Security.Cryptography.X509Certificates;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNet.Hosting;
//using Microsoft.AspNet.Builder;
//using System.IdentityModel.Tokens;
//using System.Collections.Generic;
//using IdentityServer3.Core;
//using System.Web.Helpers;
//using Microsoft.IdentityModel.Protocols;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.Google;
//using Microsoft.Owin.Security.OpenIdConnect;
//using System.Security.Claims;
//using System.Linq;
//using IdentityServer3;
//using IdentityModel.Client;
//using IdentityServer3.Core.Services.InMemory;
//using IdentityServer3.Core.Models;
//using IdentityServer3.Core.Services;
//using IdentityServer3.AccessTokenValidation;
//using System.Web.Http;

using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Web.Helpers;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityModel.Client;
using System.Threading.Tasks;
using IdentityServer3.Core.Services.InMemory;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.AccessTokenValidation;
using System.Web.Http;
using IdentityServer3.Core.Logging;
using IdentityServer3.EntityFramework;
using IdentityServer3.Core.Services.Default;

[assembly: OwinStartup(typeof(ConsegAutenticationService.Startup))]

namespace ConsegAutenticationService
{
    internal class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            var factory = new IdentityServerServiceFactory()
                   .UseInMemoryUsers(Confg.ConsegUsers.Get())
                   .UseInMemoryClients(Confg.ConsegClients.Get())
                   .UseInMemoryScopes(Confg.ConsegScopes.Get());


            //var viewOptions = new DefaultViewServiceOptions();
            //viewOptions.Stylesheets.Add("/Content/Site.css");
            //viewOptions.Stylesheets.Add("/Content/Site.css");
            //viewOptions.Stylesheets.Add("/Content/Site.css");
            //viewOptions.Stylesheets.Add("/Content/Site.css");
            //viewOptions.Stylesheets.Add("/Content/Site.css");
            //viewOptions.Scripts.Add("/Content/Site.css");
            //viewOptions.Scripts.Add("/Content/Site.css");
            //viewOptions.Scripts.Add("/Content/Site.css");
            //viewOptions.Scripts.Add("/Content/Site.css");


            //factory.ConfigureDefaultViewService(viewOptions);

            // configuração adicionada para criação de banco e relacionamento
            //var efConfg = new EntityFrameworkServiceOptions
            //{
            //    ConnectionString = "DbConseg"
            //};

            //factory.RegisterConfigurationServices(efConfg);
            //factory.RegisterConfigurationServices(efConfg);

            factory.ViewService = new Registration<IViewService, Services.CustomViewService>();

            factory.UserService = new Registration<IUserService, Services.UserService>();

            var options = new IdentityServerOptions
            {               
                SiteName = "Servidor de Autenticação SSP",
                SigningCertificate = Confg.CertificateLoad.Get(), /*new X509Certificate2(Certificate, ConfigurationManager.AppSettings["SigningCertificatePassword"]),*/
                RequireSsl = false,
                Factory = factory,
                AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
                {
                    EnablePostSignOutAutoRedirect = true,
                    IdentityProviders = ConfigureIdentityProviders
                }
            };

            app.UseCookieAuthentication(new CookieAuthenticationOptions { AuthenticationType = "Cookies" });

            app.UseIdentityServer(options);
        }

        private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                AuthenticationType = "Google",
                Caption = "Sign-in with Google",
                SignInAsAuthenticationType = signInAsType,

                ClientId = "701386055558-9epl93fgsjfmdn14frqvaq2r9i44qgaa.apps.googleusercontent.com",
                ClientSecret = "3pyawKDWaXwsPuRDL7LtKm_o"
            });
        }
        private static void ConfigureClients(IEnumerable<Client> clients,EntityFrameworkServiceOptions options)
        {
            using (var db = new ClientConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if(!db.Clients.Any())
                {
                    foreach (var c in clients)
                    {
                        var e = c.ToEntity();
                        db.Clients.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }
        private static void ConfigureScopes(IEnumerable<Scope> clients, EntityFrameworkServiceOptions options)
        {
            using (var db = new ScopeConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Scopes.Any())
                {
                    foreach (var c in clients)
                    {
                        var e = c.ToEntity();
                        db.Scopes.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }


    }
}
