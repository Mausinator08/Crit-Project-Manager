namespace Crit.Application.Mappers;

public interface IMapperService
{
	TTo ConvertTo<TFrom, TTo>(TFrom fromModel)
	where TFrom : notnull
	where TTo : new();
	List<TTo> ConvertListTo<TFrom, TTo>(List<TFrom> fromList)
	where TFrom : notnull
	where TTo : new();
}
