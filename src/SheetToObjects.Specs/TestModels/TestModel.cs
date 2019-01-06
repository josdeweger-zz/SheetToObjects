using System;
using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes;
using SheetToObjects.Lib.AttributesConfiguration.RuleAttributes;

namespace SheetToObjects.Specs.TestModels
{
    [SheetToObjectConfig]
    public class TestModel
    {
        public string StringProperty { get; set; }

        public int IntProperty { get; set; }

        public int? NullableIntProperty { get; set; }

        public double DoubleProperty { get; set; }

        public double? NullableDoubleProperty { get; set; }

        public bool BoolProperty { get; set; }

        public bool? NullableBoolProperty { get; set; }

        [IgnorePropertyMapping]
        public EnumModel EnumProperty { get; set; }

        public DateTime DateTimeProperty { get; set; }

        [IgnorePropertyMapping]
        public string PropertyWithBody => string.Empty;

        public decimal DecimalProperty { get; set; }

        [Regex(pattern: @"^$|^[^@\s]+@[^@\s]+$`", ignoreCase: true)]
        public string StringRegexProperty { get; set; }
    }
}