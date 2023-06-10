using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<IEnumerable<ProjectDto>>>
    {
        private HttpClient _httpClient;
        public GetAllProjectsQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _httpClient = factory.CreateClient();
        }
        public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.All(request.IncludeEmployees);
                var projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDto>>(route, cancellationToken);
                if (projects == null)
                    return Result<IEnumerable<ProjectDto>>.Error("Не был получен ответ с сервера.");
                else
                    return Result.Success(projects);
            }
            catch (Exception e)
            {
                return Result<IEnumerable<ProjectDto>>.Error($"Что-то пошло не так. Причина: {e.ReadErrors()}");
            }
        }
    }
}
