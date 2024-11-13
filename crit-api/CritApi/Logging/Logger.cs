
using System.Text;

namespace CritApi.Logging;

public class Logger : ILogger
{
    public Logger(string? path)
    {
        if (path != null)
        {
            Directory.CreateDirectory(path);
            filePath = path;
        }
        else
        {
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Logs"));
            filePath = Path.Combine(Environment.CurrentDirectory, "Logs", "CritApi.log");
        }
    }

    private Logger()
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
        while (exception.InnerException != null)
        {
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
