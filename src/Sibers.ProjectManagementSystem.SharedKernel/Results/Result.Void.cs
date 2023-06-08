using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.Results
{
    public class Result : Result<Result>
    {
        public Result() : base() { }
        protected internal Result(ResultStatus status) : base(status) { }

        public static Result Success()
        {
            return new Result();
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value);
        }

        public static new Result Error(params string[] errorMessages)
        {
            return new Result(ResultStatus.Error) { Errors = errorMessages };
        }

        public static new Result NotFound()
        {
            return new Result(ResultStatus.NotFound);
        }

        public static new Result NotFound(params string[] errorMessages)
        {
            return new Result(ResultStatus.NotFound) { Errors = errorMessages };
        }
    }
}
