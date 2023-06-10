using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class RemoveRangeOfEmployeesCommandHandler : IRequestHandler<RemoveRangeOfEmployeesCommand, Result>
    {
        private HttpClient _client;
        public RemoveRangeOfEmployeesCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(RemoveRangeOfEmployeesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.RemoveRangeOfEmployees(request.ProjectId);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(route, UriKind.Relative),
                    Content = JsonContent.Create(request.EmployeesIds)
                });
                if (response == null)
                    return Result.Error("Response was null!");
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
