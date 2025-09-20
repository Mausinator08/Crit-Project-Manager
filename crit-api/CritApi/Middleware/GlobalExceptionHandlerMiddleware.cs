using CritApi.Logging;

namespace CritApi.Middleware;

public class GlobalExceptionHandlerMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
	private readonly IFileLogger _fileLogger;

	public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IFileLogger fileLogger)
	{
		_next = next;
		_logger = logger;
		_fileLogger = fileLogger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unhandled exception occurred during request processing.");
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Response.ContentType = "application/json";
			_fileLogger.LogException(ex);
		}
	}
}
