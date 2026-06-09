using System.Linq.Expressions;

namespace Crit.Application.Contracts.Pagination;

public interface IPaginationStrategy<TOrderBy> where TOrderBy : struct, Enum
{
  Task<PaginationResult<TReturnDto>> ApplyAsync<TEntity, TReturnDto>(
    IQueryable<TEntity> source,
    Expression<Func<TEntity, TReturnDto>> projection,
    Func<TOrderBy, Expression<Func<TEntity, object>>> orderBySelector,
    CancellationToken cancellationToken)
      where TEntity : class;
}

