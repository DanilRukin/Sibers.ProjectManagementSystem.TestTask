using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, Result>
    {
        private HttpClient _client;
        public AddEmployeeCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.AddEmployee(request.ProjectId, request.EmployeeId);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(route, UriKind.Relative)
                });
                if (response == null)
                    return Result.Error("Result was null!");
                if (response.IsSuccessStatusCode)
                    return Result.Success();
                else
                {
                    IEnumerable<string>? errors = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    if (errors != null && errors.Any())
                        return Result.Error(errors.ToArray());
                    else
                        return Result.Error("Undefined error!");
                }
            }
            catch (Exception e)
            {
                return Result.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
