using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wissance.Authorization.Config;
using Wissance.Authorization.Data;
using Wissance.Authorization.OpenId;
using Wissance.Authorization.Tests.Checkers;
using Xunit;

namespace Wissance.Authorization.Tests.OpenId
{
    /// <summary>
    ///     WARNING This tests USES our WORKING service
    ///     If there would be a lot of activity on a realm, it would be set off
    /// </summary>
    public class TestKeyCloakOpenIdAuthenticator
    {
        [Theory]
        [InlineData(TestUser, TestPassword, TestScope, true)]
        [InlineData("unKnown", "qwerty123", TestScope, false)]
        public void TestPublicClientAuthentication(string userName, string password, string scope, bool success)
        {
            IOpenIdAuthenticator authenticator = new KeyCloakOpenIdAuthenticator(_testPublicKeyCloakConfig, new LoggerFactory());
            TokenInfo token = GetToken(authenticator, userName, password, scope);
            if (success)
            {
                Assert.NotNull(token);
            }
            else
            {
                Assert.Null(token);
            }
        }

        [Theory]
        [InlineData(TestUser, TestPassword, TestScope, true)]
        [InlineData("unKnown", "qwerty123", TestScope, false)]
        public void TestPrivateClientAuthentication(string userName, string password, string scope, bool success)
        {
            IOpenIdAuthenticator authenticator = new KeyCloakOpenIdAuthenticator(_testPrivateKeyCloakConfig, new LoggerFactory());
            TokenInfo token = GetToken(authenticator, userName, password, scope);
            if (success)
            {
                Assert.NotNull(token);
            }
            else
            {
                Assert.Null(token);
            }
        }

        [Theory]
        [InlineData(TestUser, TestPassword, TestScope)]
        public void TestGetUserInfo(string userName, string password, string scope)
        {
            IOpenIdAuthenticator authenticator = new KeyCloakOpenIdAuthenticator(_testPrivateKeyCloakConfig, new LoggerFactory());
            TokenInfo token = GetToken(authenticator, userName, password, scope);
            Assert.NotNull(token);
            Task<UserInfo> getUserInfoTask = authenticator.GetUserInfoAsync(token.AccessToken, token.TokenType);
            getUserInfoTask.Wait();
            UserInfo actualUserInfo = getUserInfoTask.Result;
            Assert.NotNull(actualUserInfo);
            UserInfo expectedUserInfo = new UserInfo(actualUserInfo.Session, TestUser, "firstTestName lastTestName",
                                                     null, false, null);
            UserInfoChecker.Check(expectedUserInfo, actualUserInfo);
        }

        [Theory]
        [InlineData(TestUser, TestPassword, TestScope, true)]
        [InlineData(TestUser, TestPassword, TestScope, false)]
        public void TestTokenRefresh(string userName, string password, string scope, bool isPrivate)
        {
            KeyCloakServerConfig config = isPrivate ? _testPrivateKeyCloakConfig : _testPublicKeyCloakConfig;
            IOpenIdAuthenticator authenticator = new KeyCloakOpenIdAuthenticator(config, new LoggerFactory());
            TokenInfo token = GetToken(authenticator, userName, password, scope);
            Assert.NotNull(token);
            Task<TokenInfo> refreshTokenTask = authenticator.RefreshTokenAsync(token.RefreshToken);
            refreshTokenTask.Wait();
            TokenInfo refreshedToken = refreshTokenTask.Result;
            Assert.NotNull(refreshedToken);
            Assert.Equal(token.Session, refreshedToken.Session);
        }

        private TokenInfo GetToken(IOpenIdAuthenticator authenticator , string userName, string password, string scope)
        {
            Task<TokenInfo> authenticateTask = authenticator.AuthenticateAsync(userName, password, scope);
            authenticateTask.Wait();
            TokenInfo token = authenticateTask.Result;
            return token;
        }

        private const string AuthServerBaseUri = "https://auth.wissance.com";
        private const string TestRealm = "authtests";
        private const string TestPrivateClientId = "private_auth_test_client";
        private const string TestPrivateClientSecret = "4e5df6a5-a2c6-44af-b644-c1291fbe558e";
        private const string TestPublicClientId = "public_auth_test_client";
        private const string TestUser = "testuser";
        private const string TestPassword = "123";
        private const string TestScope = "profile";

        private readonly KeyCloakServerConfig _testPrivateKeyCloakConfig = new KeyCloakServerConfig(AuthServerBaseUri, TestRealm, 
                                                                                                    KeyCloakClientType.Confidential, 
                                                                                                    TestPrivateClientId, TestPrivateClientSecret);
        private readonly KeyCloakServerConfig _testPublicKeyCloakConfig = new KeyCloakServerConfig(AuthServerBaseUri, TestRealm,
                                                                                                   KeyCloakClientType.Public,
                                                                                                   TestPublicClientId, null);
    }
}
