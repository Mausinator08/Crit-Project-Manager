using Crit.Application.Contracts.OrderFields;

namespace Crit.Application.Contracts.Pagination;

public interface IPaginationStrategyRequest<TOrderBy>
  where TOrderBy : struct, Enum
{
  public PaginationStrategy? StrategyType { get; }
  public int? Skip { get; }
  public int? Take { get; }
  public TOrderBy OrderBy { get; }
  public OrderDirection OrderDirection { get; }

  IPaginationStrategy<TOrderBy> ToStrategyRequest();
}
