using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration
{
    public class ColumnMappingBuilder<TModel>
    {
        private string _columnLetter;
        private string _propertyName;
        private Type _propertyType;
        private readonly List<IRule> _rules = new List<IRule>();

        public ColumnMappingBuilder<TModel> WithLetter(string columnLetter)
        {
            _columnLetter = columnLetter;
            return this;
        }

        public ColumnMappingBuilder<TModel> AddRule(IRule rule)
        {
            _rules.Add(rule);
            return this;
        }

        public ColumnMappingBuilder<TModel> IsRequired()
        {
            _rules.Add(new RequiredRule());
            return this;
        }

        public ColumnMappingBuilder<TModel> Matches(string regex)
        {
            _rules.Add(new RegexRule(regex));
            return this;
        }

        public ColumnMapping MapTo<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
        {
            var type = typeof(TModel);

            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");

            var propertyInfo = member.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

            if (type != propertyInfo.ReflectedType && !type.IsSubclassOf(propertyInfo.ReflectedType))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {type}.");

            _propertyName = propertyInfo.Name;
            _propertyType = propertyInfo.PropertyType;

            var columnMapping = new ColumnMapping(_columnLetter, _propertyName, _propertyType);
            columnMapping.AddRules(_rules);

            return columnMapping;
        }
    }
}