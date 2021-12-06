using System;
using System.Collections.Generic;
using System.Text;
using Wissance.Authorization.Data;
using Xunit;

namespace Wissance.Authorization.Tests.Checkers
{
    internal static class UserInfoChecker
    {
        public static void Check(UserInfo expected, UserInfo actual)
        {
            Assert.Equal(expected.UserId, actual.UserId);
            Assert.Equal(expected.UserName, actual.UserName);
            Assert.Equal(expected.FullName, actual.FullName);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.IsEmailVerified, actual.IsEmailVerified);
            if (expected.Roles == null)
            {
                Assert.Null(actual.Roles);
            }
        }
    }
}
