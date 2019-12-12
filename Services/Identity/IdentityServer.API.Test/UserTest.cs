using NUnit.Framework;
using IdentityServer.API.Domains;

namespace IdentityServer.API.Test
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void EqualsToNull_ShouldReturnFalse()
        {
            // Arrange
            var user = new User();

            // Act
            var result = user.Equals(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void EqualsToUserWithSameProperties_ShouldReturnTrue()
        {
            // Arrange
            var user = new User { Username = "username_test", Password = "psw_test" };
            var user_1 = new User { Username = "username_test", Password = "psw_test" };

            // Act
            var result = user.Equals(user_1);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void EqualsToUserWithDifferentProperties_ShouldReturnTrue()
        {
            // Arrange
            var user = new User { Username = "username_test", Password = "psw_test" };
            var user_1 = new User { Username = "username_test_1", Password = "psw_test_1" };

            // Act
            var result = user.Equals(user_1);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
