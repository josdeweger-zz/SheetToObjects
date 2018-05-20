using SheetToObjects.Lib.Attributes;
using SheetToObjects.Lib.Attributes.MappingType;
using SheetToObjects.Lib.Attributes.Rules;

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