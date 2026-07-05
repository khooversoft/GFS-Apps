using System.Collections.Concurrent;
using System.Reflection;
using Toolbox.SAP.sdk.Abstractions;

namespace Toolbox.SAP.sdk;

internal static class SapRowMapper
{
    private static readonly ConcurrentDictionary<Type, PropertyMapping[]> _cache = new();

    public static T Map<T>(ISapStructure row) where T : new()
    {
        var mappings = _cache.GetOrAdd(typeof(T), BuildMappings);
        var instance = new T();

        foreach (var mapping in mappings)
        {
            var sapValue = row.GetString(mapping.FieldName);
            var converted = ConvertValue(sapValue, mapping.Property.PropertyType);
            mapping.Property.SetValue(instance, converted);
        }

        return instance;
    }

    private static PropertyMapping[] BuildMappings(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new { Property = p, Attribute = p.GetCustomAttribute<SapFieldAttribute>() })
            .Where(x => x.Attribute is not null)
            .Select(x => new PropertyMapping(x.Property, x.Attribute!.FieldName))
            .ToArray();
    }

    private static object? ConvertValue(string sapValue, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (string.IsNullOrEmpty(sapValue))
        {
            if (underlying == typeof(string)) return string.Empty;
            if (targetType != underlying) return null; // nullable type
            return underlying.IsValueType ? Activator.CreateInstance(underlying) : null;
        }

        if (underlying == typeof(string)) return sapValue;
        if (underlying == typeof(int)) return int.Parse(sapValue);
        if (underlying == typeof(double)) return double.Parse(sapValue);
        if (underlying == typeof(decimal)) return decimal.Parse(sapValue);
        if (underlying == typeof(long)) return long.Parse(sapValue);
        if (underlying == typeof(DateTime)) return DateTime.Parse(sapValue);
        if (underlying == typeof(bool)) return sapValue != "0" && !string.IsNullOrWhiteSpace(sapValue);

        return Convert.ChangeType(sapValue, underlying);
    }

    private sealed record PropertyMapping(PropertyInfo Property, string FieldName);
}
