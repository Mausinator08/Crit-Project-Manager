using FluentValidation.Results;

namespace Crit.Application.Contracts.Results;

public static class CommandResult
{
	public sealed record Success : ICommandResult;
	public sealed record Success<T>(T Value) : ICommandResult;
	public sealed record Failed : ICommandResult;
	public sealed record NotFound : ICommandResult;
	public sealed record NotAllowed : ICommandResult;
	public sealed record Error(string ErrorMessage) : ICommandResult;
	public sealed record ValidationFailed(Dictionary<string, string[]> ValidationErrors) : ICommandResult
	{
		public ValidationFailed(IEnumerable<ValidationFailure> errors)
			: this(
				errors.GroupBy(x => x.PropertyName)
				.ToDictionary(
					x => x.Key,
					x => x.Select(v => v.ErrorMessage).ToArray()))
		{ }
	}
}
