using System;

namespace SheetToObjects.Lib
{
    public interface IParseValues
    {
        object Parse(Type propertyType, object value);
    }
}