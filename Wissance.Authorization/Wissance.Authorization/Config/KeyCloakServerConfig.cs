using System;
using System.Collections.Generic;
using System.Text;

namespace Wissance.Authorization.Config
{
    public enum KeyCloakClientType
    {
        Public,
        Confidential
    }

    public class KeyCloakServerConfig
    {
        public KeyCloakServerConfig()
        {

        }

        public KeyCloakServerConfig(string baseUri, string realm, KeyCloakClientType clientType, string clientId, string clientSecret)
        {
            BaseUrl = baseUri;
            Realm = realm;
            ClientType = clientType;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string BaseUrl { get; set; }
        public string Realm { get; set; }
        public KeyCloakClientType ClientType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
