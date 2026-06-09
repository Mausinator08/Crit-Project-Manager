using System.Text.Json.Serialization;

namespace Crit.Application.Contracts.Pagination;

public enum PaginationStrategy
{
  None,
  SkipTake
}
