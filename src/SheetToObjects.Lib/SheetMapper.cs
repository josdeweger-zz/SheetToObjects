using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Validation;
using SheetToObjects.Lib.ValueParsers;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly IParseValues _valueParser = new ValueParser();
        private readonly Dictionary<Type, MappingConfig> _mappingConfigs = new Dictionary<Type, MappingConfig>();
        private Sheet _sheet;

        public SheetMapper For<TModel>(Func<MappingConfigBuilder<TModel>, MappingConfig> mappingConfigFunc)
        {
            var mappingConfig = mappingConfigFunc(new MappingConfigBuilder<TModel>());
            _mappingConfigs.Add(typeof(TModel), mappingConfig);

            return this;
        }

        public SheetMapper Map(Sheet sheet)
        {
            _sheet = sheet;
            return this;
        }

        public List<T> To<T>() where T : new()
        {
            var list = new List<T>();

            if (!_mappingConfigs.TryGetValue(typeof(T), out var mappingConfig))
                throw new ApplicationException(
                    $"Could not find Mapping Configuration for type {typeof(T)}. Make sure you use new SheetMapper().Configure(cfg => cfg) to setup a configuration for the type");

            var rows = mappingConfig.DataHasHeaders ? _sheet.Rows.Skip(1) : _sheet.Rows;

            foreach (var row in rows)
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

                    var parsedValue = (Result)_valueParser.Parse(columnMapping.PropertyType, cell.Value);

                    if(parsedValue.HasValue)
                        property.SetValue(obj, parsedValue.Value, null);
                }

                list.Add(obj);
            }

            return list;
        }
    }
}