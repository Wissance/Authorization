using System;
using System.Collections.Generic;
using System.Text;

namespace Wissance.Authorization.Data
{
    public class TokenInfo
    {
        public TokenInfo()
        {
        }

        public TokenInfo(string session, string tokenType, string accessToken, int accessTokenExpiration, 
                         string refreshToken, int refreshTokenExpiration)
        {
            Session = session;
            TokenType = tokenType;
            AccessToken = accessToken;
            AccessTokenExpiration = accessTokenExpiration;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
        }

        public string Session { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
