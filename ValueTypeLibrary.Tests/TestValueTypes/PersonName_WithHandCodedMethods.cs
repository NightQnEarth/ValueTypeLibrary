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
        unchecked
        {
            var hashCode = 17;
            
            hashCode = hashCode * 31 + FirstName?.GetHashCode() ?? 0;
            hashCode = hashCode * 31 + LastName?.GetHashCode() ?? 0;
            
            return hashCode;
        }
    }
}