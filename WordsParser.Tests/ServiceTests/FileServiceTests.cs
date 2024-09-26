using Microsoft.Extensions.FileProviders;
using Moq;
using WordsParser.Infrastructure.Services;

namespace WordsParser.Tests.ServiceTests
{
    public class FileServiceTests
    {
        private readonly FileService _fileService;

        public FileServiceTests()
        {
            _fileService = new FileService();
        }

        [Fact]
        public void ThrowIfFileNotExists_ShouldThrowException_WhenFileDoesNotExist()
        {
            // Arrange
            var nonExistentFilePath = "nonexistent.txt";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _fileService.ThrowIfFileNotExists(nonExistentFilePath));
        }

        [Fact]
        public void ThrowIfFileNotExists_ShouldNotThrowException_WhenFileExists()
        {
            // Arrange
            var tempFilePath = Path.GetTempFileName();

            // Act & Assert
            _fileService.ThrowIfFileNotExists(tempFilePath);

            File.Delete(tempFilePath);
        }

        [Fact]
        public void GetFileMBytesSize_ShouldReturnCorrectSize()
        {
            // Arrange
            const long fileSizeInBytes = 1048576; // 1 MB
            var tempFilePath = Path.GetTempFileName();

            CreateFakeAnySizeFile(tempFilePath, fileSizeInBytes);

            var fileInfo = new FileInfo(tempFilePath);

            // Act
            var result = _fileService.GetFileMBytesSize(fileInfo);

            // Assert
            Assert.Equal(1, result);

            File.Delete(tempFilePath);
        }

        private void CreateFakeAnySizeFile(string filePath, long sizeInBytes)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write);
            fileStream.SetLength(sizeInBytes);
        }
    }
}