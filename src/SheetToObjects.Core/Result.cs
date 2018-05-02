using System;
using System.Diagnostics;

namespace SheetToObjects.Core
{
    public class Result
    {
        public Type Type { get; }
        public object Value { get; }
        public bool HasValue => Value.IsNotNull();
        public bool IsValid { get; }
        public string Message { get; }

        [DebuggerStepThrough]
        private Result()
        {

        }

        [DebuggerStepThrough]
        private Result(object value, Type type)
        {
            Type = type;
            Value = value;
            IsValid = true;
        }

        [DebuggerStepThrough]
        private Result(string message)
        {
            IsValid = false;
            Message = message;
        }

        [DebuggerStepThrough]
        public static Result Ok<T>(T value)
        {
            return new Result(value, typeof(T));
        }

        [DebuggerStepThrough]
        public static Result Ok()
        {
            return new Result();
        }

        [DebuggerStepThrough]
        public static Result Fail(string message)
        {
            return new Result(message);
        }
    }
}