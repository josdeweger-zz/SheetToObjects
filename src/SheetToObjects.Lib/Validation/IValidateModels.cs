using System.Collections.Generic;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.Validation
{
    internal interface IValidateModels
    {
        ValidationResult<ParsedModelResult<TModel>> Validate<TModel>(
            List<ParsedModelResult<TModel>> parsedModels, 
            List<ColumnMapping> columnMappings) 
            where TModel : new();
    }
}