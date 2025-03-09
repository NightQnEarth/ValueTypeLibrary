namespace ValueTypeLibrary.Tests.TestValueTypes;

public class PersonName : ValueType<PersonName>
{
    public string FirstName { get; }
    public string LastName { get; }
    
    public PersonName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}