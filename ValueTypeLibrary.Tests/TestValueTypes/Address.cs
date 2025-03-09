namespace ValueTypeLibrary.Tests.TestValueTypes;

public class Address : ValueType<Address>
{
    public string Street { get; init; }
    public string Building { get; init; }
    
    public Address(string street, string building)
    {
        Street = street;
        Building = building;
    }
}