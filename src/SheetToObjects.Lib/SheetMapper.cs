using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly ValueParser _valueParser = new ValueParser();
        private readonly Dictionary<Type, MappingConfig> _mappingConfigs = new Dictionary<Type, MappingConfig>();
        private Sheet _sheet;
        private List<string> _headers;
        private List<Row> _dataRows;

        public SheetMapper For<TModel>(Func<MappingConfigBuilder<TModel>, MappingConfig> mappingConfigFunc)
        {
            var mappingConfig = mappingConfigFunc(new MappingConfigBuilder<TModel>());
            _mappingConfigs.Add(typeof(TModel), mappingConfig);

            return this;
        }

        public SheetMapper Map(Sheet sheet)
        {
            _sheet = sheet;
            _headers = sheet.Rows.First().Cells.Select(c => c.Value.ToString().ToLowerInvariant()).ToList();
            _dataRows = _sheet.Rows.Skip(1).ToList();
            return this;
        }

        public List<T> To<T>() 
            where T : new()
        {
            var list = new List<T>();

            if (!_mappingConfigs.TryGetValue(typeof(T), out var mappingConfig))
                throw new ApplicationException(
                    $"Could not find Mapping Configuration for type {typeof(T)}. Make sure to setup a configuration for the type");
            
            _dataRows.ForEach(row =>
            {
                var obj = new T();
                var objType = obj.GetType();

                objType
                    .GetProperties()
                    .ToList()
                    .ForEach(property => ParseValue(mappingConfig, property, row, obj));

                list.Add(obj);
            });

            return list;
        }

        private void ParseValue<T>(MappingConfig mappingConfig, PropertyInfo property, Row row, T obj) 
            where T : new()
        {
            var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

            if (columnMapping.IsNull())
                return;

            var columnIndex = _headers.IndexOf(columnMapping.Header);
            var cell = row.GetCellByColumnIndex(columnIndex);

            if (cell.IsNull() || cell.Value.IsNull())
                return;

            var type = columnMapping.PropertyType;

            var errorMessages = new List<string>();

            switch (true)
            {
                case var _ when type == typeof(string):
                    _valueParser.Parse<string>(cell.Value)
                        .OnSuccess(parsedValue => property.SetValue(obj, parsedValue))
                        .OnFailure(errorMessages.Add);
                    break;
                case var _ when type == typeof(int) || type == typeof(int?):
                    _valueParser.Parse<int>(cell.Value)
                        .OnSuccess(parsedValue => property.SetValue(obj, parsedValue))
                        .OnFailure(errorMessages.Add);
                    break;
                case var _ when type == typeof(double) || type == typeof(double?):
                    _valueParser.Parse<double>(cell.Value)
                        .OnSuccess(parsedValue => property.SetValue(obj, parsedValue))
                        .OnFailure(errorMessages.Add);
                    break;
                case var _ when type == typeof(bool) || type == typeof(bool?):
                    _valueParser.Parse<bool>(cell.Value)
                        .OnSuccess(parsedValue => property.SetValue(obj, parsedValue))
                        .OnFailure(errorMessages.Add);
                    break;
                case var _ when type.IsEnum:
                    _valueParser.Parse(cell.Value, type)
                        .OnSuccess(parsedValue => property.SetValue(obj, parsedValue))
                        .OnFailure(errorMessages.Add);                    
                    break;
                default:
                    throw new NotImplementedException($"Parser for type {type} not implemented.");
            }
        }
    }
}