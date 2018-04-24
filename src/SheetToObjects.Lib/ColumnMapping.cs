using System;

namespace SheetToObjects.Lib
{
    public class ColumnMapping
    {
        public string ColumnLetter { get; }
        public string PropertyName { get; }
        public Type PropertyType { get; }
        public bool Required { get; }

        public ColumnMapping(string columnLetter, string propertyName, Type propertyType, bool required)
        {
            ColumnLetter = columnLetter;
            PropertyName = propertyName;
            PropertyType = propertyType;
            Required = required;
        }
    }
}