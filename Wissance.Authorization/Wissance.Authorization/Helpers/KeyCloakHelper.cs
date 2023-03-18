using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Wissance.Authorization.Config;

namespace Wissance.Authorization.Helpers
{
    public static class KeyCloakHelper
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

        public static FormUrlEncodedContent GetAccessTokenRequestBody(KeyCloakClientType clientType, string clientId, 
                                                                      string clientSecret, string scope, 
                                                                      string username, string password)
        {
            IList<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(ClientId, clientId),
                new KeyValuePair<string, string>(GrantType, PasswordGrantType),
                new KeyValuePair<string, string>(Scope, scope),
                new KeyValuePair<string, string>(Username, username),
                new KeyValuePair<string, string>(Password, password)
            };

            if (clientType == KeyCloakClientType.Confidential)
            {
                body.Add(new KeyValuePair<string, string>(ClientSecret, clientSecret));
            }

            return new FormUrlEncodedContent(body);
        }

        public static FormUrlEncodedContent GetAccessTokenRequestBody(KeyCloakClientType clientType, string clientId,
                                                                      string clientSecret, string redirectUri,
                                                                      string authorizationCode)
        {
            IList<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(AuthCode, authorizationCode),
                new KeyValuePair<string, string>(ClientId, clientId),
                new KeyValuePair<string, string>(RedirectUri, redirectUri),
                new KeyValuePair<string, string>(GrantType, AuthCodeGrantType)
            };

            if (clientType == KeyCloakClientType.Confidential)
            {
                body.Add(new KeyValuePair<string, string>(ClientSecret, clientSecret));
            }

            return new FormUrlEncodedContent(body);
        }

        public static FormUrlEncodedContent GetRefreshTokenRequestBody(KeyCloakClientType clientType, string clientId,
                                                                       string clientSecret, string refreshToken)
        {
            IList<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(ClientId, clientId),
                new KeyValuePair<string, string>(GrantType, TokenRefreshGrantType),
                new KeyValuePair<string, string>(RefreshToken, refreshToken)
            };

            if (clientType == KeyCloakClientType.Confidential)
            {
                body.Add(new KeyValuePair<string, string>(ClientSecret, clientSecret));
            }

            return new FormUrlEncodedContent(body);
        }

        // URL templates
        private const string TokenEndpointTemplate = "{0}/auth/realms/{1}/protocol/openid-connect/token";
        // i.e. http://auth.wissance-test.com:8080/auth/realms/master/protocol/openid-connect/token"
        private const string UserInfoEndpointTemplate = "{0}/auth/realms/{1}/protocol/openid-connect/userinfo";
        // i.e. http://auth.wissance-test.com:8080/auth/realms/master/protocol/openid-connect/userinfo
        private const string AuthorizationEndpointTemplate = "{0}/auth/realms/{1}/protocol/openid-connect/auth";
        // i.e. http://auth.wissance-test.com:8080/auth/realms/master/protocol/openid-connect/auth

        // Body parameters
        private const string ClientId = "client_id";
        private const string ClientSecret = "client_secret";
        private const string GrantType = "grant_type";
        private const string RefreshToken = "refresh_token";
        private const string Username = "username";
        private const string Password = "password";
        private const string Scope = "scope";
        private const string AuthCode = "code";
        private const string RedirectUri = "redirect_uri";
        public const string ProfileScope = "profile";
        public const string PasswordGrantType = "password";
        public const string AuthCodeGrantType = "authorization_code";
        public const string TokenRefreshGrantType = "refresh_token";

    }
}
