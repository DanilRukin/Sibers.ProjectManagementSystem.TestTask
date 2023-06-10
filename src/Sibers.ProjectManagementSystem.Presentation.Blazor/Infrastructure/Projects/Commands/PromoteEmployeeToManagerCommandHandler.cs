using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class PromoteEmployeeToManagerCommandHandler : IRequestHandler<PromoteEmployeeToManagerCommand, Result>
    {
        private HttpClient _client;
        public PromoteEmployeeToManagerCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(PromoteEmployeeToManagerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.PromoteEmployeeToManager(request.ProjectId, request.EmployeeId);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(route, UriKind.Relative)
                });
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return Result.Success();
                else
                {
                    IEnumerable<string>? errors = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    if (errors != null)
                        return Result.Error(errors.ToArray());
                    else
                        return Result.Error($"Ошибка в {nameof(PromoteEmployeeToManagerCommandHandler)}");
                }

            }
            catch (Exception e)
            {
                return Result.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
