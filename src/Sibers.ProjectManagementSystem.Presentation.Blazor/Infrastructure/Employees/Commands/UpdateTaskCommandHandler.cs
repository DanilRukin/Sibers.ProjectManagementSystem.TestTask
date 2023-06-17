using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<TaskDto>>
    {
        private HttpClient _client;
        public UpdateTaskCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<TaskDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.UpdateTask();
                HttpRequestMessage message = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(route, UriKind.Relative),
                    Content = JsonContent.Create(request.UpdatedTask)
                };
                HttpResponseMessage response = await _client.SendAsync(message);
                var updatedTask = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<TaskDto>(cancellationToken: cancellationToken);
                if (updatedTask == null)
                    return Result<TaskDto>.Error("Задача не была обновлена");
                else
                    return Result.Success(updatedTask);
            }
            catch (Exception e)
            {
                return Result<TaskDto>.Error(e.ReadErrors());
            }
        }
    }
}
