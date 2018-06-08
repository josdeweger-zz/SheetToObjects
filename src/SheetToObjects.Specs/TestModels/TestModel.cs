using System;
using SheetToObjects.Lib.AttributesConfiguration.MappingType;

namespace SheetToObjects.Specs.TestModels
{
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
    }
}