using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsegAutenticationService.Confg
{
    public class ConsegClients
    {
        public static List<Client> Get()
        {
            ConsegAutenticationService.Context.dbContext db = new ConsegAutenticationService.Context.dbContext();
            
            Client clients = new Client();
            List<Client> Listclients = new List<Client>();
            var ResultClients = db._Client.ToList();

            List<string> liststring = db._Scope.Select(d => d.Name).ToList<string>();

            foreach (var item in ResultClients)
            {
                //var clientid = db._ClientSecret.Where(d => d.Client.Id == item.Id).Select(d => d.Value.ToString()).SingleOrDefault().ToString();
                //var Urli = db._ClientRedirectUri.Where(d => d.Client.Id == item.Id).Select(d => d.Uri.ToString()).Single();
                //var Urlo = db._ClientPostLogoutRedirectUri.Where(d => d.Client.Id == item.Id).Select(d => d.Uri.ToString()).Single();

                if (item.ClientId == "AdminConseg" /*|| item.ClientId == "ConsegRouter"*/)
                {

                    clients = new Client
                    {
                        ClientName = item.ClientName,
                        ClientId = item.ClientId,
                        Enabled = true,
                        RequireConsent = item.RequireConsent,
                        Flow = (Flows)item.Flow,
                        ClientSecrets = new List<Secret> {
                        new Secret(db._ClientSecret.Where(d=>d.Client.Id == item.Id).Select(d=>d.Value.ToString()).Single().ToString())
                    },
                        AbsoluteRefreshTokenLifetime = 86400,
                        SlidingRefreshTokenLifetime = 43200,
                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        RedirectUris = new List<string>
                    {
                       db._ClientRedirectUri.Where(d=>d.Client.Id == item.Id).Select(d=>d.Uri.ToString()).Single(),

                       //item.LogoUri.ToString(),
                        /*"http://localhost:61218/" *//*item.RedirectUris.ToString()*/
                    },
                        PostLogoutRedirectUris = new List<string>
                    {
                        db._ClientPostLogoutRedirectUri.Where(d=>d.Client.Id == item.Id).Select(d=>d.Uri.ToString()).Single(),
                         //item.LogoUri.ToString(),
                       /* "http://localhost:61218/"*//*item.PostLogoutRedirectUris.ToString()*/
                    },
                        //AllowedScopes = db._Scope.Select(d => d.Name).ToList<string>(),
                        AllowedScopes = new List<string>
                        {
                           "openid", "profile", "roles", "SSP"
                        }
                    };
                    Listclients.Add(clients);
                }
              
               
            }
            return Listclients;

        }
        
        //var certClient = new Client
        //{
        //    ClientName = "Client Credentials Flow Client with Client Certificate",
        //    ClientId = "certclient",
        //    ClientSecrets = new List<Secret> { new Secret
        //    {
        //        Value = "61B754C541BBCFC6A45A9E9EC5E47D8702B78C29",
        //        Type = Constants.SecretTypes.X509CertificateThumbprint,
        //    } },
        //    Flow = Flows.ClientCredentials,
        //    AllowedScopes = new List<string> { "read", "write" },
        //}
    }
}


//static class Clients
//{
//    public static List<Client> Get()
//    {
//        return new List<Client>
//        { // no human involved
//            new Client
//            {
//                ClientName = "Silicon-only Client" ,
//                ClientId = "silicon" ,
//                Enabled = true ,
//                AccessTokenType = AccessTokenType.Reference,
//                Flow = Flows.ClientCredentials,
//                ClientSecrets = new List < Secret > { new Secret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256()) },
//                AllowedScopes = new List < string > { "api1" }
//            }
//        };
//    } }