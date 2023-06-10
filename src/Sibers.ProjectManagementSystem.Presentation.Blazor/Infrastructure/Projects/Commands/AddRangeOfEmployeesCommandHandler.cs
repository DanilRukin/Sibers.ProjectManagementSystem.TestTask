using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class AddRangeOfEmployeesCommandHandler : IRequestHandler<AddRangeOfEmployeesCommand, Result>
    {
        private HttpClient _client;
        public AddRangeOfEmployeesCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result> Handle(AddRangeOfEmployeesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.AddRangeOfEmployees(request.ProjectId);
                var message = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(route, UriKind.Relative),
                    Content = JsonContent.Create(request.EmployeesIds)
                });
                if (message == null)
                    return Result.Error("Response message is null");
                if (!message.IsSuccessStatusCode)
                {
                    IEnumerable<string>? errors = await message.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    if (errors != null)
                        return Result.Error(errors.ToArray());
                    else
                        return Result.Error("Response message is not success, but there were not error messages.");
                }
                else
                    return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
