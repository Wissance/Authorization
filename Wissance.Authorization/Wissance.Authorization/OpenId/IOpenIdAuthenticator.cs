using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wissance.Authorization.Data;

namespace Wissance.Authorization.OpenId
{
    public interface IOpenIdAuthenticator
    {
        Task<TokenInfo> AuthenticateAsync(string userName, string password, string scope);
        Task<UserInfo> GetUserInfoAsync(string accessToken, string tokenType);
        Task<TokenInfo> RefreshTokenAsync(string refreshToken);
    }
}
