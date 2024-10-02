using NUnit.Framework;
using MoveIT.Common;

namespace MoveIT.Tests.CommonTests
{
    [TestFixture]
    public class TokenManagerTests
    {
        private TokenManager _tokenManager;

        [SetUp]
        public void SetUp()
        {
            _tokenManager = new TokenManager();
        }

        [Test]
        public void SetToken_ShouldSetTokenCorrectly()
        {
            // Arrange
            var accessToken = "access-token";
            var refreshToken = "refresh-token";
            var expiresIn = 3600;

            // Act
            _tokenManager.SetToken(accessToken, refreshToken, expiresIn);

            // Assert
            Assert.IsNotNull(_tokenManager.Token);
            Assert.AreEqual(accessToken, _tokenManager.Token!.AccessToken);
            Assert.AreEqual(refreshToken, _tokenManager.Token.RefreshToken);
        }

        [Test]
        public void DeleteToken_ShouldRemoveToken()
        {
            // Arrange
            var accessToken = "access-token";
            var refreshToken = "refresh-token";
            var expiresIn = 3600;
            _tokenManager.SetToken(accessToken, refreshToken, expiresIn);

            // Act
            _tokenManager.DeleteToken();

            // Assert
            Assert.IsNull(_tokenManager.Token);
        }

        [Test]
        public void GetToken_ShouldReturnNull_WhenTokenNotSet()
        {
            // Act
            var token = _tokenManager.Token;

            // Assert
            Assert.IsNull(token);
        }

        [Test]
        public void GetToken_ShouldReturnValidToken_WhenTokenIsSet()
        {
            // Arrange
            var accessToken = "access-token";
            var refreshToken = "refresh-token";
            var expiresIn = 3600;
            _tokenManager.SetToken(accessToken, refreshToken, expiresIn);

            // Act
            var token = _tokenManager.Token;

            // Assert
            Assert.IsNotNull(token);
            Assert.AreEqual(accessToken, token!.AccessToken);
            Assert.AreEqual(refreshToken, token.RefreshToken);
        }
    }
}
