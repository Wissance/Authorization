using System;
using System.Collections.Generic;
using System.Text;

namespace Wissance.Authorization.Helpers
{
    public static class KeyCloakUriHelper
    {
        public static string GetTokenUri(string baseUri, string realm)
        {
            return string.Format(TokenEndpointTemplate, baseUri, realm);
        }

        public static string GetUserInfoUri(string baseUri, string realm)
        {
            return string.Format(UserInfoEndpointTemplate, baseUri, realm);
        }

        public static string GetAuthorizationUri(string baseUri, string realm)
        {
            return string.Format(AuthorizationEndpointTemplate, baseUri, realm);
        }

        private const string TokenEndpointTemplate = "{0}/auth/realms/{1}/protocol/openid-connect/token";
        // i.e. http://auth.waveslogic.com:8080/auth/realms/master/protocol/openid-connect/token"

        private const string UserInfoEndpointTemplate = "{0}/auth/realms/{1}/protocol/openid-connect/userinfo";
        // i.e. http://auth.waveslogic.com:8080/auth/realms/master/protocol/openid-connect/userinfo

        private const string AuthorizationEndpointTemplate = "{0}/auth/realms/{1}/protocol/openid-connect/auth";
        // i.e. http://auth.waveslogic.com:8080/auth/realms/master/protocol/openid-connect/auth
    }
}
