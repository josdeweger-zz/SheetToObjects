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

        /// <summary>
        /// Configure how the sheet maps to your model
        /// </summary>
        public SheetMapper For<TModel>(Func<MappingConfigBuilder<TModel>, MappingConfig> mappingConfigFunc)
        {
            var mappingConfig = mappingConfigFunc(new MappingConfigBuilder<TModel>());
            _mappingConfigs.Add(typeof(TModel), mappingConfig);

            return this;
        }

        /// <summary>
        /// Specify the sheet to map
        /// </summary>
        public SheetMapper Map(Sheet sheet)
        {
            _sheet = sheet;
            return this;
        }

        /// <summary>
        /// Returns a result containing the parsed result and validation errors
        /// </summary>
        public MappingResult<TModel> To<TModel>()
            where TModel : new()
        {
            var type = typeof(TModel);
            var parsedModels = new List<TModel>();
            var validationErrors = new List<ValidationError>();

            var mappingConfig = GetMappingConfig<TModel>(type);

            if (mappingConfig.HasHeaders)
                SetHeaderIndexesInColumnMappings(_sheet.Rows.FirstOrDefault(), mappingConfig);

            var dataRows = mappingConfig.HasHeaders ? _sheet.Rows.Skip(1).ToList() : _sheet.Rows;

            dataRows.ForEach(row =>
            {
                var rowValidationErrors = new List<ValidationError>();
                var obj = new TModel();
                var properties = obj.GetType().GetProperties().ToList();

                properties.ForEach(property =>
                {
                    var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

                    if (columnMapping.IsNull())
                        return;

                    var cell = row.GetCellByColumnIndex(columnMapping.ColumnIndex);

                    if (cell == null)
                    {
                        var validationError = ValidationError.CellNotFoundError(columnMapping.ColumnIndex, row.RowIndex,
                            columnMapping.DisplayName, property.Name);

                        if (columnMapping.IsRequired)
                        {
                            rowValidationErrors.Add(validationError);
                        }

                        return;
                    }

                    rowValidationErrors
                        .AddRange(ValidateValueByColumnMapping(cell.Value.ToString(), columnMapping, row.RowIndex, property.Name)
                        .ToList());
                    
                    ParseValue(property.PropertyType, cell.Value.ToString(), columnMapping.Format)
                        .OnSuccess(value => property.SetValue(obj, value))
                        .OnFailure(parseErrorMessage =>
                        {
                            var validationError = ValidationError.ParseValueError(
                                columnMapping.ColumnIndex,
                                row.RowIndex,
                                parseErrorMessage,
                                columnMapping.DisplayName,
                                cell.Value.ToString(),
                                property.Name);

                            if (columnMapping.IsRequired)
                            {
                                rowValidationErrors.Add(validationError);
                            }
                        });
                });

                if (rowValidationErrors.Any())
                    validationErrors.AddRange(rowValidationErrors);
                else
                    parsedModels.Add(obj);

            });

            return MappingResult<TModel>.Create(parsedModels, validationErrors);
        }

        private MappingConfig GetMappingConfig<TModel>(Type type) where TModel : new()
        {
            if (_mappingConfigs.TryGetValue(type, out var mappingConfig))
                return mappingConfig;

            var result = new MappingConfigByAttributeCreator<TModel>().CreateMappingConfigByAttributes();

            if (result.IsSuccess)
                return result.Value;

            throw new ApplicationException($"Could not find mapping configuration for type {type} " +
                                            $"and no SheetToObjectConfig attribute was set on the model " +
                                            $"to map the properties by data attributes");
        }

        private IEnumerable<ValidationError> ValidateValueByColumnMapping(string value, ColumnMapping mapping, int rowIndex, string propertyName)
        {
            return mapping.Rules
                .Select(rule => rule.Validate(value))
                .Where(validationResult => validationResult.IsFailure)
                .Select(validationResult =>
                    ValidationError.DoesNotMatchRuleError(
                        mapping.ColumnIndex,
                        rowIndex,
                        validationResult.Error,
                        mapping.DisplayName,
                        value,
                        propertyName));
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

        private Result<object, string> ParseValue(Type type, string value, string format)
        {
            switch (true)
            {
                case var _ when type == typeof(string):
                    return _cellValueParser.Parse<string>(value);
                case var _ when type == typeof(int) || type == typeof(int?):
                    return _cellValueParser.Parse<int>(value);
                case var _ when type == typeof(double) || type == typeof(double?):
                    return _cellValueParser.Parse<double>(value);
                case var _ when type == typeof(float) || type == typeof(float?):
                    return _cellValueParser.Parse<float>(value);
                case var _ when type == typeof(decimal) || type == typeof(decimal?):
                    return _cellValueParser.Parse<decimal>(value);
                case var _ when type == typeof(bool) || type == typeof(bool?):
                    return _cellValueParser.Parse<bool>(value);
                case var _ when type == typeof(DateTime) || type == typeof(DateTime?):
                    return _cellValueParser.ParseDateTime(value, format);
                case var _ when type.IsEnum:
                    return _cellValueParser.ParseEnumeration(value, type);
                default:
                    throw new NotImplementedException($"Parser for type {type} not implemented.");
            }
        }
    }
}