using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ValueTypeLibrary;

/// <summary>
///     The base class for all Value-types, that is, for classes whose instances we consider equal if their corresponding public properties are equal.<br/>
///     Thus, ValueType&lt;T> overrides Equals and GetHashCode methods for all inheritors of type T to ensure
///     correct comparision of instances based on their corresponding public properties.<br/>
///     This allows for an efficient overriding of the comparision mechanism to value-type paradigm, avoiding redundant, repetitive overriding in each individual class.
/// </summary>
public abstract class ValueType<T> where T : ValueType<T>
{
    private static readonly PropertyInfo[] propertiesInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    
    private static readonly Func<T, T, bool> tObjectsComparer = CreateEqualsFunc();
    private static readonly Func<T, int> tObjectHashCodeGenerator = CreateHashCodeFunc();
    
    public override bool Equals(object? otherObj)
    {
        return otherObj is T otherT && Equals(otherT);
    }

    public bool Equals(T? otherT)
    {
        return otherT != null && tObjectsComparer((T)this, otherT);
    }

    public override int GetHashCode()
    {
        return tObjectHashCodeGenerator((T)this);
    }

    private static Func<T, T, bool> CreateEqualsFunc()
    {
        // Define two T-type parameters to represent compared objects.
        var tParam1 = Expression.Parameter(typeof(T), "tObj1");
        var tParam2 = Expression.Parameter(typeof(T), "tObj2");
        var equalCallExpressions = new List<Expression>(propertiesInfo.Length);

        // For each property, we call the Equals method, which compares values of properties in two objects.
        foreach (var property in propertiesInfo)
        {
            var propertyAccess1 = Expression.Property(tParam1, property);
            var propertyAccess2 = Expression.Property(tParam2, property);

            // Choose a method with the correct signature to prevent boxing, if possible.
            var equalsMethodInfo = property.PropertyType.GetMethod("Equals", [property.PropertyType]) ??
                                   property.PropertyType.GetMethod("Equals", [typeof(object)]) ??
                                   throw new UnreachableException("This error is unreachable, because in any type defined the Object.Equals method.");
            var equalsCallExpression = Expression.Call(propertyAccess1, equalsMethodInfo, propertyAccess2);

            // For value-types, further null-checks are not appropriate and will cause execution-errors.
            if (property.PropertyType.IsValueType)
            {
                // Add "p1.Equals(p2)" expression for value-type properties.
                equalCallExpressions.Add(equalsCallExpression);
                continue;
            }

            // Check that "p1.Equals(p2)" expression will not fall with the NullReferenceException.
            var isFirstPropertyNotNull = Expression.NotEqual(propertyAccess1, Expression.Constant(null, property.PropertyType));
            var safeEqualExpression = Expression.AndAlso(isFirstPropertyNotNull, equalsCallExpression);

            // Check whether both properties are null.
            var isFirstPropertyNull = Expression.Equal(propertyAccess1, Expression.Constant(null, property.PropertyType));
            var isSecondPropertyNull = Expression.Equal(propertyAccess2, Expression.Constant(null, property.PropertyType));
            var areBothPropertiesNull = Expression.AndAlso(isFirstPropertyNull, isSecondPropertyNull);

            // Built final expression for reference-type properties: (p1 == null && p2 == null) || (p1 != null && p1.Equals(p2)).
            var finalEqualExpression = Expression.OrElse(areBothPropertiesNull, safeEqualExpression);

            equalCallExpressions.Add(finalEqualExpression);
        }

        // Aggregate all comparison expressions through &&, since all properties must be equal.
        Expression body = equalCallExpressions.Aggregate(Expression.AndAlso);

        // Compile the result expression into a delegate and save it for reuse.
        var lambda = Expression.Lambda<Func<T, T, bool>>(body, tParam1, tParam2);

        return lambda.Compile();
    }

    private static Func<T, int> CreateHashCodeFunc()
    {
        // Define one T-type parameter for an object whose hash code we are calculating.
        var tParam = Expression.Parameter(typeof(T), "tObj");
        var hashCodeExpressions = new List<Expression>(propertiesInfo.Length);
        
        // For each property, we call the GetHashCode method.
        foreach (var property in propertiesInfo)
        {
            var propertyAccess = Expression.Property(tParam, property);
            var getHashCodeCall = Expression.Call(propertyAccess, "GetHashCode", null);
            hashCodeExpressions.Add(getHashCodeCall);
        }

        var initialHash = Expression.Constant(397);
        Expression aggregateExpr = initialHash;

        // Aggregate all hash-codes using the initial value of 397 and the formula unchecked(aggregate * 1019 + next).
        foreach (var expr in hashCodeExpressions)
        {
            var multiply = Expression.Multiply(aggregateExpr, Expression.Constant(1019));
            var sum = Expression.Add(multiply, expr);
            aggregateExpr = Expression.Convert(sum, typeof(int));
        }

        // Compile expression into a delegate.
        var lambda = Expression.Lambda<Func<T, int>>(aggregateExpr, tParam);
        return lambda.Compile();
    }
}