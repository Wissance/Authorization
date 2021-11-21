using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wissance.Authorization.Authentication;
using Wissance.Authorization.Config;
using Wissance.Authorization.OpenId;

namespace Wissance.Authorization.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddKeyCloak(this ServiceCollection services, KeyCloakServerConfig config)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>();
            services.AddScoped<IOpenIdAuthenticator>(s => new KeyCloakOpenIdAuthenticator(config, loggerFactory));

            services.AddAuthentication(KeyCloakCodeTokenSchemeName)
                    .AddScheme<OpenIdAuthenticationSchemeOptions, OpenIdAuthenticationHandler>(KeyCloakCodeTokenSchemeName,
                        options => { });
        }

        private const string KeyCloakCodeTokenSchemeName = "code_token";
    }
}
