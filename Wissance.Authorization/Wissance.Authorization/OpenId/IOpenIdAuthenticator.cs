using System;
using System.Collections.Generic;
using System.Text;
using Wissance.Authorization.Data;

namespace Wissance.Authorization.OpenId
{
    public interface IOpenIdAuthenticator
    {
        TokenInfo Authenticate(string baseUrl, IDictionary<string, string> formParameters);
        UserInfo GetUserInfo(string baseUrl);
    }
}
