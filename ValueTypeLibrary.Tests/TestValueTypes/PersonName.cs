namespace ValueTypeLibrary.Tests.TestValueTypes;

public class PersonName : ValueType<PersonName>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public PersonName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}