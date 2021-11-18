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

        public UserInfo(string session, string userName, string[] roles)
        {
            Session = session;
            UserName = userName;
            Roles = roles;
        }

        public string Session { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; } 
    }
}
