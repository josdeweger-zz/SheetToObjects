using SheetToObjects.Lib.Attributes;
using SheetToObjects.Lib.Attributes.MappingType;
using SheetToObjects.Lib.Attributes.Rules;

namespace SheetToObjects.Specs.TestModels
{
    [SheetToObjectConfig(sheetHasHeaders:true)]
    public class AttributeTestModel
    {

        [MappingByIndex(3)]
        [IsRequired]
        [Regex(@"/^[a-z]+[0-9_\/\s,.-]+$", true)]
        public string StringProperty { get; set; }
    }
}