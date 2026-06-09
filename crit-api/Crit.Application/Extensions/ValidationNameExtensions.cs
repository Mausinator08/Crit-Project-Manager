using FluentValidation;

namespace Crit.Application.Extensions;

public static class ValidationNameExtensions
{
	public static IRuleBuilderOptions<T, TProperty> WithCamelCasePropertyName<T, TProperty>(
		this IRuleBuilderOptions<T, TProperty> rule,
		string propertyName)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

		var camel = propertyName.Length == 1
		? char.ToLowerInvariant(propertyName[0]).ToString()
		: char.ToLowerInvariant(propertyName[0]) + propertyName[1..];

		return rule.OverridePropertyName(camel);
	}
}
