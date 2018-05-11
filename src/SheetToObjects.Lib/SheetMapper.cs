using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly CellValueParser _cellValueParser = new CellValueParser();
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

        public MappingResult<TModel> To<TModel>() 
            where TModel : new()
        {
            var parsedModels = new List<TModel>();
            var validationErrors = new List<ValidationError>();

            if (!_mappingConfigs.TryGetValue(typeof(TModel), out var mappingConfig))
            {
                mappingConfig = new MappingConfigBuilder<TModel>().Object();
            }
                

            if(mappingConfig.HasHeaders)
                SetHeaderIndexesInColumnMappings(_sheet.Rows.FirstOrDefault(), mappingConfig);

            List<Row> dataRows = mappingConfig.HasHeaders ? _sheet.Rows.Skip(1).ToList() : _sheet.Rows; 


            dataRows.ForEach(row => 
            {
                var obj = new TModel();
                var properties = obj.GetType().GetProperties().ToList();

                properties.ForEach(property =>
                {
                    var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

                    if (columnMapping.IsNull())
                        return;

                    var cell = row.GetCellByColumnIndex(columnMapping?.ColumnIndex ?? -1);

                    ParseValue(property.PropertyType, cell)
                        .OnSuccess(value => property.SetValue(obj, value))
                        .OnFailure(validationError =>
                        {
                            validationErrors.Add(validationError);
                            property.SetValue(obj, property.PropertyType.GetDefault());
                        });
                });

                parsedModels.Add(obj);
            });

            return MappingResult<TModel>.Create(parsedModels, validationErrors);
        }

        private void SetHeaderIndexesInColumnMappings(Row firstRow, MappingConfig mappingConfig)
        {
            foreach (var columnMapping in mappingConfig.ColumnMappings.OfType<IUseHeaderRow>())
            {
                var headerCell = firstRow.Cells.FirstOrDefault(c => c.Value.ToString().Equals(columnMapping.ColumnName.ToString(), StringComparison.OrdinalIgnoreCase));
                if (headerCell != null)
                {
                    columnMapping.SetColumnIndex(headerCell.ColumnIndex);
                }
            }
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