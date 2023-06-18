using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class DeleteTaskFromProjectCommandHandler : IRequestHandler<DeleteTaskFromProjectCommand, Result>
    {
        private HttpClient _client;
        public DeleteTaskFromProjectCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(DeleteTaskFromProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.RemoveTaskFromProject(request.ProjectId, request.EmployeeId, request.TaskId);
                HttpRequestMessage message = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(route, UriKind.Relative),
                };
                var response = await _client.SendAsync(message, cancellationToken);
                if (response.IsSuccessStatusCode)
                    return Result.Success();
                else
                {
                    IEnumerable<string>? errors = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    if (errors != null)
                        return Result.Error(errors.ToArray());
                    else
                        return Result.Error("Неопределенная ошибка");
                }    
            }
            catch (Exception e)
            {
                return Result.Error(e.ReadErrors());
            }
        }
    }
}
