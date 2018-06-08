using System;

namespace SheetToObjects.Core
{
    internal static class TypeExtensions
    {
        public static object GetDefault(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static bool IsNotNullable(this Type type)
        {
            return !IsNullable(type);
        }
    }
}