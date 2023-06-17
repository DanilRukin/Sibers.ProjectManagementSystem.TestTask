using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks.Queries
{
    public class GetRangeOfTasksQueryHandler : IRequestHandler<GetRangeOfTasksQuery, Result<IEnumerable<TaskDto>>>
    {
        private HttpClient _client;

        public GetRangeOfTasksQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }

        public async Task<Result<IEnumerable<TaskDto>>> Handle(GetRangeOfTasksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.TasksIds.Count() == 0)
                    return Result.Success(new List<TaskDto>().AsEnumerable());
                string route = ApiHelper.Get.Range(request.TasksIds, "ids");
                HttpRequestMessage message = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(route, UriKind.Relative)
                };
                HttpResponseMessage response = await _client.SendAsync(message);
                if (response == null)
                    return Result<IEnumerable<TaskDto>>.NotFound("Задачи не были найдены");
                IEnumerable<TaskDto>? tasks = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<IEnumerable<TaskDto>>(cancellationToken: cancellationToken);
                if (tasks == null)
                    return Result<IEnumerable<TaskDto>>.NotFound("Задачи не были найдены");
                else
                    return Result.Success(tasks);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TaskDto>>.Error(ex.ReadErrors());
            }
        }
    }
}
