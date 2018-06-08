using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingType;

namespace SheetToObjects.Specs.TestModels
{
    [SheetToObjectConfig()]
    public class LetterAttributeTestModel
    {

        [MappingByLetter("C")]
       
        public string StringProperty { get; set; }
       
    }
}