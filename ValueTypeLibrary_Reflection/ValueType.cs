using System.Linq;
using System.Reflection;

namespace ValueTypeLibrary_Reflection;

/// <summary>
///     Easy, simple and pretty slow reflection-based implementation of the ValueType class from the ValueTypeLibrary.
///     As an advantage, one can highlight the simplicity and readability of this approach; however, the cost of this is speed: this code
///     runs significantly slower than an Expressions-based implementation.
/// </summary>
public abstract class ValueType<T> where T : ValueType<T>
{
    private static readonly PropertyInfo[] propertiesInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public override bool Equals(object? otherObj)
    {
        return otherObj is T otherValue && Equals(otherValue);
    }

    public bool Equals(T? otherValue)
    {
        return otherValue != null && propertiesInfo.Select(propertyInfo => propertyInfo.GetValue(otherValue))
                                                   .SequenceEqual(propertiesInfo.Select(propertyInfo => propertyInfo.GetValue(this)));
    }

    public override int GetHashCode()
    {
        return propertiesInfo.Select(propertyInfo => propertyInfo.GetValue(this)?.GetHashCode() ?? 0)
                             .Aggregate(17, (accumulation, propertyHashCode) => unchecked(accumulation * 31 + propertyHashCode));
    }
}