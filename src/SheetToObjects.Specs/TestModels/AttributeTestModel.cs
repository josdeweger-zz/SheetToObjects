using System;
using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes;
using SheetToObjects.Lib.AttributesConfiguration.RuleAttributes;

namespace SheetToObjects.Specs.TestModels
{
    [SheetToObjectConfig(sheetHasHeaders:true)]
    public class AttributeTestModel
    {

        [MappingByIndex(3)]
        [IsRequired]
        [DefaultValue("Default String Value")]
        [Regex(@"/^[a-z]+[0-9_\/\s,.-]+$", true)]
        public string StringProperty { get; set; }

        [MappingByIndex(2)]
        [Format("yyyy-MM-dd")]
        [IsRequired]
        public DateTime DateTimeProperty { get; set; }


        [MappingByHeader("RequiredInHeaderProperty")]
        [RequiredInHeaderRow]
        public string RequiredInHeaderProperty { get; set; }
    }
}