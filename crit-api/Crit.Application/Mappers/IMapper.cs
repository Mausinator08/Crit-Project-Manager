namespace Crit.Application.Mappers;

public interface IMapper<TFrom, TTo>
	where TFrom : notnull
	where TTo : new()
{
	TTo ConvertTo(TFrom fromModel);
}
