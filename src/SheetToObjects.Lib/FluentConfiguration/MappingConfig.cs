using System;
using System.Collections.Generic;
using System.Linq;

namespace SheetToObjects.Lib.FluentConfiguration
{
    public class MappingConfig
    {
        public bool AutoMapProperties { get; set; }

        public bool HasHeaders { get; set; }

        public MappingConfig(bool hasHeaders = true, bool autoMapProperties = true)
        {
            HasHeaders = hasHeaders;
            AutoMapProperties = autoMapProperties;
        }

        public List<ColumnMapping> ColumnMappings = new List<ColumnMapping>();
        
        public ColumnMapping GetColumnMappingByPropertyName(string propertyName)
        {
            return ColumnMappings.FirstOrDefault(m =>
                m.PropertyName.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}