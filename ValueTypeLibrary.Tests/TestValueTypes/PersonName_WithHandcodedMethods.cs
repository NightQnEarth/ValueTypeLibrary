namespace ValueTypeLibrary.Tests.TestValueTypes;

public class PersonName_WithHandcodedMethods : ValueType<PersonName>
{
    public string FirstName { get; }
    public string LastName { get; }
    
    public PersonName_WithHandcodedMethods(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public override int GetHashCode()
    {
        return unchecked((FirstName == null ? 0 : FirstName.GetHashCode()) * 16777619 + (LastName == null ? 0 : LastName.GetHashCode()));
    }
}