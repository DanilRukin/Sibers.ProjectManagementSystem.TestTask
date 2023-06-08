using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.Results
{
    public interface IResult
    {
        bool IsSuccess { get; }
        ResultStatus ResultStatus { get; }
        Type ValueType { get; }
        object GetValue();
        IEnumerable<string> Errors { get; }
    }
}
