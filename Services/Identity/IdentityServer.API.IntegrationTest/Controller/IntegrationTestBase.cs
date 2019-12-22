using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace IdentityServer.API.IntegrationTest.Controller
{
    public class IntegrationTestBase : IDisposable
    {
        private readonly TestServer _server;
        protected readonly HttpClient _client;

        protected const string serverBaseAddress = "https://localhost:5001";
        protected readonly string requestTokenAddress = string.Join("/", serverBaseAddress, "connect/token");

        public IntegrationTestBase()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        protected async Task<TokenResponse> RequestToken(string username, string password, string scope)
        {
            return await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = requestTokenAddress,
                GrantType = GrantTypes.ResourceOwnerPassword.First(),

                ClientId = "test_ClientId",
                ClientSecret = "test_client_key",
                Scope = scope,

                UserName = username,
                Password = password.ToSha256()
            });
        }
    }
}
