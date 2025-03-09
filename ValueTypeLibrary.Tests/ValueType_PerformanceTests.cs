using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using ValueTypeLibrary.Tests.TestValueTypes;

namespace ValueTypeLibrary.Tests;

[TestFixture]
public class ValueType_PerformanceTests
{
    [Test]
    public void EqualsPerformance()
    {
        var count = 50_000_000;
        new PersonName("", "").Equals(new PersonName("", ""));
        new PersonName_WithHandcodedMethods("", "").Equals(new PersonName_WithHandcodedMethods("", ""));
        var people1 = Enumerable.Range(1, count).Select(i => new PersonName(new string('f', i % 10), new string('s', i % 10))).ToList();
        var people2 = Enumerable.Range(1, count).Select(i => new PersonName_WithHandcodedMethods(new string('f', i % 10), new string('s', i % 10))).ToList();
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < people1.Count; i++)
        {
            people1[i].Equals(people1[people1.Count - 1 - i]);
        }

        Console.WriteLine("ValueType<T> GetHashCode: " + sw.Elapsed.TotalSeconds);
        sw.Restart();
        for (var i = 0; i < people2.Count; i++)
        {
            people2[i].Equals(people2[people2.Count - 1 - i]);
        }
        Console.WriteLine("Hand coded GetHashCode:   " + sw.Elapsed.TotalSeconds);
    }
    
    [Test]
    public void GetHashCodePerformance()
    {
        var count = 50_000_000;
        new PersonName("", "").GetHashCode();
        new PersonName_WithHandcodedMethods("", "").GetHashCode();
        var people1 = Enumerable.Range(1, count).Select(i => new PersonName(new string('f', i % 10), new string('s', i % 10))).ToList();
        var people2 = Enumerable.Range(1, count).Select(i => new PersonName_WithHandcodedMethods(new string('f', i % 10), new string('s', i % 10))).ToList();
        var sw = Stopwatch.StartNew();
        foreach (var person in people1)
            person.GetHashCode();
        Console.WriteLine("ValueType<T> GetHashCode: " + sw.Elapsed.TotalSeconds);
        sw.Restart();
        foreach (var person in people2)
            person.GetHashCode();
        Console.WriteLine("Hand coded GetHashCode:   " + sw.Elapsed.TotalSeconds);
    }
}