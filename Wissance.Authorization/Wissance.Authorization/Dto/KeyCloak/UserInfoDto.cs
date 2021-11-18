using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Wissance.Authorization.Dto.KeyCloak
{
    internal class UserInfoDto
    {
        public UserInfoDto()
        {

        }

        public UserInfoDto(string sub, bool isEmailVerified, string[] roles, string name, string userName,
                           string firstName, string lastName)
        {
            Sub = sub;
            IsEmailVerified = isEmailVerified;
            Roles = roles;
            Name = name;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
        }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("email_verified")]
        public bool IsEmailVerified { get; set; }

        [JsonProperty("roles")]
        public string[] Roles { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("preferred_username")]
        public string UserName { get; set; }

        [JsonProperty("given_name")]
        public string FirstName { get; set; }

        [JsonProperty("family_name")]
        public string LastName { get; set; }
    }
}
