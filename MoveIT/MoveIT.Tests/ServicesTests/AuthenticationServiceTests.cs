using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using MoveIT.Gateways.Contracts;
using MoveIT.Gateways.Contracts.Models;
using MoveIT.Services;
using MoveIT.Common.Helpers;

namespace MoveIT.Tests.ServicesTests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private Mock<IMoveITGateway> _mockGateway;
        private Mock<Action<AuthenticationResponseModel>> _mockPersistToken;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void Setup()
        {
            _mockGateway = new Mock<IMoveITGateway>();
            _mockPersistToken = new Mock<Action<AuthenticationResponseModel>>();
            _authenticationService = new AuthenticationService(_mockGateway.Object, _mockPersistToken.Object);
        }

        [Test]
        public async Task Authenticate_WhenDataIsNotNull_And_ShouldCallPersistToken()
        {
            // Arrange
            var authResponse = new AuthenticationResponseModel { AccessToken = "test-token" };
            var expectedResult = Result<AuthenticationResponseModel>.ToResult(authResponse);

            _mockGateway.Setup(g => g.Authenticate()).ReturnsAsync(expectedResult);

            // Act
            var result = await _authenticationService.Authenticate();

            // Assert
            _mockPersistToken.Verify(p => p(It.Is<AuthenticationResponseModel>(data => data == authResponse)), Times.Once);
            Assert.AreEqual(expectedResult, result); 
        }

        [Test]
        public async Task Authenticate_WhenDataIsNull_And_ShouldNotCallPersistToken()
        {
            // Arrange
            var expectedResult = Result<AuthenticationResponseModel>.ToResult(null);

            _mockGateway.Setup(g => g.Authenticate()).ReturnsAsync(expectedResult);

            // Act
            var result = await _authenticationService.Authenticate();

            // Assert
            _mockPersistToken.Verify(p => p(It.IsAny<AuthenticationResponseModel>()), Times.Never);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task ReAuthenticate_WhenDataIsNotNull_And_ShouldCallPersistToken()
        {
            // Arrange
            var authResponse = new AuthenticationResponseModel { RefreshToken = "refresh-token" };
            var expectedResult = Result<AuthenticationResponseModel>.ToResult(authResponse);
            var refreshToken = "valid-refresh-token";

            _mockGateway.Setup(g => g.ReAuthenticate(refreshToken)).ReturnsAsync(expectedResult);

            // Act
            var result = await _authenticationService.ReAuthenticate(refreshToken);

            // Assert
            _mockPersistToken.Verify(p => p(It.Is<AuthenticationResponseModel>(data => data == authResponse)), Times.Once);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task ReAuthenticate_WhenDataIsNull_And_ShouldNotCallPersistToken()
        {
            // Arrange
            var expectedResult = Result<AuthenticationResponseModel>.ToResult(null);
            var refreshToken = "valid-refresh-token";

            _mockGateway.Setup(g => g.ReAuthenticate(refreshToken)).ReturnsAsync(expectedResult);

            // Act
            var result = await _authenticationService.ReAuthenticate(refreshToken);

            // Assert
            _mockPersistToken.Verify(p => p(It.IsAny<AuthenticationResponseModel>()), Times.Never);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
