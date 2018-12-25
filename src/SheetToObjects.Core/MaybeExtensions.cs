using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Core
{
    internal static class MaybeExtensions
    {
        public static Maybe<T> OnEmpty<T>(this Maybe<T> maybe, Func<T> resultFunc)
        {
            if(maybe.HasNoValue)
                return Maybe<T>.From(resultFunc());

            return maybe;
        }

        public static Maybe<TResult> OnValue<T, TResult>(this Maybe<T> maybe, Func<T, TResult> resultFunc)
        {
            if(maybe.HasNoValue)
                return Maybe<TResult>.None;

            return Maybe<TResult>.From(resultFunc(maybe.Value));
        }
    }
}
