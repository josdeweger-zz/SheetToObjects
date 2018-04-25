using System;

namespace SheetToObjects.Lib.Configuration
{
    public class ColumnConfig
    {
        public Type ValueType { get; }
        public string ColumnLetter { get; }
        public bool Required { get; }

        public ColumnConfig(Type valueType, string columnLetter, bool required)
        {
            ValueType = valueType;
            ColumnLetter = columnLetter;
            Required = required;
        }
    }
}