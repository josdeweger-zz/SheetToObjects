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
        private readonly ValueParser _cellValueParser = new ValueParser();
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
                mappingConfig = new MappingConfigBuilder<TModel>().BuildConfig();
            }
                

            if(mappingConfig.HasHeaders)
                SetHeaderIndexesInColumnMappings(_sheet.Rows.FirstOrDefault(), mappingConfig);

            List<Row> dataRows = mappingConfig.HasHeaders ? _sheet.Rows.Skip(1).ToList() : _sheet.Rows; 


            dataRows.ForEach(row =>
            {
                List<ValidationError> rowValidationErrors = new List<ValidationError>();
                var obj = new TModel();
                var properties = obj.GetType().GetProperties().ToList();
                
                properties.ForEach(property =>
                {
                    var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

                    if (columnMapping.IsNull())
                        return;

                    var cell = row.GetCellByColumnIndex(columnMapping?.ColumnIndex ?? -1);

                    if (cell == null)
                    {
                        var validationError = new ValidationError(columnMapping?.ColumnIndex, row.RowIndex, "Cell not found", columnMapping?.DisplayName, null, property.Name);
                        CheckToAddValidationErrorToList(rowValidationErrors, columnMapping, validationError);
                        return;
                    }
                    
                    rowValidationErrors.AddRange(
                        ValidateValueByColumnMapping(cell.Value.ToString(), columnMapping, row.RowIndex, property.Name).ToList());

                    
                    ParseValue(property.PropertyType, cell?.Value?.ToString() ?? string.Empty)
                        .OnSuccess(value => property.SetValue(obj, value))
                        .OnFailure(parseErrorMessage =>
                        {
                            var validationError = new ValidationError(columnMapping?.ColumnIndex, row.RowIndex, parseErrorMessage, columnMapping?.DisplayName, cell?.Value?.ToString(), property.Name);
                            CheckToAddValidationErrorToList(rowValidationErrors, columnMapping, validationError);
                        });
                });

                if(rowValidationErrors?.Any() == true)
                    validationErrors.AddRange(rowValidationErrors);
                else 
                    parsedModels.Add(obj);
                
            });

            return MappingResult<TModel>.Create(parsedModels, validationErrors);
        }


        private void CheckToAddValidationErrorToList(List<ValidationError> validationErrorList, ColumnMapping columnMapping, 
            ValidationError validationError)
        {
            if (columnMapping.IsRequired)
            {
                validationErrorList.Add(validationError);
            }
        }

        private IEnumerable<ValidationError> ValidateValueByColumnMapping(string value, ColumnMapping mapping, int rowIndex, string propertyName)
        {
            foreach (var rule in mapping.Rules)
            {
                var validationResult = rule.Validate(value);
                if (validationResult.IsFailure)
                {
                    yield return new ValidationError(mapping.ColumnIndex, rowIndex, validationResult.Error, mapping.DisplayName, value, propertyName);
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

        private Result<object, string> ParseValue(Type type, string value)
        {
            switch (true)
            {
                case var _ when type == typeof(string):
                    return _cellValueParser.ParseValueType<string>(value);
                case var _ when type == typeof(int) || type == typeof(int?):
                    return _cellValueParser.ParseValueType<int>(value);
                case var _ when type == typeof(double) || type == typeof(double?):
                    return _cellValueParser.ParseValueType<double>(value);
                case var _ when type == typeof(bool) || type == typeof(bool?):
                    return _cellValueParser.ParseValueType<bool>(value);
                case var _ when type.IsEnum:
                    return _cellValueParser.ParseEnumeration(value, type);
                default:
                    throw new NotImplementedException($"Parser for type {type} not implemented.");
            }
        }
    }
}