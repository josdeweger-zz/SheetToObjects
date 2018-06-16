using System;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib
{
    public interface IMapSheetToObjects
    {
        SheetMapper AddConfigFor<T>(Func<MappingConfigBuilder<T>, MappingConfigBuilder<T>> mappingConfigFunc)
            where T : new();

        MappingResult<T> Map<T>(Sheet sheet) where T : new();
    }
}