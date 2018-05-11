using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly CellValueParser _cellValueParser = new CellValueParser();
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

        public MappingResult<TModel> To<TModel>() 
            where TModel : new()
        {
            var parsedModels = new List<TModel>();
            var validationErrors = new List<ValidationError>();

            if (!_mappingConfigs.TryGetValue(typeof(TModel), out var mappingConfig))
                throw new ApplicationException(
                    $"Could not find Mapping Configuration for type {typeof(TModel)}. Make sure to setup a configuration for the type");
            
            _dataRows.ForEach(row => 
            {
                var obj = new TModel();
                var properties = obj.GetType().GetProperties().ToList();

                properties.ForEach(property =>
                {
                    var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

                    if (columnMapping.IsNull())
                        return;

                    var columnIndex = _headers.IndexOf(columnMapping.Header);
                    var cell = row.GetCellByColumnIndex(columnIndex);

                    ParseValue(columnMapping.PropertyType, cell)
                        .OnSuccess(value => property.SetValue(obj, value))
                        .OnFailure(validationError =>
                        {
                            validationErrors.Add(validationError);
                            property.SetValue(obj, columnMapping.PropertyType.GetDefault());
                        });
                });

                parsedModels.Add(obj);
            });

            return MappingResult<TModel>.Create(parsedModels, validationErrors);
        }

        private Result<object, ValidationError> ParseValue(Type type, Cell cell)
        {
            switch (true)
            {
                case var _ when type == typeof(string):
                    return _cellValueParser.ParseValueType<string>(cell);
                case var _ when type == typeof(int) || type == typeof(int?):
                    return _cellValueParser.ParseValueType<int>(cell);
                case var _ when type == typeof(double) || type == typeof(double?):
                    return _cellValueParser.ParseValueType<double>(cell);
                case var _ when type == typeof(bool) || type == typeof(bool?):
                    return _cellValueParser.ParseValueType<bool>(cell);
                case var _ when type.IsEnum:
                    return _cellValueParser.ParseEnumeration(cell, type);
                default:
                    throw new NotImplementedException($"Parser for type {type} not implemented.");
            }
        }
    }
}