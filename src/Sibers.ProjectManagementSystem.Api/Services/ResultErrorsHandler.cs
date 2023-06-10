using Microsoft.AspNetCore.Mvc;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Api.Services
{
    internal static class ResultErrorsHandler
    {
        internal static ActionResult<T> Handle<T>(Result<T> result)
        {
            if (result.ResultStatus == ResultStatus.NotFound)
                return new NotFoundObjectResult(result.Errors);
            else if (result.ResultStatus == ResultStatus.Error)
                return new BadRequestObjectResult(result.Errors);
            else
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        internal static ActionResult Handle(Result result)
        {
            if (result.ResultStatus == ResultStatus.NotFound)
                return new NotFoundObjectResult(result.Errors);
            else if (result.ResultStatus == ResultStatus.Error)
                return new BadRequestObjectResult(result.Errors);
            else
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
