using WordsParser.Infrastructure.Configurations.Interfaces;

namespace WordsParser.Infrastructure.Configurations;

public class FileSettings : IFileSettings
{
    public int MaxFileSizeMbytes { get; set; }
}