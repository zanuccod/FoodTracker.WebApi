using System.Threading.Tasks;
using IdentityServer.API.Controllers;
using IdentityServer.API.Domains;
using IdentityServer.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace IdentityServer.API.Test.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {
        private readonly Mock<IUserDataModel> mockUserDataModel;
        private readonly AccountController controller;

        public AccountControllerTest()
        {
            mockUserDataModel = new Mock<IUserDataModel>();
            var userDataModel = mockUserDataModel.Object;

            controller = new AccountController(userDataModel, new NullLogger<AccountController>());
        }

        [Test]
        public async Task RegisterUser_NullUser_ShouldReturnBadRequest()
        {
            // Act
            var result = await controller.RegisterUser(null);

            // Assert
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
        }

        [Test]
        public async Task RegisterUser_AlreadyExistUser_ShouldReturnConflict()
        {
            // Arrange
            // moq data model to return an already existing user
            mockUserDataModel.Setup(x => x.GetUser(It.IsAny<string>())).Returns(Task.FromResult(new User()));

            // Act
            var result = await controller.RegisterUser(new User());

            // Assert
            Assert.AreEqual(typeof(ConflictObjectResult), result.GetType());
        }

        [Test]
        public async Task RegisterUser_NewUser_ShouldReturnOk()
        {
            // Arrange
            // moq data model to return an already existing user
            mockUserDataModel.Setup(x => x.GetUser(It.IsAny<string>())).Returns(Task.FromResult<User>(null));

            // Act
            var result = await controller.RegisterUser(new User());

            // Assert
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        }

        [Test]
        public async Task DeleteUser_NullUser_ShouldReturnBadRequest()
        {
            // Act
            var result = await controller.DeleteUser(null);

            // Assert
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
        }

        [Test]
        public async Task DeleteUser_UserNotExist_ShouldReturnNotFound()
        {
            // Arrange
            // moq data model to return an already existing user
            mockUserDataModel.Setup(x => x.GetUser(It.IsAny<string>())).Returns(Task.FromResult<User>(null));

            // Act
            var result = await controller.DeleteUser("demo");

            // Assert
            Assert.AreEqual(typeof(NotFoundObjectResult), result.GetType());
        }

        [Test]
        public async Task RegisterUser_UserExsits_ShouldReturnOk()
        {
            // Arrange
            // moq data model to return an already existing user
            mockUserDataModel.Setup(x => x.GetUser(It.IsAny<string>())).Returns(Task.FromResult(new User()));

            // Act
            var result = await controller.DeleteUser("demo");

            // Assert
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        }
    }
}
