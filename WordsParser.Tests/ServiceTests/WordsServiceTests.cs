using Microsoft.Extensions.Logging;
using Moq;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Repositories.Interfaces;
using WordsParser.Infrastructure.Services;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Tests.ServiceTests
{
    public class WordsServiceTests
    {
        private readonly Mock<IRepository<Word>> _repositoryMock;
        private readonly Mock<ILogger<IWordsService>> _loggerMock;
        private readonly WordsService _wordsService;

        public WordsServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Word>>();
            _loggerMock = new Mock<ILogger<IWordsService>>();
            _wordsService = new WordsService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task SaveWordsCountAsync_ShouldCallAddOrUpdateRangeAsyncOnce()
        {
            // Arrange
            var words = new List<Word>
            {
                new("test1", 1),
                new("test2", 2)
            };

            // Act
            await _wordsService.SaveWordsCountAsync(words);

            // Assert
            _repositoryMock.Verify(repo => repo.AddOrUpdateRangeAsync(words), Times.Once);
        }
    }
}
