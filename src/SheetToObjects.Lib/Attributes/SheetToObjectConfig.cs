using System;


namespace SheetToObjects.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SheetToObjectConfig : Attribute
    {
        public bool SheetHasHeaders { get; }

        public SheetToObjectConfig(bool sheetHasHeaders)
        {
            SheetHasHeaders = sheetHasHeaders;
        }
    }
}
