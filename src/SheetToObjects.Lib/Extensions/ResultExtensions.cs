using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Extensions
{
    internal static class ResultExtensions
    {
        public static Result<TResult, TError> OnValidationSuccess<TResult, TError>(this Result<TResult, TError> result, Func<TResult, TResult> func)
            where TError : class
        {
            if (result.IsSuccess)
                return Result.Ok<TResult, TError>(func(result.Value));

            return Result.Fail<TResult, TError>(result.Error);
        }

        public static Result<TResult, TError> OnValidationFailure<TResult, TError>(this Result<TResult, string> result, Func<string, TError> func) 
            where TError : class
        {
            if (result.IsFailure)
                return Result.Fail<TResult, TError>(func(result.Error));

            return Result.Ok<TResult, TError>(result.Value);
        }
    }
}
