using System.Threading.Tasks;
using IdentityServer.API.Controllers;
using IdentityServer.API.Domains;
using IdentityServer.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace IdentityServer.API.Test.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {
        Mock<IUserDataModel> mockUserDataModel;
        private IUserDataModel userDataModel;
        private AccountController controller;

        [SetUp]
        public void BeforeEachTest()
        {
            mockUserDataModel = new Mock<IUserDataModel>();
            userDataModel = mockUserDataModel.Object;
            controller = new AccountController(userDataModel);
        }

        [Test]
        public async Task RegisterUser_NullUser_ShouldReturnBadRequest()
        {
            // Act
            var result = await controller.RegisterUser(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
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
            Assert.IsInstanceOf<ConflictObjectResult>(result);
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
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteUser_NullUser_ShouldReturnBadRequest()
        {
            // Act
            var result = await controller.DeleteUser(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task DeleteUser_UserNotExist_ShouldReturnNotFound()
        {
            // Arrange
            // moq data model to return an already existing user
            mockUserDataModel.Setup(x => x.GetUser(It.IsAny<string>())).Returns(Task.FromResult<User>(null));

            // Act
            var result = await controller.DeleteUser(new User());

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task RegisterUser_UserExsits_ShouldReturnOk()
        {
            // Arrange
            // moq data model to return an already existing user
            mockUserDataModel.Setup(x => x.GetUser(It.IsAny<string>())).Returns(Task.FromResult(new User()));

            // Act
            var result = await controller.DeleteUser(new User());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
