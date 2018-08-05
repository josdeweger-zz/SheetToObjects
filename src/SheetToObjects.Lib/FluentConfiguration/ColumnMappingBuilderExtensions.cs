using System;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.FluentConfiguration
{
    public static class ColumnMappingBuilderExtensions
    {
        /// <summary>
        /// Make values in this column required, which means it can not be empty
        /// </summary>
        public static ColumnMappingBuilder<T> IsRequired<T>(this ColumnMappingBuilder<T> columnMappingBuilder)
        {
            columnMappingBuilder.AddParsingRule(new RequiredRule());
            return columnMappingBuilder;
        }

        /// <summary>
        /// Values in this column need to match the given regex
        /// </summary>
        public static ColumnMappingBuilder<T> Matches<T>(this ColumnMappingBuilder<T> columnMappingBuilder, string regex)
        {
            columnMappingBuilder.AddRule(new RegexRule(regex));
            return columnMappingBuilder;
        }

        /// <summary>
        /// Values in this column have to have at least the minimum given value
        /// </summary>
        public static ColumnMappingBuilder<T> WithMinimum<T, TComparer>(this ColumnMappingBuilder<T> columnMappingBuilder, TComparer comparer)
            where TComparer : IComparable<TComparer>
        {
            columnMappingBuilder.AddRule(new MinimumRule<TComparer>(comparer));
            return columnMappingBuilder;
        }

        /// <summary>
        /// Values in this column can have a maximum of the given value
        /// </summary>
        public static ColumnMappingBuilder<T> WithMaximum<T, TComparer>(this ColumnMappingBuilder<T> columnMappingBuilder, TComparer comparer)
            where TComparer : IComparable<TComparer>
        {
            columnMappingBuilder.AddRule(new MaximumRule<TComparer>(comparer));
            return columnMappingBuilder;
        }
        
        /// <summary>
        /// Values in this column have to be unique
        /// </summary>
        public static ColumnMappingBuilder<T> ShouldHaveUniqueValues<T>(this ColumnMappingBuilder<T> columnMappingBuilder)
        {
            columnMappingBuilder.AddRule(new UniqueValuesInColumnRule());
            return columnMappingBuilder;
        }
    }
}