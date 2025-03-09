using System.Diagnostics.CodeAnalysis;

namespace ValueTypeLibrary.Tests.TestValueTypes;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")] // Reflection works for public properties.
public class PersonName_WithHandCodedMethods : ValueType<PersonName>
{
    public string? FirstName { get; }
    public string? LastName { get; }
    
    public PersonName_WithHandCodedMethods(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public override bool Equals(object? otherObj)
    {
        return otherObj is PersonName_WithHandCodedMethods other && Equals(other);
    }

    public bool Equals(PersonName_WithHandCodedMethods? other)
    {
        return other != null &&
               ((FirstName == null && other.FirstName == null) || (FirstName != null && FirstName.Equals(other.FirstName))) &&
               ((LastName == null && other.LastName == null) || (LastName != null && LastName.Equals(other.LastName)));
    }

    public override int GetHashCode()
    {
        var hashCode = 397 * 1019;
        
        unchecked
        {
            hashCode += FirstName?.GetHashCode() ?? 0;
            hashCode *= 1019;
            hashCode += LastName?.GetHashCode() ?? 0;
            hashCode *= 1019;
        }
        
        return hashCode;
    }
}