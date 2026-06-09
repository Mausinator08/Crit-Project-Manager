using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Crit.Application.Contracts.Pagination.Strategies;

public sealed class NoPaginationStrategy<TOrderBy>
  : IPaginationStrategy<TOrderBy>
  where TOrderBy : struct, Enum
{
  public const int DefaultMaxResults = 100_000;

  public async Task<PaginationResult<TReturnDto>> ApplyAsync<TEntity, TReturnDto>(
    IQueryable<TEntity> source,
    Expression<Func<TEntity, TReturnDto>> projection,
    Func<TOrderBy, Expression<Func<TEntity, object>>> orderBySelector,
    CancellationToken cancellationToken)
      where TEntity : class
  {
    // Ignores orderby and getKeySelector since we're not doing any pagination, but we still need to accept them to satisfy the interface contract.

    // Apply safety limit
    var items = await source.Select(projection).ToListAsync(cancellationToken);
    if (items.Count > DefaultMaxResults)
    {
      throw new InvalidOperationException($"Query returned more than {DefaultMaxResults} results. Please use pagination instead of loading all results.");
    }

    return new NoPaginationResult<TReturnDto>
    {
      Items = items,
      StrategyType = PaginationStrategy.None
    };
  }
}
