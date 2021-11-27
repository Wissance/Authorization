using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Wissance.Authorization.Config;

namespace Wissance.Authorization.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSwaggerWithKeyCloakAuthentication(this IApplicationBuilder app, string appName, 
                                                                KeyCloakServerConfig config, string[] scopesOptions)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", appName);
                c.OAuthClientId(config.ClientId);
                //c.OAuthScopes(new string[] { "profile" });
                c.OAuthScopes(scopesOptions);
                c.OAuthRealm(config.Realm);
                c.OAuthUsePkce();
            });
        }
    }
}
