using SheetToObjects.Lib;

namespace SheetToObjects.Specs.Builders
{
    public class CellBuilder
    {
        private int _columnIndex;
        private int _rowIndex;
        private object _value;

        public CellBuilder WithColumnIndex(int columnIndex)
        {
            _columnIndex = columnIndex;
            return this;
        }

        public CellBuilder WithRowIndex(int rowIndex)
        {
            _rowIndex = rowIndex;
            return this;
        }

        public CellBuilder WithValue(object value)
        {
            _value = value;
            return this;
        }

        public Cell Build()
        {
            return new Cell(_columnIndex, _rowIndex, _value);
        }
    }
}