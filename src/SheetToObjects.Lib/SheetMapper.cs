using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly MappingConfig _mappingConfig;
        private Sheet _sheet;

        public SheetMapper()
        {
            
        }

        public SheetMapper(IProvideMappingConfig mappingConfigProvider)
        {
            _mappingConfig = mappingConfigProvider.Get();
        }

        public SheetMapper Map(Sheet sheet)
        {
            _sheet = sheet;
            return this;
        }

        public List<T> To<T>()
            where T : new()
        {
            return To<T>(_mappingConfig);
        }

        public List<T> To<T>(MappingConfig mappingConfig)
            where T : new()
        {
            var list = new List<T>();

            foreach (var row in _sheet.Rows)
            {
                var obj = new T();
                var objType = obj.GetType();

                foreach (var property in objType.GetProperties())
                {
                    var columnMapping = mappingConfig.ColumnMappings.FirstOrDefault(m =>
                        m.PropertyName.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (columnMapping.IsNull())
                        continue;

                    var cell = row.Cells.FirstOrDefault(c =>
                        c.ColumnLetter.Equals(columnMapping.ColumnLetter,
                            StringComparison.InvariantCultureIgnoreCase));

                    if (cell.IsNull() || cell.Value.IsNull())
                        continue;

                    switch (true)
                    {
                        case var _ when columnMapping.PropertyType == typeof(string):
                            property.SetValue(obj, cell.Value.ToString(), null);
                            break;
                        case var _ when columnMapping.PropertyType == typeof(int) || columnMapping.PropertyType == typeof(int?):
                            if (int.TryParse(cell.Value.ToString(), out var intValue))
                            {
                                property.SetValue(obj, intValue, null);
                            }
                            break;
                        case var _ when columnMapping.PropertyType == typeof(double) || columnMapping.PropertyType == typeof(double?):
                            if (double.TryParse(cell.Value.ToString(), out var doubleValue))
                            {
                                property.SetValue(obj, doubleValue, null);
                            }
                            break;
                    }
                }

                list.Add(obj);
            }

            return list;
        }
    }
}