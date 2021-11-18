using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wissance.Authorization.Config;
using Wissance.Authorization.Data;
using Wissance.Authorization.Dto.KeyCloak;

namespace Wissance.Authorization.OpenId
{
    public class KeyCloakOpenIdAuthenticator : IOpenIdAuthenticator
    {
        public KeyCloakOpenIdAuthenticator(KeyCloakServerConfig config, ILoggerFactory loggerFactory)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<KeyCloakOpenIdAuthenticator>();
        }

        public TokenInfo Authenticate(IDictionary<string, string> formParameters)
        {
            throw new NotImplementedException();
        }

        public UserInfo GetUserInfo(string accessToken)
        {
            throw new NotImplementedException();
        }

        public TokenInfo RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        private async Task<TokenInfo> GetTokenAsync(string realm, FormUrlEncodedContent formContent, string operation)
        {
            try
            {
                string url = KeyCloakUriHelper.GetTokenUri(_config.BaseUrl, realm);
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.PostAsync(url, formContent);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogDebug($"Can't {operation}: {response.StatusCode} with body: {responseBody}");
                        return null;
                    }

                    TokenDto tokenData = JsonConvert.DeserializeObject<TokenDto>(responseBody);
                    if (tokenData == null)
                    {
                        _logger.LogError("Keycloak token is null (can't be deserialized)");
                        return null;
                    }

                    return new TokenInfo(tokenData.SessionState, tokenData.AccessToken, tokenData.TokenExpiration, 
                                         tokenData.RefreshToken, tokenData.RefreshExpiration);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during {operation} from: {_config.BaseUrl}, error: {ex.Message}");
                return null;
            }
        }

        private readonly KeyCloakServerConfig _config;
        private readonly ILogger<KeyCloakOpenIdAuthenticator> _logger;
    }
}
