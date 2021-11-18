using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Wissance.Authorization.Dto.KeyCloak
{
    internal class TokenDto
    {
        public TokenDto()
        {

        }

        public TokenDto(string accessToken, int tokenExpiration, string refreshToken, int refreshExpiration,
                        string tokenType, int policy, string sessionState, string scope)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            TokenExpiration = tokenExpiration;
            RefreshExpiration = refreshExpiration;
            TokenType = tokenType;
            Policy = policy;
            SessionState = sessionState;
            Scope = scope;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int TokenExpiration { get; set; }

        [JsonProperty("refresh_expires_in")]
        public int RefreshExpiration { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("not-before-policy")]
        public int Policy { get; set; }

        [JsonProperty("session_state")]
        public string SessionState { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
