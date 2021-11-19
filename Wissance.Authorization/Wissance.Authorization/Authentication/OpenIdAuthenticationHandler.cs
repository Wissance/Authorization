using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wissance.Authorization.OpenId;

namespace Wissance.Authorization.Authentication
{
    public class OpenIdAuthenticationHandler : AuthenticationHandler<OpenIdAuthenticationSchemeOptions>
    {
        public OpenIdAuthenticationHandler(IOptionsMonitor<OpenIdAuthenticationSchemeOptions> options, 
                                           ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
                                           IOpenIdAuthenticator authenticator) 
            : base(options, logger, encoder, clock)
        {
            _authenticator = authenticator;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        private IOpenIdAuthenticator _authenticator;
    }
}
