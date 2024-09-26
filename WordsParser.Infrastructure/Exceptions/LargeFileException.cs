namespace WordsParser.Infrastructure.Exceptions;

public class LargeFileException(string message) : Exception(message);