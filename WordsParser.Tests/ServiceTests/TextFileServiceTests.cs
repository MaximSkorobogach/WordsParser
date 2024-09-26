using Microsoft.Extensions.Options;
using Moq;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.Services;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Tests.ServiceTests
{
    public class TextFileServiceTests
    {
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IOptions<WordsParserSettings>> _optionsMock;
        private readonly TextFileService _textFileService;

        public TextFileServiceTests()
        {
            _fileServiceMock = new Mock<IFileService>();
            _optionsMock = new Mock<IOptions<WordsParserSettings>>();

            _optionsMock.Setup(o => o.Value).Returns(new WordsParserSettings
            {
                RegexWordPattern = @"\b\w+\b",
                MinFrequency = 1
            });

            _textFileService = new TextFileService(_optionsMock.Object, _fileServiceMock.Object);
        }

        [Fact]
        public void GetWords_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            var filePath = "nonexistent.txt";

            _fileServiceMock.Setup(fs => fs.ThrowIfFileNotExists(filePath)).Throws<FileNotFoundException>();

            // Assert
            Assert.Throws<FileNotFoundException>(() => _textFileService.GetWords(filePath));
        }

        [Fact]
        public void GetWords_ShouldReturnCorrectWordList_WhenFileExists()
        {
            // Arrange
            var tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, "word word word test Test tESt");

            // Act
            var words = _textFileService.GetWords(tempFilePath);

            // Assert
            Assert.Equal(2, words.Count);
            Assert.Contains(words, w => w is { WordName: "word", Count: 3 });
            Assert.Contains(words, w => w is { WordName: "test", Count: 3 });

            File.Delete(tempFilePath);
        }
    }
}
