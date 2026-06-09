namespace Crit.Application.Contracts.Pagination;

public abstract class PaginationResult<T>
{
  public required PaginationStrategy StrategyType { get; init; }
  public required IEnumerable<T> Items { get; init; }
}
