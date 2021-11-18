using System;
using System.Collections.Generic;
using System.Text;
using Wissance.Authorization.Data;

namespace Wissance.Authorization.OpenId
{
    public class KeyCloakOpenIdAuthenticator : IOpenIdAuthenticator
    {
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
    }
}
