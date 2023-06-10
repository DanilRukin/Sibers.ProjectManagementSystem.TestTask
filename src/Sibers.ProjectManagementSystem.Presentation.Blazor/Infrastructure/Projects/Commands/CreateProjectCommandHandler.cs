using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
    {
        private HttpClient _client;
        public CreateProjectCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Post.Create();
                var response = await _client.PostAsJsonAsync(route, request.ProjectDto, cancellationToken);
                var result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<ProjectDto>();
                if (result != null)
                    return Result.Success(result);
                else
                    return Result<ProjectDto>.Error("Проект не был создан");
            }
            catch (Exception e)
            {
                return Result<ProjectDto>.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
