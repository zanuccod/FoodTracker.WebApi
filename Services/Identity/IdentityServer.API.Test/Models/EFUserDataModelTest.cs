using System;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using IdentityServer.API.Models;
using IdentityServer.API.Domains;

namespace IdentityServer.API.Test.Models
{
    [TestFixture]
    public class EFUserDataModelTest
    {
        private EFUserDataModel dataModel;

        [SetUp]
        public void BeforeEachTest()
        {
            var options = new DbContextOptionsBuilder()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;
            dataModel = new EFUserDataModel(options);
        }

        [Test]
        public async Task InsertUser()
        {
            // Arrange
            var item = new User { Username = "username_test", Password = "password_test" };

            // Act
            await dataModel.InsertUser(item);

            // Assert
            Assert.AreEqual(1, dataModel.GetUsersList().Result.Count);
        }

        [Test]
        public async Task GetAllUsers()
        {
            // Arrange
            var itemCount = 10;
            for (var i = 0; i < itemCount; i++)
            {
                await dataModel.InsertUser(new User { Username = "username_test_" + i, Password = "password_test" });
            }

            // Act
            var items = await dataModel.GetUsersList();

            // Assert
            Assert.AreEqual(itemCount, items.Count);
        }

        [Test]
        public async Task GetUser()
        {
            // Arrange
            var item = new User { Username = "username_test", Password = "password_test" };
            await dataModel.InsertUser(item);

            // Act
            var item2 = await dataModel.GetUser(item.Username);

            // Assert
            Assert.True(item.Equals(item2));
        }

        [Test]
        public async Task UpdateUser()
        {
            // Arrange
            const string newPassword = "password_test_1";

            var item = new User { Username = "username_test", Password = "password_test" };
            await dataModel.InsertUser(item);

            // Act
            item.Password = newPassword;
            await dataModel.UpdateUser(item);

            var item2 = await dataModel.GetUser(item.Username);

            // Assert
            Assert.AreEqual(newPassword, item2.Password);
        }

        [Test]
        public async Task DeleteUser()
        {
            // Arrange
            var item = new User { Username = "username_test", Password = "password_test" };
            await dataModel.InsertUser(item);

            // Act
            await dataModel.DeleteUser(item.Username);

            // Assert
            Assert.AreEqual(0, dataModel.GetUsersList().Result.Count);
        }
    }
}
