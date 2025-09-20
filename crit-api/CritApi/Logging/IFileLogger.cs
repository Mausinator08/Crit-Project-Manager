namespace CritApi.Logging;

public interface IFileLogger
{
    string FilePath { get; }
    void LogInfo(string formattedMessage, params object?[] args);
    void LogWarning(string formattedMessage, params object?[] args);
    void LogError(string formattedMessage, params object?[] args);
    void LogException(Exception ex);
}
