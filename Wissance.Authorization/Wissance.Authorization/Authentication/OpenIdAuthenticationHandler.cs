using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wissance.Authorization.Data;
using Wissance.Authorization.OpenId;

namespace Wissance.Authorization.Authentication
{
    public class OpenIdAuthenticationHandler : AuthenticationHandler<OpenIdAuthenticationSchemeOptions>
    {
        public OpenIdAuthenticationHandler(IOptionsMonitor<OpenIdAuthenticationSchemeOptions> options, 
                                           ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock,
                                           IOpenIdAuthenticator authenticator) 
            : base(options, loggerFactory, encoder, clock)
        {
            _authenticator = authenticator;
            _logger = loggerFactory.CreateLogger<OpenIdAuthenticationHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                // 1. Get http Authorization header
                if (!Request.Headers.ContainsKey(AuthorizationHeader))
                {
                    return AuthenticateResult.Fail($"There is no \"{AuthorizationHeader}\" header");
                }
                string authorizationKeyValue = Request.Headers[AuthorizationHeader].ToString();

                // 2. Check presenсe of Bearer Token
                if (!authorizationKeyValue.ToLowerInvariant().Contains(TokenAuthorizationScheme.ToLowerInvariant()))
                {
                    return AuthenticateResult.Fail("Non supporting authorization scheme, expecting \"Bearer + token\"!");
                }

                string[] keyValueParts = authorizationKeyValue.Split(" ");
                if (keyValueParts.Length != 2)
                {
                    return AuthenticateResult.Fail("Non supporting token format, if you feel that this is error please create issue on github");
                }
                // 3. Get token itself
                string token = keyValueParts[1];
                UserInfo userInfo = await _authenticator.GetUserInfoAsync(token, TokenAuthorizationScheme);
                if (userInfo == null)
                {
                    return AuthenticateResult.Fail("Token is invalid");
                }
                Response.StatusCode = (int)HttpStatusCode.OK;

                IList<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo.UserName),
                    new Claim(ClaimTypes.Name, userInfo.FullName)
                    //new Claim(ClaimTypes.Email, null),
                };

                if (userInfo.Email != null)
                    claims.Add(new Claim(ClaimTypes.Email, userInfo.Email));
                // todo: umv: add Custom claims (EmailVerified & so on )
                if (userInfo.Roles != null)
                {
                    foreach (string role in userInfo.Roles)
                        claims.Add(new Claim(ClaimTypes.Role, role));
                }
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, nameof(OpenIdAuthenticationHandler));
                AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred during user handle authentication: {e.Message}");
                throw;
            }
        }

        private const string AuthorizationHeader = "Authorization";
        private const string TokenAuthorizationScheme = "Bearer";

        private readonly IOpenIdAuthenticator _authenticator;
        private readonly ILogger<OpenIdAuthenticationHandler> _logger;
    }
}
