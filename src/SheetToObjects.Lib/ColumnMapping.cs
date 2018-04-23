using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SheetToObjects.Lib
{
    public class ColumnMapping<TModel> : IColumnMapping
    {
        private readonly MappingConfig _mappingConfiguration;

        public string ColumnLetter { get; private set; }
        public bool Required { get; private set; }
        public string PropertyName { get; private set; }
        public Type PropertyType { get; private set; }

        public ColumnMapping(MappingConfig mappingConfiguration)
        {
            _mappingConfiguration = mappingConfiguration;
        }

        public ColumnMapping<TModel> Column(string columnLetter)
        {
            ColumnLetter = columnLetter;
            return this;
        }

        public ColumnMapping<TModel> IsRequired()
        {
            Required = true;
            return this;
        }

        public ColumnMapping<TModel> MapTo<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
        {
            var type = typeof(TModel);

            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            
            var propertyInfo = member.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

            if (type != propertyInfo.ReflectedType && !type.IsSubclassOf(propertyInfo.ReflectedType))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {type}.");

            PropertyName = propertyInfo.Name;
            PropertyType = propertyInfo.PropertyType;

            _mappingConfiguration.ColumnMappings.Add(this);

            return new ColumnMapping<TModel>(_mappingConfiguration);
        }

        public MappingConfig Build()
        {
            return _mappingConfiguration;
        }
    }
}