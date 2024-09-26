using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Exceptions;
using WordsParser.Infrastructure.Services.Interfaces;
using WordsParser.Infrastructure.Strategies;
using WordsParser.Infrastructure.Strategies.Interfaces;
using Xunit;

namespace WordsParser.Tests.StrategyTests
{
    public class FileProcessingStrategyTests
    {
        private readonly Mock<IOptions<FileSettings>> _fileSettingsMock;
        private readonly Mock<ITextFileService> _textFileServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IWordsService> _wordsServiceMock;
        private readonly Mock<ILogger<IFileProcessingStrategy>> _loggerMock;
        private readonly FileProcessingStrategy _fileProcessingStrategy;

        public FileProcessingStrategyTests()
        {
            _fileSettingsMock = new Mock<IOptions<FileSettings>>();
            _textFileServiceMock = new Mock<ITextFileService>();
            _fileServiceMock = new Mock<IFileService>();
            _wordsServiceMock = new Mock<IWordsService>();
            _loggerMock = new Mock<ILogger<IFileProcessingStrategy>>();

            _fileSettingsMock.Setup(x => x.Value).Returns(new FileSettings { MaxFileSizeMbytes = 1000 }); // 1000 MB

            _fileProcessingStrategy = new FileProcessingStrategy(
                _fileSettingsMock.Object,
                _textFileServiceMock.Object,
                _fileServiceMock.Object,
                _wordsServiceMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowLargeFileException_WhenFileSizeExceedsLimit()
        {
            // Arrange
            var filePath = "largeFile.txt";

            var largeFileSize = 1001; // 1 gb 1 mbyte

            _fileServiceMock.Setup(fs => fs.GetFileMBytesSize(It.IsAny<FileInfo>())).Returns(largeFileSize);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<StrategyException>(() => _fileProcessingStrategy.ExecuteAsync(filePath));
            Assert.Equal($"Не удалось обработать стратегию {nameof(FileProcessingStrategy)}, message : Файл превышает лимит в 1000 МБ.", exception.Message);
        }
    }
}
