using System.Collections.Generic;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.Validation
{
    internal interface IValidateColumn
    {
        ValidationResult<ParsedModel<TModel>> Validate<TModel>(
            List<ParsedModel<TModel>> parsedModels, 
            List<ColumnMapping> columnMappings) 
            where TModel : new();
    }
}