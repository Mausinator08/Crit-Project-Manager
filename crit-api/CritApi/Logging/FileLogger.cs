using System.Text;

namespace CritApi.Logging;

public class FileLogger : IFileLogger
{
    private readonly int maxExceptionDepth = 5;

    public FileLogger(string? path)
    {
        if (path != null)
        {
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, path));
            filePath = Path.Combine(path, "crit-api.log");
        }
        else
        {
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Logs"));
            filePath = Path.Combine(Environment.CurrentDirectory, "Logs", "CritApi.log");
        }
    }

    private FileLogger()
    {
        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Logs"));
        filePath = Path.Combine(Environment.CurrentDirectory, "Logs", "CritApi.log");
    }

    public string FilePath
    {
        get
        {
            return filePath;
        }
    }

    private readonly string filePath;

    public void LogError(string formattedMessage, params object?[] args)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        stringBuilder.AppendLine($"--- ERROR: {dateTime} ---");
        stringBuilder.AppendLine(string.Format(formattedMessage, args));
        stringBuilder.AppendLine("--- ---");

        File.AppendAllText(filePath, stringBuilder.ToString());
    }

    public void LogException(Exception ex)
    {
        Exception exception = ex;
        StringBuilder stringBuilder = new StringBuilder();
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        stringBuilder.AppendLine($"--- EXCEPTION: {dateTime} ---");
        stringBuilder.AppendLine($"MESSAGE: {exception.Message}");
        stringBuilder.AppendLine($"SOURCE: {exception.Source}");
        stringBuilder.AppendLine($"STACK TRACE: {exception.StackTrace}");

        int indent = 0;
        while (exception.InnerException != null && indent < maxExceptionDepth)
        {
            exception = exception.InnerException;
            indent++;
            stringBuilder.AppendLine($"--- INNER EXCEPTION: {dateTime} ---".PadLeft(indent, '\t'));
            stringBuilder.AppendLine($"MESSAGE: {exception.Message}".PadLeft(indent, '\t'));
            stringBuilder.AppendLine($"SOURCE: {exception.Source}".PadLeft(indent, '\t'));
            stringBuilder.AppendLine($"STACK TRACE: {exception.StackTrace}".PadLeft(indent, '\t'));
        }

        stringBuilder.AppendLine("--- ---");

        File.AppendAllText(filePath, stringBuilder.ToString());
    }

    public void LogInfo(string formattedMessage, params object?[] args)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        stringBuilder.AppendLine($"--- INFO: {dateTime} ---");
        stringBuilder.AppendLine(string.Format(formattedMessage, args));
        stringBuilder.AppendLine("--- ---");

        File.AppendAllText(filePath, stringBuilder.ToString());
    }

    public void LogWarning(string formattedMessage, params object?[] args)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        stringBuilder.AppendLine($"--- WARNING: {dateTime} ---");
        stringBuilder.AppendLine(string.Format(formattedMessage, args));
        stringBuilder.AppendLine("--- ---");

        File.AppendAllText(filePath, stringBuilder.ToString());
    }
}
