using System;

namespace SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultValue : Attribute
    {
        public object Value { get; }

        public DefaultValue(object defaultValue)
        {
            Value = defaultValue;
        }
    }
}
