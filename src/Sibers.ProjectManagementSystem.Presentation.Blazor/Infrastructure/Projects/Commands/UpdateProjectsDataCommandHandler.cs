using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class UpdateProjectsDataCommandHandler : IRequestHandler<UpdateProjectsDataCommand, Result<ProjectDto>>
    {
        private HttpClient _client;
        public UpdateProjectsDataCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<ProjectDto>> Handle(UpdateProjectsDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.Update();
                var result = await _client.PutAsJsonAsync(route, request.Project, cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    ProjectDto? project = await result.Content
                        .ReadFromJsonAsync<ProjectDto>(cancellationToken: cancellationToken);
                    if (project == null)
                        return Result<ProjectDto>.Error("Project was null after updating.");
                    return Result<ProjectDto>.Success(project);
                }
                IEnumerable<string>? errors = await result.Content
                    .ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: cancellationToken);
                return Result<ProjectDto>.Error(errors?.ToArray());
            }
            catch (Exception e)
            {
                return Result<ProjectDto>.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
