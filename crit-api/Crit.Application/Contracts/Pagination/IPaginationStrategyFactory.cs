namespace Crit.Application.Contracts.Pagination;

public interface IPaginationStrategyFactory
{
  IPaginationStrategy<TOrderBy> CreateStrategy<TOrderBy>(IPaginationStrategyRequest<TOrderBy> request)
      where TOrderBy : struct, Enum;
}
