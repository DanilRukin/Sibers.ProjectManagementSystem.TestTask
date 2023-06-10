using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class DemoteManagerToEmployeeCommandHandler : IRequestHandler<DemoteManagerToEmployeeCommand, Result>
    {
        private HttpClient _client;
        public DemoteManagerToEmployeeCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(DemoteManagerToEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.DemoteManager(request.ProjectId, request.Reason);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(route, UriKind.Relative)
                });
                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                        return Result.Success();
                    else
                    {
                        IEnumerable<string>? errors = await response.Content
                            .ReadFromJsonAsync<IEnumerable<string>>();
                        if (errors != null)
                            return Result.Error(errors.ToArray());
                        else
                            return Result.Error();
                    }
                }
                else
                {
                    return Result.Error("Result was null!");
                }
            }
            catch (Exception e)
            {
                return Result.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
