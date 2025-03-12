using System;
using System.Reflection;
using NUnit.Framework;
using ValueTypeLibrary.Tests.TestValueTypes;

namespace ValueTypeLibrary.Tests;

[TestFixture]
public class ValueType_Tests
{
    [Test]
    public void AddressesWithNulls_AreEqual()
    {
        var address1 = new Address(null, null);
        var address2 = new Address(null, null);

        Assert.That(address1.GetHashCode(), Is.EqualTo(address2.GetHashCode()));
        Assert.That(address1.Equals(address2), Is.True);
    }

    [Test]
    public void CorrectAddress_NotEqualToNull()
    {
        var address = new Address("Turgenev Street", "4");

        Assert.That(address.Equals(null), Is.False);
    }

    [Test]
    public void DifferentTypesWithSameValues_AreNotEqual()
    {
        var address = new Address("A", "B");
        var person = new PersonName("A", "B");

        Assert.That(address.GetHashCode(), Is.EqualTo(person.GetHashCode()));
        // ReSharper disable once SuspiciousTypeConversion.Global (for testing purposes)
        Assert.That(address.Equals(person), Is.False);
    }

    [Test]
    public void DifferentAddressesWithSomeNullProperties_AreNotEqual()
    {
        var address1 = new Address("A", null);
        var address2 = new Address(null, "Y");

        Assert.That(address1.GetHashCode(), Is.Not.EqualTo(address2.GetHashCode()));
        Assert.That(address1.Equals(address2), Is.False);
    }

    [Test]
    public void DifferentAddresses_AreNotEqual()
    {
        var address1 = new Address("A", "B");
        var address2 = new Address("X", "Y");

        Assert.That(address1.GetHashCode(), Is.Not.EqualTo(address2.GetHashCode()));
        Assert.That(address1.Equals(address2), Is.False);
    }

    [Test]
    public void SameAddresses_AreEqual()
    {
        var address1 = new Address("Turgenev Street", "4");
        var address2 = new Address("Turgenev Street", "4");

        Assert.That(address1.GetHashCode(), Is.EqualTo(address2.GetHashCode()));
        Assert.That(address1.Equals(address2), Is.True);
    }

    [Test]
    public void EqualsMethodWithObjectParameter_OnSameAddresses_ReturnsTrue()
    {
        var address1 = new Address("Turgenev Street", "4");
        var address2 = new Address("Turgenev Street", "4");

        Assert.That(address1.Equals(address2 as object), Is.True);
    }

    [Test]
    public void ValueTypeHasTypedEqualsMethod()
    {
        MethodInfo? methodInfo = typeof(PersonName).GetMethod("Equals", [typeof(PersonName)]);
        const string errorMessage = $"{nameof(PersonName)} : {nameof(ValueType<PersonName>)
        } should contains a typed Equals method to prevent undesirable boxing: public bool Equals({nameof(PersonName)} personName)";

        Assert.That(methodInfo, Is.Not.Null, errorMessage);
        Assert.That(methodInfo!.IsPublic, Is.True, errorMessage);
        Assert.That(methodInfo.ReturnType == typeof(bool), Is.True, errorMessage);
        Assert.That(methodInfo.GetParameters()[0].ParameterType == typeof(PersonName), Is.True, errorMessage);
    }

    [Test]
    public void EqualComplexObjects_AreEqual()
    {
        var person1 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
        var person2 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));

        Assert.That(person1.GetHashCode(), Is.EqualTo(person2.GetHashCode()));
        Assert.That(person1.Equals(person2), Is.True);
    }

    [Test]
    public void ObjectsWithDifferentValueTypeProperties_AreNotEqual()
    {
        var person1 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
        var person2 = new Person(new PersonName("A", "XXX"), 180, new DateTime(1988, 2, 29));

        Assert.That(person1.GetHashCode(), Is.Not.EqualTo(person2.GetHashCode()));
        Assert.That(person1.Equals(person2), Is.False);
    }

    [Test]
    public void ObjectsWithDifferentDateTimeProperties_AreNotEqual()
    {
        var person1 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
        var person2 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 28));

        Assert.That(person1.GetHashCode(), Is.Not.EqualTo(person2.GetHashCode()));
        Assert.That(person1.Equals(person2), Is.False);
    }

    [Test]
    public void ObjectsWithDifferentIntProperties_AreNotEqual()
    {
        var person1 = new Person(new PersonName("A", "B"), 180, new DateTime(1988, 2, 29));
        var person2 = new Person(new PersonName("A", "B"), 181, new DateTime(1988, 2, 29));

        Assert.That(person1.GetHashCode(), Is.Not.EqualTo(person2.GetHashCode()));
        Assert.That(person1.Equals(person2), Is.False);
    }
}