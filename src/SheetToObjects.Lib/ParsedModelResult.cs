namespace SheetToObjects.Lib
{
    public class ParsedModelResult<TModel> where TModel : new()
    {
        public TModel ParsedModel { get; }
        public int RowIndex { get; }

        public ParsedModelResult(TModel parsedModel, int rowIndex)
        {
            ParsedModel = parsedModel;
            RowIndex = rowIndex;
        }
    }
}