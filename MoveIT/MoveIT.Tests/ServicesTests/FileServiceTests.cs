using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using MoveIT.Gateways.Contracts;
using MoveIT.Services;
using MoveIT.Common.Helpers;

namespace MoveIT.Tests.ServicesTests
{
    [TestFixture]
    public class FileServiceTests
    {
        private Mock<IMoveITGateway> _mockGateway;
        private FileService _fileService;

        [SetUp]
        public void Setup()
        {
            _mockGateway = new Mock<IMoveITGateway>();
            _fileService = new FileService(_mockGateway.Object);
        }

        [Test]
        public async Task Upload_ShouldCallUploadFileToDirectory_WhenFileIsReadSuccessfully()
        {
            // Arrange
            var fileData = new byte[] { 1, 2, 3 };
            var fileName = "test.txt";
            int directoryId = 123;

            var mockFileReader = new Func<Task<(byte[] File, string FileName)>>(() =>
                Task.FromResult((fileData, fileName))
            );

            var expectedResult = Result.ToEmptyResult();

            _mockGateway.Setup(g => g.UploadFileToDirectory(fileData, fileName, directoryId))
                        .ReturnsAsync(expectedResult);

            // Act
            var result = await _fileService.Upload(mockFileReader, directoryId);

            // Assert
            _mockGateway.Verify(g => g.UploadFileToDirectory(fileData, fileName, directoryId), Times.Once);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task Upload_ShouldReturnError_WhenFileReaderReturnsNullFile()
        {
            // Arrange
            byte[] fileData = null;
            var fileName = "test.txt";
            int directoryId = 123;

            var mockFileReader = new Func<Task<(byte[] File, string FileName)>>(() =>
                Task.FromResult((fileData, fileName))
            );

            var expectedResult = Result.ToVoidError("File is null");

            _mockGateway.Setup(g => g.UploadFileToDirectory(It.IsAny<byte[]>(), It.IsAny<string>(), directoryId))
                        .ReturnsAsync(expectedResult);

            // Act
            var result = await _fileService.Upload(mockFileReader, directoryId);

            // Assert
            _mockGateway.Verify(g => g.UploadFileToDirectory(It.IsAny<byte[]>(), It.IsAny<string>(), directoryId), Times.Once);
            Assert.AreEqual(expectedResult.ErrorMessage, result.ErrorMessage);
        }

        [Test]
        public async Task Upload_ShouldNotCallGateway_WhenFileReaderThrowsException()
        {
            // Arrange
            int directoryId = 123;

            var mockFileReader = new Func<Task<(byte[] File, string FileName)>>(() =>
            {
                throw new Exception("Error reading file");
            });

            // Act
            Assert.ThrowsAsync<Exception>(async () => await _fileService.Upload(mockFileReader, directoryId));

            // Assert
            _mockGateway.Verify(g => g.UploadFileToDirectory(It.IsAny<byte[]>(), It.IsAny<string>(), directoryId), Times.Never);
        }
    }
}
