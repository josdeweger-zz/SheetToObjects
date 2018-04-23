using System.Collections.Generic;

namespace SheetToObjects.Lib
{
    public class MappingConfig
    {
        public IList<IColumnMapping> ColumnMappings = new List<IColumnMapping>();

        public ColumnMapping<TModel> For<TModel>()
        {
            return new ColumnMapping<TModel>(this);
        }
    }
}