using System.Net;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4;
using Xunit;

namespace IdentityServer.API.IntegrationTest.Controller
{
    public class IdentityServerRequestTokenIntegrationTest : IntegrationTestBase
    {
        [Theory]
        [InlineData(IdentityServerConstants.LocalApi.ScopeName)]
        [InlineData("test_WebApi")]
        public async Task RequestToken_CorrectCredentials_ShouldReturnBearerToken(string scope)
        {
            // Act
            var identityServerResponse = await RequestToken("demo", "demo", scope);

            // Assert
            Assert.Equal(HttpStatusCode.OK, identityServerResponse.HttpStatusCode);
            Assert.False(identityServerResponse.IsError);
            Assert.NotNull(identityServerResponse.AccessToken);
        }

        [Theory]
        [InlineData(IdentityServerConstants.LocalApi.ScopeName)]
        [InlineData("test_WebApi")]
        public async Task RequestToken_IncorrectCredentials_ShouldReturnBadRequest(string scope)
        {
            // Act
            var identityServerResponse = await RequestToken("demo_false", "demo_false", scope);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, identityServerResponse.HttpStatusCode);
            Assert.True(identityServerResponse.IsError);
            Assert.Null(identityServerResponse.AccessToken);
        }
    }
}
