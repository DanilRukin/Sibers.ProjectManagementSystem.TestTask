using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<TaskDto>>
    {
        private HttpClient _client;
        public CreateTaskCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int projectId = request.Task.ProjectId;
                int authorId = request.Task.AuthorEmployeeId;
                string route = ApiHelper.Post.CreateTask(projectId, authorId);
                HttpRequestMessage message = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(route, UriKind.Relative),
                    Content = JsonContent.Create(request.Task),
                };
                HttpResponseMessage response = await _client.SendAsync(message);
                TaskDto? createdTask = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<TaskDto>(cancellationToken: cancellationToken);
                if (createdTask == null)
                    return Result<TaskDto>.Error("Задача не была создана");
                else
                    return Result.Success(createdTask);
            }
            catch (Exception e)
            {
                return Result<TaskDto>.Error(e.ReadErrors());
            }
        }
    }
}
