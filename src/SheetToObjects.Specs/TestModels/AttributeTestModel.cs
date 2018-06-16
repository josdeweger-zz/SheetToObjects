using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingType;
using SheetToObjects.Lib.AttributesConfiguration.RuleAttributes;

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