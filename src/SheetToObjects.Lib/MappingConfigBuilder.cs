using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SheetToObjects.Lib
{
    public class MappingConfigBuilder
    {
        private readonly MappingConfig _mappingConfig = new MappingConfig();
        
        public ColumnMappingBuilder<TModel> For<TModel>()
        {
            _mappingConfig.ForType = typeof(TModel);
            return new ColumnMappingBuilder<TModel>(_mappingConfig);
        }

        public MappingConfig Build()
        {
            return _mappingConfig;
        }
    }

    public class ColumnMappingBuilder<TModel>
    {
        private string _columnLetter;
        private string _propertyName;
        private Type _propertyType;
        private bool _isRequired;
        private readonly MappingConfig _mappingConfig;

        public ColumnMappingBuilder(MappingConfig mappingConfig)
        {
            _mappingConfig = mappingConfig;
        }

        public ColumnMappingBuilder<TModel> Column(string columnLetter)
        {
            _columnLetter = columnLetter;
            return this;
        }

        public ColumnMappingBuilder<TModel> IsRequired()
        {
            _isRequired = true;
            return this;
        }

        public ColumnMappingBuilder<TModel> MapTo<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
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
            
            _mappingConfig.ColumnMappings.Add(new ColumnMapping(_columnLetter, _propertyName, _propertyType, _isRequired));

            return this;
        }

        public MappingConfig Build()
        {
            return _mappingConfig;
        }
    }
}