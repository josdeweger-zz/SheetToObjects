using System;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib
{
    public interface IMapSheetTo<T> where T : new()
    {
        SheetMapper<T> Configure(Func<MappingConfigBuilder<T>, MappingConfig> mappingConfigFunc);

        MappingResult<T> Map(Sheet sheet);
    }
}