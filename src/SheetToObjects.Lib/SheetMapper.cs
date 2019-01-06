using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Lib.Parsing;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly IMapRow _rowMapper;
        private readonly IValidateModels _modelValidator;
        private readonly ConcurrentDictionary<Type, MappingConfig> _mappingConfigs = new ConcurrentDictionary<Type, MappingConfig>();

        private SheetMapper(IMapRow rowMapper, IValidateModels modelValidator)
        {
            _rowMapper = rowMapper;
            _modelValidator = modelValidator;
        }

        public SheetMapper() : this(new RowMapper(new ValueMapper(new ParsingStrategyProvider())), new ModelValidator())
        {

        }
        
        /// <summary>
        /// Adds a SheetMap that contains configuration about how the sheet maps to your model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="sheetMap"></param>
        /// <returns></returns>
        public SheetMapper AddSheetToObjectConfig<TModel>(SheetToObjectConfig<TModel> sheetMap) where TModel : new()
        {
            var mappingConfig = sheetMap.MappingConfig;
            _mappingConfigs.AddOrUpdate(typeof(TModel), mappingConfig, (type, existingConfig) => mappingConfig);

            return this;
        }

        /// <summary>
        /// Configure how the sheet maps to your model
        /// </summary>
        public SheetMapper AddConfigFor<T>(Func<MappingConfigBuilder<T>, MappingConfigBuilder<T>> mappingConfigFunc)
            where T : new()
        {
            var mappingConfig = mappingConfigFunc(new MappingConfigBuilder<T>()).Build();
            _mappingConfigs.AddOrUpdate(typeof(T), mappingConfig, (type, existingConfig) => mappingConfig);

            return this;
        }

        /// <summary>
        /// Returns a result containing the parsed result and validation errors
        /// </summary>
        public MappingResult<T> Map<T>(Sheet sheet)
            where T : new()
        {
            var parsedModels = new List<ParsedModel<T>>();
            var validationErrors = new List<IValidationError>();

            var mappingConfig = GetMappingConfig<T>();

            if (mappingConfig.HasHeaders)
            {
                var headerValidationErrors = HandleHeaderRow(sheet.Rows.FirstOrDefault(), mappingConfig);
                if (headerValidationErrors.Any())
                {
                    return MappingResult<T>.Create(
                        parsedModels,
                        headerValidationErrors
                    );
                }
            }

            MapRows(sheet, mappingConfig, parsedModels, validationErrors);

            var validationResult = _modelValidator.Validate(parsedModels, mappingConfig.ColumnMappings);

            return MappingResult<T>.Create(
                validationResult.ValidatedModels,
                validationErrors.Concat(validationResult.ValidationErrors).ToList()
            );
        }

        private void MapRows<T>(Sheet sheet, MappingConfig mappingConfig, List<ParsedModel<T>> parsedModels, List<IValidationError> validationErrors)
            where T : new()
        {
            var dataRows = mappingConfig.HasHeaders ? sheet.Rows.Skip(1).ToList() : sheet.Rows;

            if (mappingConfig.StopParsingAtFirstEmptyRow)
                dataRows = FilterAfterFirstEmptyRow<T>(dataRows);
            
            dataRows.ForEach(row =>
            {
                _rowMapper.Map<T>(row, mappingConfig)
                    .OnSuccess(result => parsedModels.Add(result))
                    .OnFailure(rowValidationErrors => validationErrors.AddRange(rowValidationErrors));
            });
        }

        private static List<Row> FilterAfterFirstEmptyRow<T>(List<Row> dataRows) 
            where T : new()
        {
            var firstEmptyRow = dataRows
                .FirstOrDefault(d => d.Cells
                    .TrueForAll(c => c.Value.IsNull() ||
                                     c.Value is string s && s.IsNullOrEmpty()));

            if (firstEmptyRow.IsNotNull())
                dataRows = dataRows.Where(r => r.RowIndex < firstEmptyRow.RowIndex).ToList();

            return dataRows;
        }

        private MappingConfig GetMappingConfig<T>()
        {
            var type = typeof(T);

            if (_mappingConfigs.TryGetValue(type, out var mappingConfig))
                return mappingConfig;

            var result = new MappingConfigByAttributeCreator<T>().CreateMappingConfig();

            if (result.IsSuccess)
                return result.Value;

            throw new ArgumentException($"Could not find mapping configuration for type {type} " +
                                            $"and no SheetToObjectConfig attribute was set on the model " +
                                            $"to map the properties by data attributes");
        }

        private List<IValidationError> HandleHeaderRow(Row firstRow, MappingConfig mappingConfig)
        {
            var validationErrors = new List<IValidationError>();

            foreach (var columnMapping in mappingConfig.ColumnMappings.OfType<IUseHeaderRow>())
            {
                var headerCell = firstRow.Cells.FirstOrDefault(c => c.Value.ToString().Equals(columnMapping.ColumnName.ToString(), StringComparison.OrdinalIgnoreCase));
                if (headerCell != null)
                {
                    columnMapping.SetColumnIndex(headerCell.ColumnIndex);
                }
                else if (columnMapping.IsRequiredInHeaderRow)
                {
                    validationErrors.Add(ParsingValidationError.CouldNotFindHeader(-1, 0,columnMapping.ColumnName, columnMapping.ColumnName));
                }

            }

            return validationErrors;
        }
    }
}