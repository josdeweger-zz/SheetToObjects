using System;

namespace SheetToObjects.Lib
{
    public class ColumnConfigBuilder
    {
        private string _columnLetter;
        private Type _valueType;
        private bool _required;


        public ColumnConfigBuilder WithLetter(string columnLetter)
        {
            _columnLetter = columnLetter;

            return this;
        }

        public ColumnConfigBuilder OfType<T>()
        {
            _valueType = typeof(T);

            return this;
        }

        public ColumnConfigBuilder IsRequired()
        {
            _required = true;

            return this;
        }

        public ColumnConfig Build()
        {
            return new ColumnConfig(_valueType, _columnLetter, _required);
        }
    }
}