using System.Linq.Expressions;
using Crit.Application.Contracts.OrderFields;
using Microsoft.EntityFrameworkCore;

namespace Crit.Application.Contracts.Pagination.Strategies;

public sealed class SkipTakePaginationStrategy<TOrderBy>(int skip, int take, TOrderBy orderBy = default, OrderDirection orderDirection = default)
  : IPaginationStrategy<TOrderBy>
  where TOrderBy : struct, Enum
{
  public const int MaxPageSize = 5_000;

  private readonly int _skip = Math.Max(skip, 0);
  private readonly int _take = Math.Max(Math.Min(take, MaxPageSize), 0);
  private readonly OrderDirection _orderDirection = orderDirection;
  private readonly TOrderBy _orderBy = orderBy;

  public async Task<PaginationResult<TReturnDto>> ApplyAsync<TEntity, TReturnDto>(
      IQueryable<TEntity> source,
      Expression<Func<TEntity, TReturnDto>> projection,
      Func<TOrderBy, Expression<Func<TEntity, object>>> orderBySelector,
      CancellationToken cancellationToken)
        where TEntity : class
  {
    var selector = orderBySelector(_orderBy);
    var orderedQuery = _orderDirection == OrderDirection.Desc
        ? source.OrderByDescending(selector)
        : source.OrderBy(selector);

    var items = await orderedQuery
      .Skip(_skip)
      .Take(_take)
      .Select(projection)
      .ToListAsync(cancellationToken);

    var totalCount = orderedQuery.Count();
    return new SkipTakePaginationResult<TReturnDto>
    {
      Items = items,
      Count = items.Count,
      TotalCount = totalCount,
      TotalPages = (long)Math.Ceiling((decimal)totalCount / _take),
      CurrentPage = (long)Math.Ceiling((decimal)(_skip / _take) + 1),
      StrategyType = PaginationStrategy.SkipTake
    };
  }
}
