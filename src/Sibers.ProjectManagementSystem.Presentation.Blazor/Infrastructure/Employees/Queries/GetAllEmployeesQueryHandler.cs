using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, Result<IEnumerable<EmployeeDto>>>
    {
        private HttpClient _client;
        public GetAllEmployeesQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<IEnumerable<EmployeeDto>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.All(request.IncludeProjects);
                var result = await _client
                    .GetFromJsonAsync<IEnumerable<EmployeeDto>>(route, cancellationToken)
                    .ConfigureAwait(false);
                if (result == null)
                    return Result<IEnumerable<EmployeeDto>>.Error("Не удалось получить ответ с сервера.");
                else
                    return Result<IEnumerable<EmployeeDto>>.Success(result);
            }
            catch (Exception e)
            {
                return Result<IEnumerable<EmployeeDto>>.Error($"Что-то пошло не так. Причина: {e.ReadErrors()}");
            }
        }
    }
}
