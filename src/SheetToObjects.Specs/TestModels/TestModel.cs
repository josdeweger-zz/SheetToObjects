using SheetToObjects.Lib.Attributes.MappingType;

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

        [IgnorePropertyMapping]
        public string PropertyWithBody
        {
            get { return string.Empty; }
        }
    }
}