using FluentValidation.Results;

namespace Crit.Application.Contracts.Results;

public static class QueryResult
{
	public sealed record Success<T>(T Value) : IQueryResult;
	public sealed record Failed : IQueryResult;
	public sealed record NotFound : IQueryResult;
	public sealed record NotAllowed : IQueryResult;
	public sealed record Error(string ErrorMessage) : IQueryResult;
	public sealed record ValidationFailed(Dictionary<string, string[]> ValidationErrors) : IQueryResult
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
