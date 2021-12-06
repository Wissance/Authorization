using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wissance.Authorization.Config;
using Wissance.Authorization.Data;
using Wissance.Authorization.Dto.KeyCloak;
using Wissance.Authorization.Helpers;

namespace Wissance.Authorization.OpenId
{
    public class KeyCloakOpenIdAuthenticator : IOpenIdAuthenticator
    {
        public KeyCloakOpenIdAuthenticator(KeyCloakServerConfig config, ILoggerFactory loggerFactory)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<KeyCloakOpenIdAuthenticator>();
        }

        public async Task<TokenInfo> AuthenticateAsync(string userName, string password, string scope)
        {
            FormUrlEncodedContent content = KeyCloakHelper.GetAccessTokenRequestBody(_config.ClientType,
                                            _config.ClientId, _config.ClientSecret, scope, userName, password);
            TokenInfo tokenInfo = await GetTokenAsync(_config.Realm, content, "get access token");
            return tokenInfo;
        }

        public async Task<UserInfo> GetUserInfoAsync(string accessToken, string tokenType)
        {
            try
            {
                string url = KeyCloakHelper.GetUserInfoUri(_config.BaseUrl, _config.Realm);
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogDebug($"Can't get user info: {response.StatusCode} with body: {responseBody}");
                        return null;
                    }
                    UserInfoDto kcUserInfo = JsonConvert.DeserializeObject<UserInfoDto>(responseBody);
                    if (kcUserInfo == null)
                    {
                        _logger.LogError("Keycloak user info is null (can't be deserialized)");
                        return null;
                    }

                    // todo: umv: add pass and parse e-mail to userinfo
                    return new UserInfo(kcUserInfo.Id, kcUserInfo.Sub, kcUserInfo.UserName, kcUserInfo.Name, 
                                        kcUserInfo.Roles, kcUserInfo.IsEmailVerified, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during getting user info: {ex.Message}");
                return null;
            }
        }

        public async Task<TokenInfo> RefreshTokenAsync(string refreshToken)
        {
            FormUrlEncodedContent content = KeyCloakHelper.GetRefreshTokenRequestBody(_config.ClientType,
                                            _config.ClientId, _config.ClientSecret, refreshToken);
            TokenInfo tokenInfo = await GetTokenAsync(_config.Realm, content, "refresh token");
            return tokenInfo;
        }

        private async Task<TokenInfo> GetTokenAsync(string realm, FormUrlEncodedContent formContent, string operation)
        {
            try
            {
                string url = KeyCloakHelper.GetTokenUri(_config.BaseUrl, realm);
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.PostAsync(url, formContent);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogDebug($"Can't \"{operation}\": {response.StatusCode} with body: {responseBody}");
                        return null;
                    }

                    TokenDto tokenData = JsonConvert.DeserializeObject<TokenDto>(responseBody);
                    if (tokenData == null)
                    {
                        _logger.LogError("Keycloak token is null (can't be deserialized)");
                        return null;
                    }

                    return new TokenInfo(tokenData.SessionState, tokenData.TokenType, tokenData.AccessToken, tokenData.TokenExpiration, 
                                         tokenData.RefreshToken, tokenData.RefreshExpiration);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during \"{operation}\" from: {_config.BaseUrl}, error: {ex.Message}");
                return null;
            }
        }

        private readonly KeyCloakServerConfig _config;
        private readonly ILogger<KeyCloakOpenIdAuthenticator> _logger;
    }
}
