using System;

namespace SheetToObjects.Lib.AttributesConfiguration.MappingType
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
