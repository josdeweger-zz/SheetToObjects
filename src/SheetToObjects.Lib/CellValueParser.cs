using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class CellValueParser : IParseCellValue
    {
        public Result<object, ValidationError> ParseValueType<TValue>(Cell cell)
        {
            var type = typeof(TValue);
            
            if (cell.IsNull())
                return Result.Fail<object, ValidationError>(new ValidationError(-1, -1, "Cell is not set"));

            if (cell.Value.IsNull())
                return Result.Fail<object, ValidationError>(new ValidationError(cell.ColumnIndex, cell.RowIndex, $"Value of type {type} is empty."));

            try
            {
                if(cell.Value.ToString().IsNullOrEmpty())
                    return Result.Fail<object, ValidationError>(new ValidationError(cell.ColumnIndex, cell.RowIndex, $"Value of type {type} is empty."));
            }
            catch (Exception)
            {
            }

            try
            {
                var parsedValue = (TValue) Convert.ChangeType(cell.Value, type);
                return Result.Ok<object, ValidationError>(parsedValue);
            }
            catch (Exception)
            {
                return Result.Fail<object, ValidationError>(new ValidationError(cell.ColumnIndex, cell.RowIndex,
                    $"Something went wrong parsing value of type {type}."));
            }
        }

        public Result<object, ValidationError> ParseEnumeration(Cell cell, Type type)
        {
            if (!type.IsEnum)
                return Result.Fail<object, ValidationError>(new ValidationError(cell.ColumnIndex, cell.RowIndex,
                    $"Type {type.Name} is not an Enumeration"));

            if (cell.IsNull())
                return Result.Fail<object, ValidationError>(new ValidationError(cell.ColumnIndex, cell.RowIndex,
                    $"Cell or cell value is not set for column index {cell.ColumnIndex} and row index {cell.RowIndex}"));

            if (cell.Value.IsNull())
                return Result.Fail<object, ValidationError>(new ValidationError(cell.ColumnIndex, cell.RowIndex,
                    $"Cell or cell value is not set for column index {cell.ColumnIndex} and row index {cell.RowIndex}"));

            try
            {
                var enumValue = Enum.Parse(type, cell.Value.ToString(), ignoreCase: true);
                if (enumValue.IsNotNull())
                {
                    return Result.Ok<object, ValidationError>(enumValue);
                }
            }
            catch (Exception)
            {
                return Result.Fail<object, ValidationError>(new ValidationError(cell.ColumnIndex, cell.RowIndex, $"Could not parse value to {type}"));
            }

            return Result.Fail<object, ValidationError>(new ValidationError(cell.ColumnIndex, cell.RowIndex, $"Could not parse value to {type}"));
        }
    }
}