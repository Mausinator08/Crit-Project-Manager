using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class ApiResponse
{
    [JsonConstructor]
    public ApiResponse(string? message, List<string>? errors, object? data = default)
    {
        Message = message;
        if (errors != null)
        {
            Errors = errors;
        }
        else
        {
            Errors = new List<string>();
        }
        Data = data;
    }

    public string? Message { get; set; }
    public List<string> Errors { get; set; }
    public object? Data { get; set; }
}
