using System.Collections.Generic;
using System.Linq;

namespace SheetToObjects.Lib
{
    public class MappingResult<T>
        where T : new()
    {
        public List<T> ParsedModels { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public bool IsFailure => ValidationErrors.Any();
        public bool IsSuccess => !IsFailure;

        private MappingResult(List<T> parsedModels, List<ValidationError> validationErrors)
        {
            ParsedModels = parsedModels;
            ValidationErrors = validationErrors;
        }

        public static MappingResult<T> Create(List<T> parsedModels,List<ValidationError> validationErrors)
        {
            return new MappingResult<T>(parsedModels, validationErrors);
        }
    }
}