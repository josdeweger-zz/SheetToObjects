using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib
{
    public interface IParseValues
    {
        Result<T> Parse<T>(object value, IFormatProvider formatProvider = null);
        Result<object> Parse(object value, Type type);
    }
}