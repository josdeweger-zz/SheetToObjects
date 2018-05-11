using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SheetToObjects.Core;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration.ColumnMappings
{
    public class ColumnMappingBuilder<TModel>
    {
        private string _header;
        private int _columnIndex = -1;
        private string _columnLetter;
        private string _propertyName;
        private readonly List<IRule> _rules = new List<IRule>();

        public ColumnMappingBuilder<TModel> WithHeader(string header)
        {
            _header = header;
            return this;
        }
        
        public ColumnMappingBuilder<TModel> WithColumnIndex(int columnIndex)
        {
            _columnIndex = columnIndex; ;
            return this;
        }

        public ColumnMappingBuilder<TModel> WithColumnLetter(string columnLetter)
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

            return this.MapTo(propertyInfo);
        }

        public ColumnMapping MapTo(PropertyInfo property)
        {
            if(property.IsNull())
                throw new ArgumentNullException($"Property is null");
            
            _propertyName = property.Name;

            if(_header.IsNotNullOrWhiteSpace())
                return new NameColumnMapping(_header, _propertyName, _rules);
            if(_columnLetter.IsNotNullOrWhiteSpace())
                return new LetterColumnMapping(_columnLetter, _propertyName, _rules);
            if(_columnIndex >= 0)
                return new IndexColumnMapping(_columnIndex, _propertyName, _rules);

            return new PropertyColumnMapping(_propertyName, _rules);
        }


    }
}
