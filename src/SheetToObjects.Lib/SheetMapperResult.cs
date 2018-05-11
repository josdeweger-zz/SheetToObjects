using System.Collections.Generic;


namespace SheetToObjects.Lib
{
    public class SheetMapperResult<T> where T : new()
    {
        public Dictionary<int, T> Result { get; }

        public Dictionary<int, List<SheetMapperError>> Errors { get; }

        public SheetMapperResult(Dictionary<int,T> result, Dictionary<int, List<SheetMapperError>> errors)
        {
            Result = result;
            Errors = errors;
        }
    }
}
