using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application
{
    internal class ExceptionHandler
    {
        internal static Result<T> Handle<T>(Exception e)
        {
            Exception ex = e;
            List<string> errors = new List<string>();
            while (ex != null)
            {
                errors.Add(ex.Message);
                ex = ex.InnerException;
            }
            return Result<T>.Error(errors.ToArray());
        }

        internal static Result Handle(Exception e)
        {
            Exception ex = e;
            List<string> errors = new List<string>();
            while (ex != null)
            {
                errors.Add(ex.Message);
                ex = ex.InnerException;
            }
            return Result.Error(errors.ToArray());
        }
    }
}
