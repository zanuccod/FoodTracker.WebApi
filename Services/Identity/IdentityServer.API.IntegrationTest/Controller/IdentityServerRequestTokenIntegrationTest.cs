using System.Net;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4;
using Xunit;

namespace IdentityServer.API.IntegrationTest.Controller
{
    public class IdentityServerRequestTokenIntegrationTest : IntegrationTestBase
    {
        [Fact]
        public async Task RequestTokenForLocalApiResource_CorrectCredentials_ShouldReturnBearerToken()
        {
            // Act
            var identityServerResponse = await RequestToken("demo", "demo", IdentityServerConstants.LocalApi.ScopeName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, identityServerResponse.HttpStatusCode);
            Assert.False(identityServerResponse.IsError);
            Assert.NotNull(identityServerResponse.AccessToken);
        }

        [Fact]
        public async Task RequestTokenForExternalApi_CorrectCredentials_ShouldReturnBearerToken()
        {
            // Act
            var identityServerResponse = await RequestToken("demo", "demo", "test_WebApi");

            // Assert
            Assert.Equal(HttpStatusCode.OK, identityServerResponse.HttpStatusCode);
            Assert.False(identityServerResponse.IsError);
            Assert.NotNull(identityServerResponse.AccessToken);
        }

        [Fact]
        public async Task RequestTokenForLocalApiResource_IncorrectCredentials_ShouldReturnBadRequest()
        {
            // Act
            var identityServerResponse = await RequestToken("demo_false", "demo_false", IdentityServerConstants.LocalApi.ScopeName);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, identityServerResponse.HttpStatusCode);
            Assert.True(identityServerResponse.IsError);
            Assert.Null(identityServerResponse.AccessToken);
        }

        [Fact]
        public async Task RequestTokenForExternalApiResource_IncorrectCredentials_ShouldReturnBadRequest()
        {
            // Act
            var identityServerResponse = await RequestToken("demo_false", "demo_false", "test_WebApi");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, identityServerResponse.HttpStatusCode);
            Assert.True(identityServerResponse.IsError);
            Assert.Null(identityServerResponse.AccessToken);
        }
    }
}
