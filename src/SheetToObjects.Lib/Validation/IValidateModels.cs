using System.Collections.Generic;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.Validation
{
    internal interface IValidateModels
    {
        ValidationResult<TModel> Validate<TModel>(List<TModel> parsedModels, List<ColumnMapping> columnMappings) 
            where TModel : new();
    }
}