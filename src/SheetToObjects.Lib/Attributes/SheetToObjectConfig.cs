using System;


namespace SheetToObjects.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SheetToObjectConfig : Attribute
    {
        public bool SheetHasHeaders { get; }

        public bool AutoMapProperties { get; }

        public SheetToObjectConfig(bool sheetHasHeaders = true, bool autoMapProperties = true)
        {
            SheetHasHeaders = sheetHasHeaders;
            AutoMapProperties = autoMapProperties;
        }
    }
}
