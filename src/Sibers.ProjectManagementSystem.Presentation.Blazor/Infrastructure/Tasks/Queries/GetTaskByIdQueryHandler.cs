using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks.Queries
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
    {
        private HttpClient _client;
        public GetTaskByIdQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }

        public async Task<Result<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.ById(request.TaskId);
                HttpRequestMessage message = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(route, UriKind.Relative)
                };
                HttpResponseMessage response = await _client.SendAsync(message);
                TaskDto? result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<TaskDto>(cancellationToken: cancellationToken);
                if (result == null)
                    return Result<TaskDto>.NotFound("Задача не была найдена");
                else
                    return Result.Success(result);
            }
            catch (Exception e)
            {
                return Result<TaskDto>.Error(e.ReadErrors());
            }
        }
    }
}
