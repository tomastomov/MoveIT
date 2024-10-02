using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MoveIT.Gateways;
using MoveIT.Gateways.Contracts.Models;
using MoveIT.Common.Contracts;
using MoveIT.Common.Extensions;
using MoveIT.Common.Contracts.Models;
using MoveIT.Tests.GatewaysTests;

namespace MoveIT.Tests.GatewayTests
{
    [TestFixture]
    public class MoveITGatewayTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<ITokenManager> _mockTokenManager;
        private Mock<IOptions<MoveITOptions>> _mockOptions;
        private MoveITGateway _gateway;
        private MoveITOptions _options;

        [SetUp]
        public void Setup()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockTokenManager = new Mock<ITokenManager>();
            _mockOptions = new Mock<IOptions<MoveITOptions>>();

            _options = new MoveITOptions
            {
                API_USER = "testuser",
                API_PASSWORD = "testpassword",
                AUTH_URL = "https://example.com/auth",
                UPLOAD_FILE_TO_DIRECTORY_URL = "https://example.com/upload/{0}"
            };

            _mockOptions.Setup(o => o.Value).Returns(_options);

            _gateway = new MoveITGateway(_mockHttpClientFactory.Object, _mockTokenManager.Object, _mockOptions.Object);
        }

        [Test]
        public async Task Authenticate_ShouldReturnSuccess_WhenValidCredentials()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"access_token\": \"test-token\"}")
            };

            var handler = new MockDelegatingHandler(response);

            var client = new HttpClient(handler);
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var result = await _gateway.Authenticate();

            // Assert
            Assert.AreEqual("test-token", result.Data.AccessToken);
        }

        [Test]
        public async Task ReAuthenticate_ShouldReturnSuccess_WhenValidRefreshToken()
        {
            // Arrange
            var refreshToken = "valid-refresh-token";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"access_token\": \"test-token\"}")
            };
            var handler = new MockDelegatingHandler(response);

            var client = new HttpClient(handler);
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var result = await _gateway.ReAuthenticate(refreshToken);

            // Assert
            Assert.AreEqual("test-token", result.Data.AccessToken);
        }

        [Test]
        public async Task UploadFileToDirectory_ShouldReturnResult_WhenValidFileUpload()
        {
            // Arrange
            var fileData = new byte[] { 1, 2, 3 };
            var fileName = "test.txt";
            int directoryId = 123;

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var handler = new MockDelegatingHandler(response);

            var client = new HttpClient(handler);
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            _mockTokenManager.Setup(t => t.Token).Returns(new JwtToken("test-token", "refresh-token", 500));

            // Act
            var result = await _gateway.UploadFileToDirectory(fileData, fileName, directoryId);

            // Assert
            _mockHttpClientFactory.Verify(f => f.CreateClient(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task UploadFileToDirectory_ShouldReturnError_WhenRequestFails()
        {
            // Arrange
            var fileData = new byte[] { 1, 2, 3 };
            var fileName = "test.txt";
            int directoryId = 123;

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var handler = new MockDelegatingHandler(response);

            var client = new HttpClient(handler);
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            _mockTokenManager.Setup(t => t.Token).Returns(new JwtToken("test-token", "refresh-token", 500));

            // Act
            var result = await _gateway.UploadFileToDirectory(fileData, fileName, directoryId);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest.ToMessage(), result.ErrorMessage);
        }
    }
}
