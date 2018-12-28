using System.Collections.Generic;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    public interface IMapRow
    {
        Result<ParsedModel<TModel>, List<IValidationError>> Map<TModel>(Row row, MappingConfig mappingConfig)
            where TModel : new();
    }
}