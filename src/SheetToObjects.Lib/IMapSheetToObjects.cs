using System.Collections.Generic;

namespace SheetToObjects.Lib
{
    public interface IMapSheetToObjects
    {
        SheetMapper Map(Sheet sheet);

        List<T> To<T>() where T : new();

        List<T> To<T>(MappingConfig mappingConfig) where T : new();
    }
}