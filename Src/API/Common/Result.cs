using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace API.Common
{
    /// <summary>
    /// This is the object that will be returned from every API endpoint. 
    /// </summary>
    public class Result
    {
        public string Message { get; }
        public bool Success { get; }
        public bool Error => !Success;

        protected Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static Result Fail(string message) => new Result(false, message);
        public static Result<T> Fail<T>(string message) => new Result<T>(default(T), false, message);
        public static Result<T> Fail<T>(T value, string message) => new Result<T>(value, false, message);
        public static Result Ok() => new Result(true, string.Empty);
        public static Result<T> Ok<T>(T value) => new Result<T>(value, true, string.Empty);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected internal Result(T value, bool success, string message) 
            : base(success, message)
        {
            Value = value;
        }
    }
}
