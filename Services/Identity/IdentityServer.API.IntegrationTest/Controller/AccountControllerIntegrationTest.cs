using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer.API.Domains;
using IdentityServer.API.IntegrationTest.Controller;
using IdentityServer4;
using Newtonsoft.Json;
using Xunit;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace IdentityServer.API.IntegrationTest.Controllers
{
    public class AccountControllerIntegrationTest : IntegrationTestBase
    {
        [Fact]
        public async Task GetUsers_WithoutToken_ShouldReturnUnauthorized()
        {
            // Act
            var response = await _client.GetAsync(string.Join("/", serverBaseAddress, "api/account/get-users"));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_WithToken_ShouldReturnUsersList()
        {
            // Arrange
            var identityServerResponse = await RequestToken("demo", "demo", IdentityServerConstants.LocalApi.ScopeName);
            _client.SetBearerToken(identityServerResponse.AccessToken);

            // Act
            var response = await _client.GetAsync(string.Join("/", serverBaseAddress, "api/account/get-users"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<User>>(result);
            Assert.NotNull(list.Find(x => x.Username.Equals("demo")));
        }

        [Fact]
        public async Task RegisterUser_NewUser_ShouldAddNewUser()
        {
            // Arrange
            var user = new User { Username = "new_demo", Password = "new_demo" };
            var content = new StringContent(JsonConvert.SerializeObject(user));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync(string.Join("/", serverBaseAddress, "api/account/register-user"), content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_WithoutToken_ShouldReturnUnauthorized()
        {
            // Act
            var response = await _client.DeleteAsync(string.Join("/", serverBaseAddress, "api/account/delete-user", "demo"));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_WithToken_ExistUser_ShouldDeleteUser()
        {
            // Arrange
            var identityServerResponse = await RequestToken("demo", "demo", IdentityServerConstants.LocalApi.ScopeName);
            _client.SetBearerToken(identityServerResponse.AccessToken);

            // Act
            var response = await _client.DeleteAsync(string.Join("/", serverBaseAddress, "api/account/delete-user", "demo"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
