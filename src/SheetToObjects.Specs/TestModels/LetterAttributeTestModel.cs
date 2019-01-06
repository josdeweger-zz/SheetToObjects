using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes;

namespace SheetToObjects.Specs.TestModels
{
    [SheetToObjectAttributeConfig()]
    public class LetterAttributeTestModel
    {

        [MappingByLetter("C")]
       
        public string StringProperty { get; set; }
       
    }
}