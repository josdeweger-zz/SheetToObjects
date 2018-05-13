using SheetToObjects.Lib.Attributes.MappingType;

namespace SheetToObjects.Specs.TestModels
{
    public class LetterAttributeTestModel
    {

        [MappingByLetter("C")]
       
        public string StringProperty { get; set; }
       
    }
}