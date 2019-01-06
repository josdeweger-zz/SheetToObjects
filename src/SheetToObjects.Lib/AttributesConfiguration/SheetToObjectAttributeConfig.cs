using System;

namespace SheetToObjects.Lib.AttributesConfiguration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SheetToObjectAttributeConfig : Attribute
    {
        public bool SheetHasHeaders { get; }

        public bool AutoMapProperties { get; }

        public SheetToObjectAttributeConfig(bool sheetHasHeaders = true, bool autoMapProperties = true)
        {
            SheetHasHeaders = sheetHasHeaders;
            AutoMapProperties = autoMapProperties;
        }
    }
}
