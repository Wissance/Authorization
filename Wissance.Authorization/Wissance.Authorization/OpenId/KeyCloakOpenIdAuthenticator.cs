using System;
using System.Collections.Generic;
using System.Text;
using Wissance.Authorization.Data;

namespace Wissance.Authorization.OpenId
{
    public class KeyCloakOpenIdAuthenticator : IOpenIdAuthenticator
    {
        public TokenInfo Authenticate(string baseUrl, IDictionary<string, string> formParameters)
        {
            throw new NotImplementedException();
        }

        public UserInfo GetUserInfo(string baseUrl)
        {
            throw new NotImplementedException();
        }
    }
}
