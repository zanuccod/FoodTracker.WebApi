using System;
using IdentityModel;
using IdentityServer.API.Domains;

namespace IdentityServer.API.IntegrationTest
{
    public static class PredefinedData
    {
        public readonly static string Password = "demo";

        public readonly static User[] Profiles = {
            new User { Username = "demo", Password = Password.ToSha256() },
            new User { Username = "tester@test.com", Password = Password },
            new User { Username = "author@test.com", Password = Password }
        };
    }
}
