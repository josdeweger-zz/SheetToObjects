using System;
using System.Collections.Generic;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly Dictionary<Type, MappingConfig> _mappingConfigs = new Dictionary<Type, MappingConfig>();
        private Sheet _sheet;

        public SheetMapper()
        {
        }

        public SheetMapper(MappingConfig mappingConfig)
        {
            _mappingConfigs.Add(mappingConfig.ForType, mappingConfig);
        }

        public SheetMapper Configure(Func<MappingConfigBuilder, MappingConfig> mappingConfigFunc)
        {
            var mappingConfig = mappingConfigFunc(new MappingConfigBuilder());
            _mappingConfigs.Add(mappingConfig.ForType, mappingConfig);
            return this;
        }

        public SheetMapper Map(Sheet sheet)
        {
            _sheet = sheet;
            return this;
        }

        public List<T> To<T>()
            where T : new()
        {
            var list = new List<T>();

            if (!_mappingConfigs.TryGetValue(typeof(T), out var mappingConfig))
                throw new ApplicationException(
                    $"Could not find Mapping Configuration for type {typeof(T)}. Make sure you use new SheetMapper().Configure(cfg => cfg) to setup a configuration for the type");

            foreach (var row in _sheet.Rows)
            {
                var obj = new T();
                var objType = obj.GetType();

                foreach (var property in objType.GetProperties())
                {
                    var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

                    if (columnMapping.IsNull())
                        continue;

                    var cell = row.GetCellByColumnLetter(columnMapping.ColumnLetter);
                    
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