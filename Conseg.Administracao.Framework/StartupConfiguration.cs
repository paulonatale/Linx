using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Framework
{
    public class StartupConfiguration
    {
        public bool UseMvc { get; set; }
        public bool UseWebapi { get; set; }
        public bool UseGenericAutoMapper { get; set; }
        public bool UseGenericController { get; set; }
        public bool UseGenericApiController { get; set; }
        public bool UseGenericODataController { get; set; }
        public bool UseCookieAuthentication { get; set; }
        public string NameOrConnectionString { get; set; }
        //public CookieAuthenticationOptions CookieAuthenticationOptions { get; set; }
        public bool UseExternalSignInCookie { get; set; }
        public bool UseMicrosoftAccountAuthentication { get; set; }
        public string MicrosoftClientId { get; set; }
        public string MicrosoftClientSecret { get; set; }
        public bool UseTwitterAuthentication { get; set; }
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }
        public bool UseFacebookAuthentication { get; set; }
        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }
        public bool UseGoogleAuthentication { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
    }
}
