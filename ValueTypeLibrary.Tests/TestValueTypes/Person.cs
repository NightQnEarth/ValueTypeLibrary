using System;

namespace ValueTypeLibrary.Tests.TestValueTypes;

public class Person : ValueType<Person>
{
    public PersonName Name { get; init; }
    public decimal Height { get; init; }
    public DateTime BirthDate { get; init; }

    public Person(PersonName name, decimal height, DateTime birthDate)
    {
        Name = name;
        Height = height;
        BirthDate = birthDate;
    }
}