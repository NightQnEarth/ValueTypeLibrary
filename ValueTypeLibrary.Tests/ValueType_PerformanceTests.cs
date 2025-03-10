using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using ValueTypeLibrary.Tests.TestValueTypes;

namespace ValueTypeLibrary.Tests;

[TestFixture]
public class ValueType_PerformanceTests
{
    private const int TestIterationsCount = 50_000_000;

    private static readonly ImmutableArray<PersonName> personNames;
    private static readonly ImmutableArray<PersonName_WithHandCodedMethods> handCodedPersonNames;

    // We can see very rare and interesting behaviour in case of move static fields initialization inline from static constructor.
    // This is due to the initialization order depending on "beforefieldinit" flag.
    // See Article by Jon Skeet about "C# and beforefieldinit" (https://csharpindepth.com/Articles/BeforeFieldInit).
    static ValueType_PerformanceTests()
    {
        personNames =
            Enumerable.Range(1, TestIterationsCount)
                      .Select(n => new PersonName(new string('f', n % 10), new string('s', n % 10)))
                      .ToImmutableArray();
        
        handCodedPersonNames =
            Enumerable.Range(1, TestIterationsCount)
                      .Select(n => new PersonName_WithHandCodedMethods(new string('f', n % 10), new string('s', n % 10)))
                      .ToImmutableArray();
    }
    
    [Test]
    public void EqualsPerformance()
    {
        new PersonName("", "").Equals(new PersonName("", ""));
        new PersonName_WithHandCodedMethods("", "").Equals(new PersonName_WithHandCodedMethods("", ""));
        
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < TestIterationsCount; i++)
        {
            personNames[i].Equals(personNames[TestIterationsCount - 1 - i]);
        }
        Console.WriteLine("ValueType<T> Equals: " + sw.Elapsed.TotalSeconds);
        
        sw.Restart();
        for (var i = 0; i < TestIterationsCount; i++)
        {
            handCodedPersonNames[i].Equals(handCodedPersonNames[TestIterationsCount - 1 - i]);
        }
        Console.WriteLine("Hand coded Equals:   " + sw.Elapsed.TotalSeconds);
    }
    
    [Test]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")] // Calling for testing purposes.
    public void GetHashCodePerformance()
    {
        new PersonName("", "").GetHashCode();
        new PersonName_WithHandCodedMethods("", "").GetHashCode();
        
        var sw = Stopwatch.StartNew();
        foreach (var personName in personNames)
        {
            personName.GetHashCode();
        }
        Console.WriteLine("ValueType<T> GetHashCode: " + sw.Elapsed.TotalSeconds);
        
        sw.Restart();
        foreach (var handCodedPersonName in handCodedPersonNames)
        {
            handCodedPersonName.GetHashCode();
        }
        Console.WriteLine("Hand coded GetHashCode:   " + sw.Elapsed.TotalSeconds);
    }
}