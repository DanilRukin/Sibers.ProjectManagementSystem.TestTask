using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.Results
{
    public class Result<T> : IResult
    {
        protected Result() { }
        public Result(T value)
        {
            Value = value;
            if (Value != null)
            {
                ValueType = Value.GetType();
            }
        }
        public bool IsSuccess => ResultStatus == ResultStatus.Ok;

        public ResultStatus ResultStatus { get; protected set; } = ResultStatus.Ok;

        public Type ValueType { get; protected set; }

        public IEnumerable<string> Errors { get; protected set; } = new List<string>();

        public object GetValue()
        {
            return Value;
        }
        public T Value { get; }

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => new Result<T>(value);

        public static Result<T> Success(T value) => new Result<T>(value);

        protected Result(ResultStatus resultStatus)
        {
            ResultStatus = resultStatus;
        }

        public static Result<T> Error(params string[] errors) => new Result<T>(ResultStatus.Error) { Errors = errors };

        public static Result<T> NotFound()
        {
            return new Result<T>(ResultStatus.NotFound);
        }

        public static Result<T> NotFound(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.NotFound) { Errors = errorMessages };
        }
    }
}
