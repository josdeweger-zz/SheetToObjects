using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly ValueParser _valueParser = new ValueParser();
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
                    $"Could not find Mapping Configuration for type {typeof(T)}. Make sure to setup a configuration for the type");

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

                    var method = typeof(ValueParser).GetMethod("Parse");
                    var genericMethod = method.MakeGenericMethod(columnMapping.PropertyType);
                    var result = genericMethod.Invoke(_valueParser, new [] { cell.Value }) as Result;
                    
                    if(result.IsNotNull() && result.IsValid)
                        property.SetValue(obj, result.Value, null);
                }

                list.Add(obj);
            }

            return list;
        }
    }
}