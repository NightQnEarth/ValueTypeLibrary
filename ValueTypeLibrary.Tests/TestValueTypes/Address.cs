namespace ValueTypeLibrary.Tests.TestValueTypes;

public class Address : ValueType<Address>
{
    public string Street { get; }
    public string Building { get; }
    
    public Address(string street, string building)
    {
        Street = street;
        Building = building;
    }
}