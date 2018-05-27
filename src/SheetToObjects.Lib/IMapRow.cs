using System.Collections.Generic;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    internal interface IMapRow
    {
        Result<T, List<IValidationError>> Map<T>(Row row, MappingConfig mappingConfig) where T : new();
    }
}