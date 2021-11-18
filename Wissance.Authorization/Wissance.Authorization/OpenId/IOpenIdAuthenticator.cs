using System;
using System.Collections.Generic;
using System.Text;
using Wissance.Authorization.Data;

namespace Wissance.Authorization.OpenId
{
    public interface IOpenIdAuthenticator
    {
        TokenInfo Authenticate(IDictionary<string, string> formParameters);
        UserInfo GetUserInfo(string accessToken);
        TokenInfo RefreshToken(string refreshToken);
    }
}
