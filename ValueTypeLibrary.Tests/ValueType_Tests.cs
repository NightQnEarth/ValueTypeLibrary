using System;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using ValueTypeLibrary.Tests.TestValueTypes;

namespace ValueTypeLibrary.Tests;

[TestFixture]
public class ValueType_Tests
{
    [Test]
	public void AddressesWithNullsAreEqual()
	{
		var address = new Address(null, null);
		var otherAddress = new Address(null, null);
        
        Assert.That(address.Equals(otherAddress), Is.True);
	}

	[Test]
	public void AddressNotEqualToNull()
	{
		var address = new Address("Turgenev Street", "4");
        
        Assert.That(address.Equals(null), Is.False);
	}

	[Test]
	public void AddressNotEqualToPersonName()
	{
		var address = new Address("A", "B");
		var person = new PersonName("A", "B");
        
        Assert.That(address.Equals(person), Is.False);
	}

	[Test]
	public void CompareAddressesWithoutSomeValues()
	{
		var address = new Address("A", null);
		var other = new Address(null, "Y");
        
        Assert.That(address.Equals(other), Is.False);
	}

	[Test]
	public void ComplexTypesAreEqual()
	{
		var person1 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
		var person2 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
		
        Assert.That(person1.Equals(person2), Is.True);
	}

	[Test]
	public void DifferentAddressesAreNotEqual()
	{
		var address = new Address("A", "B");
		var other = new Address("X", "Y");
        
        Assert.That(address.Equals(other), Is.False);
	}

	[Test]
	public void HasTypedEqualsMethod()
	{
        const string equalsMethodName = "Equals";
		MethodInfo? methodInfo = typeof(PersonName).GetMethod(equalsMethodName, new[] { typeof(PersonName) });
		const string errorMessage = $"{nameof(PersonName)} should contain method public bool {equalsMethodName}({nameof(PersonName)} name)";
		
        Assert.That(methodInfo, Is.Not.Null, errorMessage);
        Assert.That(methodInfo.IsPublic, Is.True, errorMessage);
        Assert.That(methodInfo.ReturnType == typeof(bool), Is.True, errorMessage);
        Assert.That(methodInfo.GetParameters()[0].ParameterType == typeof(PersonName), Is.True, errorMessage);
	}

	[Test]
	public void NotEqualComplexProperty()
	{
		var person1 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
		var person2 = new Person(new PersonName("A", "XXX"), 180, new DateTime(1988, 2, 29));
		
        Assert.That(person1.Equals(person2), Is.False);
	}

	[Test]
	public void NotEqualDateProperties()
	{
		var person1 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
		var person2 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 28));
		
        Assert.That(person1.Equals(person2), Is.False);
	}

	[Test]
	public void NotEqualIntProperties()
	{
		var person1 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
		var person2 = new Person(new PersonName("A", "B"), 181, new DateTime(1988, 2, 29));
		
        Assert.That(person1.Equals(person2), Is.False);
	}

	[Test]
	public void SameAddressesAreEqual()
	{
		var address1 = new Address("ул. Тургенева", "4");
		var address2 = new Address("ул. Тургенева", "4");
        
        Assert.That(address1.Equals((object) address2), Is.True);
	}
}