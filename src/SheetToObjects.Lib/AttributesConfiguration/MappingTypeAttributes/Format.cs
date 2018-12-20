using System;

namespace SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Format : Attribute
    {
        public string FormatString { get; }

        public Format(string formatString)
        {
            FormatString = formatString;
        }
    }
}
