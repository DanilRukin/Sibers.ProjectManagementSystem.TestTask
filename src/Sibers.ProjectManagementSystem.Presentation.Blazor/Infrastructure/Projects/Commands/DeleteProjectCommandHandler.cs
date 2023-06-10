using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
    {
        private HttpClient _client;
        public DeleteProjectCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Delete.ById(request.ProjectId);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(route, UriKind.Relative),
                });
                if (response == null)
                    return Result.Error("Во время удаления проекта не был получен ответ с сервера.");
                if (response.IsSuccessStatusCode)
                    return Result.Success();
                else
                {
                    IEnumerable<string>? errors = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    if (errors == null)
                        return Result.Error("Undefined error");
                    else
                        return Result.Error(errors.ToArray());
                }
            }
            catch (Exception e)
            {
                return Result.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
