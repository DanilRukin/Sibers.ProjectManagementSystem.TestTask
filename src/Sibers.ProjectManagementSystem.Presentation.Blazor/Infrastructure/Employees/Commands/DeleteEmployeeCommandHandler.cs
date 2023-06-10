using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result>
    {
        private HttpClient _client;
        public DeleteEmployeeCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Delete.ById(request.EmployeeId);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(route, UriKind.Relative)
                });
                if (response == null)
                    return Result.Error("Во время удаления сотрудника не был получен ответ с сервера.");
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
