using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes;
using SheetToObjects.Lib.AttributesConfiguration.RuleAttributes;

namespace SheetToObjects.Specs.TestModels
{
    [SheetToObjectConfig()]
    public class ColumnNameAttributeTestModel
    {
        [MappingByHeader("StringColumn")]
        [IsRequired(true)]
        public string StringProperty { get; set; }
    }
}