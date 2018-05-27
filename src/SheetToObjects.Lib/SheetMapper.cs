using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib
{
    public class SheetMapper : IMapSheetToObjects
    {
        private readonly IMapRow _rowMapper;
        private readonly Dictionary<Type, MappingConfig> _mappingConfigs = new Dictionary<Type, MappingConfig>();
        private Sheet _sheet;

        private SheetMapper(IMapRow rowMapper)
        {
            _rowMapper = rowMapper;
        }

        public SheetMapper() : this(new RowMapper(new ValueMapper(new ValueParser())))
        {
        }

        /// <summary>
        /// Configure how the sheet maps to your model
        /// </summary>
        public SheetMapper For<T>(Func<MappingConfigBuilder<T>, MappingConfig> mappingConfigFunc)
        {
            var mappingConfig = mappingConfigFunc(new MappingConfigBuilder<T>());
            _mappingConfigs.Add(typeof(T), mappingConfig);

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
        public MappingResult<T> To<T>()
            where T : new()
        {
            var type = typeof(T);
            var parsedModels = new List<T>();
            var validationErrors = new List<ValidationError>();

            var mappingConfig = GetMappingConfig<T>(type);

            if (mappingConfig.HasHeaders)
                SetHeaderIndexesInColumnMappings(_sheet.Rows.FirstOrDefault(), mappingConfig);

            var dataRows = mappingConfig.HasHeaders ? _sheet.Rows.Skip(1).ToList() : _sheet.Rows;

            dataRows.ForEach(row =>
            {
                _rowMapper.Map<T>(row, mappingConfig)
                    .OnSuccess(obj => parsedModels.Add(obj))
                    .OnFailure(rowValidationErrors => validationErrors.AddRange(rowValidationErrors));
            });

            return MappingResult<T>.Create(parsedModels, validationErrors);
        }

        private MappingConfig GetMappingConfig<T>(Type type) where T : new()
        {
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