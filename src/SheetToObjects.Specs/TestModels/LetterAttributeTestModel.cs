using SheetToObjects.Lib.Attributes;
using SheetToObjects.Lib.Attributes.MappingType;

namespace SheetToObjects.Specs.TestModels
{
    [SheetToObjectConfig()]
    public class LetterAttributeTestModel
    {

        [MappingByLetter("C")]
       
        public string StringProperty { get; set; }
       
    }
}