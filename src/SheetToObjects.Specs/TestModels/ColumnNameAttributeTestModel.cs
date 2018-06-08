using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingType;
using SheetToObjects.Lib.AttributesConfiguration.Rules;

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