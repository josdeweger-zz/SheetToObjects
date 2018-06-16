using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    public class CustomRule<TValue> : ICustomRule
    {
        private readonly Func<TValue, bool> _customRuleFunc;
        private readonly string _validationMessage;

        public CustomRule(Func<TValue, bool> customRuleFunc, string validationMessage)
        {
            _customRuleFunc = customRuleFunc;
            _validationMessage = validationMessage;
        }

        public Result<object, IValidationError> Validate(int columnIndex, int rowIndex, string columnName,
            string propertyName, object value)
        {
            try
            {
                var typedValue = (TValue)value;

                if (_customRuleFunc(typedValue))
                {
                    return Result.Ok<object, IValidationError>(value);
                }

                var validationError = new RuleValidationError(columnIndex, rowIndex, _validationMessage, columnName,
                    typedValue.ToString(), propertyName);

                return Result.Fail<object, IValidationError>(validationError);
            }
            catch (Exception)
            {
                var validationError =
                    RuleValidationError.CouldNotValidateValue(columnIndex, rowIndex, columnName, propertyName);

                return Result.Fail<object, IValidationError>(validationError);
            }
        }
    }
}