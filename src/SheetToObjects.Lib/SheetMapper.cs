using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    public class SheetMapper<T> : IMapSheetTo<T> where T : new()
    {
        private readonly IMapRow _rowMapper;
        private readonly IValidateModels _modelValidator;
        private readonly Dictionary<Type, MappingConfig> _mappingConfigs = new Dictionary<Type, MappingConfig>();

        private SheetMapper(IMapRow rowMapper, IValidateModels modelValidator)
        {
            _rowMapper = rowMapper;
            _modelValidator = modelValidator;
        }

        public SheetMapper() : this(new RowMapper(new ValueMapper(new ValueParser())), new ModelValidator())
        {

        }

        /// <summary>
        /// Configure how the sheet maps to your model
        /// </summary>
        public SheetMapper<T> Configure(Func<MappingConfigBuilder<T>, MappingConfig> mappingConfigFunc)
        {
            var mappingConfig = mappingConfigFunc(new MappingConfigBuilder<T>());
            _mappingConfigs.Add(typeof(T), mappingConfig);

            return this;
        }

        /// <summary>
        /// Returns a result containing the parsed result and validation errors
        /// </summary>
        public MappingResult<T> Map(Sheet sheet)
        {
            var parsedModels = new List<T>();
            var validationErrors = new List<IValidationError>();

            var mappingConfig = GetMappingConfig();

            if (mappingConfig.HasHeaders)
                SetHeaderIndexesInColumnMappings(sheet.Rows.FirstOrDefault(), mappingConfig);

            var dataRows = mappingConfig.HasHeaders ? sheet.Rows.Skip(1).ToList() : sheet.Rows;

            dataRows.ForEach(row =>
            {
                _rowMapper.Map<T>(row, mappingConfig)
                    .OnSuccess(obj => parsedModels.Add(obj))
                    .OnFailure(rowValidationErrors => validationErrors.AddRange(rowValidationErrors));
            });

            var validationResult = _modelValidator.Validate(parsedModels, mappingConfig.ColumnMappings);

            return MappingResult<T>.Create(
                validationResult.ValidatedModels,
                validationErrors.Concat(validationResult.ValidationErrors).ToList()
            );
        }

        private MappingConfig GetMappingConfig()
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
    }
}