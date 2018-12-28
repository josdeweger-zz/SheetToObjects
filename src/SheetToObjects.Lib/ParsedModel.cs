namespace SheetToObjects.Lib
{
    public class ParsedModel<TModel> where TModel : new()
    {
        public TModel Value { get; }
        public int RowIndex { get; }

        public ParsedModel(TModel value, int rowIndex)
        {
            Value = value;
            RowIndex = rowIndex;
        }
    }
}