using SheetToObjects.Lib;

namespace SheetToObjects.Specs.Builders
{
    public class CellBuilder
    {
        private string _columnLetter;
        private int _rowNumber;
        private object _value;

        public CellBuilder WithColumnLetter(string columnLetter)
        {
            _columnLetter = columnLetter;
            return this;
        }

        public CellBuilder WithRowNumber(int rowNumber)
        {
            _rowNumber = rowNumber;
            return this;
        }

        public CellBuilder WithValue(object value)
        {
            _value = value;
            return this;
        }

        public Cell Build()
        {
            return new Cell(_columnLetter, _rowNumber, _value);
        }
    }
}