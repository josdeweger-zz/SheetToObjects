using System.Collections.Generic;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Attributes.MappingType
{
    interface IMappingAttribute
    {
        ColumnMapping GetColumnMapping(List<IRule> rules);
    }
}
