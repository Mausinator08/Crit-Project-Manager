namespace Crit.Application.Contracts.Pagination;

public sealed class SkipTakePaginationResult<T>
  : PaginationResult<T>
{
  public required int Count { get; init; }
  public required long TotalCount { get; init; }
  public required long TotalPages { get; init; }
  public required long CurrentPage { get; init; }
  public bool HasNextPage => CurrentPage < TotalPages;
}
