using Microsoft.Extensions.DependencyInjection;

namespace Crit.Application.Mappers;

public class MapperService : IMapperService
{
	private readonly IServiceProvider _serviceProvider;

	public MapperService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public TTo ConvertTo<TFrom, TTo>(TFrom fromModel)
		where TFrom : notnull
		where TTo : new()
	{
		if (fromModel is null)
			throw new ArgumentNullException(nameof(fromModel));

		IMapper<TFrom, TTo>? mapper = _serviceProvider.GetRequiredService<IMapper<TFrom, TTo>>();
		return mapper.ConvertTo(fromModel);
	}

	public List<TTo> ConvertListTo<TFrom, TTo>(List<TFrom> fromList)
		where TFrom : notnull
		where TTo : new()
	{
		if (fromList is null)
			throw new ArgumentNullException(nameof(fromList));

		IMapper<TFrom, TTo>? mapper = _serviceProvider.GetRequiredService<IMapper<TFrom, TTo>>();
		return fromList.Select(mapper.ConvertTo).ToList();
	}
}
