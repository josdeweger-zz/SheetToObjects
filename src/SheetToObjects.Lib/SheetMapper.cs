using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Configuration.ColumnMappings;
using SheetToObjects.Lib.Validation;

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
                List<ValidationError> columnValidationErrors = new List<ValidationError>();
                var obj = new TModel();
                var properties = obj.GetType().GetProperties().ToList();
                
                properties.ForEach(property =>
                {
                    var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

                    if (columnMapping.IsNull())
                        return;

                    var cell = row.GetCellByColumnIndex(columnMapping?.ColumnIndex ?? -1);

                    columnValidationErrors =
                        ValidateValueByColumnMapping(cell?.Value?.ToString(), columnMapping, row.RowIndex).ToList();

                    
                    ParseValue(property.PropertyType, cell?.Value?.ToString() ?? string.Empty, columnMapping?.ColumnIndex ?? -1, row.RowIndex, columnMapping.IsRequired)
                        .OnSuccess(value => property.SetValue(obj, value))
                        .OnFailure(validationError =>
                        {
                            columnValidationErrors.Add(validationError);
                            property.SetValue(obj, property.PropertyType.GetDefault());
                        });
                });

                if(columnValidationErrors?.Any() == true)
                    validationErrors.AddRange(columnValidationErrors);
                else 
                    parsedModels.Add(obj);
                
            });

            return MappingResult<TModel>.Create(parsedModels, validationErrors);
        }

        private IEnumerable<ValidationError> ValidateValueByColumnMapping(string value, ColumnMapping mapping, int rowIndex)
        {
            foreach (var rule in mapping.Rules)
            {
                var validationResult = rule.Validate(value);
                if (validationResult.IsFailure)
                {
                    yield return new ValidationError(mapping.ColumnIndex ?? -1, rowIndex, validationResult.Error);
                }
            }
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

        private Result<object, ValidationError> ParseValue(Type type, string value, int columnIndex, int rowIndex, bool isRequired)
        {
            switch (true)
            {
                case var _ when type == typeof(string):
                    return _cellValueParser.ParseValueType<string>(value, columnIndex, rowIndex, isRequired);
                case var _ when type == typeof(int) || type == typeof(int?):
                    return _cellValueParser.ParseValueType<int>(value, columnIndex, rowIndex, isRequired);
                case var _ when type == typeof(double) || type == typeof(double?):
                    return _cellValueParser.ParseValueType<double>(value, columnIndex, rowIndex,isRequired);
                case var _ when type == typeof(bool) || type == typeof(bool?):
                    return _cellValueParser.ParseValueType<bool>(value, columnIndex, rowIndex,isRequired);
                case var _ when type.IsEnum:
                    return _cellValueParser.ParseEnumeration(value, columnIndex, rowIndex, type);
                default:
                    throw new NotImplementedException($"Parser for type {type} not implemented.");
            }
        }
    }
}