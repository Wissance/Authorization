using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Wissance.Authorization.Authentication;
using Wissance.Authorization.Config;
using Wissance.Authorization.Helpers;
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

        public static void AddSwaggerWithKeyCloakPasswordAuthentication(this ServiceCollection services, KeyCloakServerConfig config,
                                                                       IDictionary<string, string> defaultScopes)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(SwaggerSecurityDefinitionName, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Password = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri(KeyCloakHelper.GetAuthorizationUri(config.BaseUrl, config.Realm)),
                            TokenUrl = new Uri(KeyCloakHelper.GetTokenUri(config.BaseUrl, config.Realm)),
                            Scopes = defaultScopes
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = SwaggerSecurityDefinitionName
                            },
                            Scheme = SwaggerSecurityDefinitionName,
                            Name = SwaggerSecurityDefinitionName,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }

        /*public static IDictionary<string, string> ProfileScopeOptions = new Dictionary<string, string>()
        {
            {"profile", "Profile scope"}
        };*/

        private const string KeyCloakCodeTokenSchemeName = "code_token";
        private const string SwaggerSecurityDefinitionName = "oauth2";
    }
}
