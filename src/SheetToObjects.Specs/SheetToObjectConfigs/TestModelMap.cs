using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Specs.TestModels;

namespace SheetToObjects.Specs.SheetToObjectConfigs
{
    public class TestModelMap : SheetToObjectConfig<TestModel>
    {
        public TestModelMap()
        {
            CreateMap(x => x
                .HasHeaders()
                .MapColumn(c => c.WithHeader("StringProperty").MapTo(m => m.StringProperty))
                .MapColumn(c => c.WithHeader("IntProperty").MapTo(m => m.IntProperty))
                .MapColumn(c => c.WithHeader("NullableIntProperty").MapTo(m => m.NullableIntProperty))
                .MapColumn(c => c.WithHeader("DoubleProperty").MapTo(m => m.DoubleProperty))
                .MapColumn(c => c.WithHeader("NullableDoubleProperty").MapTo(m => m.NullableDoubleProperty))
                .MapColumn(c => c.WithHeader("BoolProperty").MapTo(m => m.BoolProperty))
                .MapColumn(c => c.WithHeader("NullableBoolProperty").MapTo(m => m.NullableBoolProperty))
                .MapColumn(c => c.WithHeader("DateTimeProperty").UsingFormat("D-M-YYYY").MapTo(m => m.DateTimeProperty))
                .MapColumn(c => c.WithHeader("DecimalProperty").MapTo(m => m.DecimalProperty))
                .MapColumn(c => c.WithHeader("StringRegexProperty").Matches(@"^$|^[^@\s]+@[^@\s]+$`").MapTo(m => m.StringRegexProperty))
            );
        }
    }
}