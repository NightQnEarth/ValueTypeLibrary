using System;

namespace ValueTypeLibrary.Tests.TestValueTypes;

public class Person : ValueType<Person>
{
    public PersonName Name { get; }
    public decimal Height { get; }
    public DateTime BirthDate { get; }

    public Person(PersonName name, decimal height, DateTime birthDate)
    {
        Name = name;
        Height = height;
        BirthDate = birthDate;
    }
}