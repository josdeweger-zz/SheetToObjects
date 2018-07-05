using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SheetToObjects.Core;
using SheetToObjects.Lib.Exceptions;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.FluentConfiguration
{
    public class ColumnMappingBuilder<T>
    {
        private string _header;
        private int _columnIndex = -1;
        private string _columnLetter;
        private string _propertyName;
        private string _format;
        private object _defaultValue;
        private bool _isRequiredInHeaderRow;
        private readonly List<IParsingRule> _parsingRules = new List<IParsingRule>();
        private readonly List<IRule> _rules = new List<IRule>();

        /// <summary>
        /// Map to column by header (other options are to map by column index or column letter)
        /// </summary>
        public ColumnMappingBuilder<T> WithHeader(string header)
        {
            _header = header;
            return this;
        }

        /// <summary>
        /// Map to column by column index (other options are to map by header or column letter)
        /// </summary>
        public ColumnMappingBuilder<T> WithColumnIndex(int columnIndex)
        {
            _columnIndex = columnIndex;
            return this;
        }

        /// <summary>
        /// Map to column by column letter (other options are to map by header or column index)
        /// </summary>
        public ColumnMappingBuilder<T> WithColumnLetter(string columnLetter)
        {
            _columnLetter = columnLetter; 
            return this;
        }

        /// <summary>
        /// Set a default value for non-nullable value types that are not required
        /// </summary>
        public ColumnMappingBuilder<T> WithDefaultValue<TValue>(TValue defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        public ColumnMappingBuilder<T> WithRequiredInHeaderRow()
        {
            _isRequiredInHeaderRow = true;
            return this;
        }

    /// <summary>
        /// Add new rule that the column needs to adhere to during parsing
        /// </summary>
        public ColumnMappingBuilder<T> AddParsingRule(IParsingRule rule)
        {
            _parsingRules.Add(rule);
            return this;
        }

        /// <summary>
        /// Add new rule that the column needs to adhere to. After parsing these rules are validated
        /// </summary>
        public ColumnMappingBuilder<T> AddRule(IRule rule)
        {
            _rules.Add(rule);
            return this;
        }

        /// <summary>
        /// Values in this column need to satisfy the custom func
        /// </summary>
        public ColumnMappingBuilder<T> WithCustomRule<TValue>(
            Func<TValue, bool> customRuleFunc,
            string validationMessage)
        {
            AddRule(new CustomRule<TValue>(customRuleFunc, validationMessage));
            return this;
        }

        /// <summary>
        /// Use format for parsing, can only be used for DateTime formats, e.g. "dd-MM-yyyy"
        /// </summary>
        public ColumnMappingBuilder<T> UsingFormat(string format)
        {
            _format = format;
            return this;
        }

        /// <summary>
        /// Specify the model property this column needs to map to. The type is inferred from the type on the model
        /// </summary>
        public ColumnMapping MapTo<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            var modelType = typeof(T);
            var propertyType = typeof(TProperty);

            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");

            var propertyInfo = member.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

            if (modelType != propertyInfo.ReflectedType && !modelType.IsSubclassOf(propertyInfo.ReflectedType))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {modelType}.");

            var hasRequiredRule = _parsingRules.Any(r => r.GetType() == typeof(RequiredRule));

            if (propertyType.IsNotNullable()
                && propertyType.IsValueType
                && !hasRequiredRule
                && _defaultValue.IsNull())
            {
                throw new MappingConfigurationException(
                    $"Non-nullable property '{propertyInfo.Name}' is not required and therefor needs a default value.");
            }
            
            return MapTo(propertyInfo);
        }

        /// <summary>
        /// Specify the model property this column needs to map to by specifying the property info
        /// </summary>
        public ColumnMapping MapTo(PropertyInfo property)
        {
            if(property.IsNull())
                throw new ArgumentNullException($"Property is null");
            
            _propertyName = property.Name;

            if(_header.IsNotNullOrWhiteSpace())
                return new NameColumnMapping(_header, _propertyName, _format, _parsingRules, _rules, _defaultValue,_isRequiredInHeaderRow);
            if(_columnLetter.IsNotNullOrWhiteSpace())
                return new LetterColumnMapping(_columnLetter, _propertyName, _format, _parsingRules, _rules, _defaultValue);
            if(_columnIndex >= 0)
                return new IndexColumnMapping(_columnIndex, _propertyName, _format, _parsingRules, _rules, _defaultValue);

            return new PropertyColumnMapping(_propertyName, _format, _parsingRules, _rules, _defaultValue,_isRequiredInHeaderRow);
        }
    }
}
