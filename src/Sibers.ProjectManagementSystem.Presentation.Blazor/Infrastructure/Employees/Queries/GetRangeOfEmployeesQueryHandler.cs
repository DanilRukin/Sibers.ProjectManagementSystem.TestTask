using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries
{
    public class GetRangeOfEmployeesQueryHandler : IRequestHandler<GetRangeOfEmployeesQuery, Result<IEnumerable<EmployeeDto>>>
    {
        private HttpClient _client;
        public GetRangeOfEmployeesQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<IEnumerable<EmployeeDto>>> Handle(GetRangeOfEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Options.Count() <= 0)
                    return Result.Success(new List<EmployeeDto>().AsEnumerable());
                string route = ApiHelper.Get.Range();
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(route, UriKind.Relative),
                    Content = JsonContent.Create(request.Options),
                });
                var result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<IEnumerable<EmployeeDto>>(cancellationToken: cancellationToken);
                if (result == null)
                    return Result<IEnumerable<EmployeeDto>>.Error("Не был получен ответ с сервера.");
                return Result<IEnumerable<EmployeeDto>>.Success(result);
            }
            catch (Exception e)
            {
                return Result<IEnumerable<EmployeeDto>>.Error(e.ReadErrors());
            }
        }
    }
}
