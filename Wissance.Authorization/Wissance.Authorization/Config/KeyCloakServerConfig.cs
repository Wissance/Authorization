using System;
using System.Collections.Generic;
using System.Text;

namespace Wissance.Authorization.Config
{
    public class KeyCloakServerConfig
    {
        public KeyCloakServerConfig()
        {

        }

        public KeyCloakServerConfig(string baseUri, string realm, string clientId, string clientSecret)
        {
            BaseUrl = baseUri;
            Realm = realm;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string BaseUrl { get; set; }
        public string Realm { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
