using System;

namespace SheetToObjects.Lib
{
    public interface IColumnMapping
    {
        string ColumnLetter { get; }
        bool Required { get; }
        string PropertyName { get; }
        Type PropertyType { get; }
    }
}