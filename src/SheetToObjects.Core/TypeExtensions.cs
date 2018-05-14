using System;

namespace SheetToObjects.Core
{
    internal static class TypeExtensions
    {
        public static object GetDefault(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}