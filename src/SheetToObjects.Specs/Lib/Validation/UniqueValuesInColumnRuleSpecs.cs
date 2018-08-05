using System.Collections.Generic;
using FluentAssertions;
using SheetToObjects.Lib.Validation;
using Xunit;

namespace SheetToObjects.Specs.Lib.Validation
{
    public class UniqueValuesInColumnRuleSpecs
    {
        [Fact]
        public void GivenValidatingUniqueInColumnValue_WhenValueOccursOnce_ValidationSucceeds()
        {
            var values = new List<object> { "FirstValue", "SecondValue" };
            
            var result = new UniqueValuesInColumnRule().Validate(0, "MyColumn", values);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void GivenValidatingUniqueInColumnValue_WhenValueOccursTwice_ValidationFails()
        {
            var values = new List<object> { "SameValue", "SameValue" };

            var result = new UniqueValuesInColumnRule().Validate(0, "MyColumn", values);

            result.IsSuccess.Should().BeFalse();
        }
    }
}