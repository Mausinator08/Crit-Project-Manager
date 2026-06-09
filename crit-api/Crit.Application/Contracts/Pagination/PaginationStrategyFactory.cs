using Crit.Application.Contracts.Pagination.Strategies;

namespace Crit.Application.Contracts.Pagination;

public sealed class PaginationStrategyFactory
  : IPaginationStrategyFactory
{
  public IPaginationStrategy<TOrderBy> CreateStrategy<TOrderBy>(IPaginationStrategyRequest<TOrderBy> request)
  where TOrderBy : struct, Enum =>
    request.StrategyType switch
    {
      PaginationStrategy.SkipTake => new SkipTakePaginationStrategy<TOrderBy>(
          skip: Math.Max(0, request.Skip ?? 0),
          take: Math.Max(1, request.Take ?? 100)),

      PaginationStrategy.None => new NoPaginationStrategy<TOrderBy>(),

      _ => (request.Skip is not null || request.Take is not null)
        ? new SkipTakePaginationStrategy<TOrderBy>(
          skip: Math.Max(0, request.Skip ?? 0),
          take: Math.Max(1, request.Take ?? 100))
        : new NoPaginationStrategy<TOrderBy>()
    };
}

