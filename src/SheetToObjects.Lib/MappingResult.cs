using System.Collections.Generic;
using System.Linq;

namespace SheetToObjects.Lib
{
    public class MappingResult<TModel>
        where TModel : new()
    {
        public List<TModel> ParsedModels { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public bool IsFailure => ValidationErrors.Any();
        public bool IsSuccess => !IsFailure;

        private MappingResult(List<TModel> parsedModels, List<ValidationError> validationErrors)
        {
            ParsedModels = parsedModels;
            ValidationErrors = validationErrors;
        }

        public static MappingResult<TModel> Create(List<TModel> parsedModels,List<ValidationError> validationErrors)
        {
            return new MappingResult<TModel>(parsedModels, validationErrors);
        }
    }
}