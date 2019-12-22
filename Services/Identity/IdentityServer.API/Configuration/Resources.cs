using System;
using System.Collections.Generic;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.Configuration
{
    public static class Resources
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                // local API
                new ApiResource(LocalApi.ScopeName),

                // use for testing
                new ApiResource("test_WebApi")
                {
                    ApiSecrets = { new Secret("test_web_api_key".Sha256()) }
                }
            };
        }
    }
}
