using System.Collections.Generic;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.Configuration
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                ///////////////////////////////////////////
                // Client for integration tests
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "test_ClientId",
                    ClientSecrets = { new Secret("test_client_key".Sha256()) },
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        LocalApi.ScopeName,
                        "test_WebApi"
                    }
                }
            };
        }
    }
}
