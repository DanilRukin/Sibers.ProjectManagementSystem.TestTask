using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks.Commands
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result>
    {
        private HttpClient _client;
        public DeleteTaskCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Delete.ById(request.TaskId);
                var response = await _client.DeleteFromJsonAsync<Result>(route, cancellationToken);
                if (response == null)
                    return Result.Error("Не был получен ответ с сервера.");
                else
                    return response;
            }
            catch (Exception e)
            {
                return Result.Error(e.ReadErrors());
            }
        }
    }
}
