using System;
using System.Collections.Generic;
using System.Text;

namespace Wissance.Authorization.Data
{
    public class UserInfo
    {
        public UserInfo()
        {
        }

        public UserInfo(string userId, string session, string userName, string fullName, string[] roles, bool isEmailVerified, string email)
        {
            UserId = userId;
            Session = session;
            UserName = userName;
            FullName = fullName;
            Roles = roles;
            IsEmailVerified = isEmailVerified;
            Email = email;
        }
        
        public string UserId { get; set; }
        public string Session { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string[] Roles { get; set; }
        public bool IsEmailVerified { get; set; }
        public string Email { get; set; }
    }
}
