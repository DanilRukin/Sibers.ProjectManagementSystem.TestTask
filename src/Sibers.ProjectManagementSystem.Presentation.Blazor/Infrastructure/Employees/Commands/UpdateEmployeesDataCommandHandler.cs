using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands
{
    public class UpdateEmployeesDataCommandHandler : IRequestHandler<UpdateEmployeesDataCommand, Result<EmployeeDto>>
    {
        private HttpClient _client;
        public UpdateEmployeesDataCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<EmployeeDto>> Handle(UpdateEmployeesDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Put.Update();
                var response = await _client.PutAsJsonAsync<EmployeeDto>(route, request.Employee);
                if (response == null)
                    return Result<EmployeeDto>.Error("При обновлении данных не был получен ответ с сервера.");
                if (response.IsSuccessStatusCode)
                {
                    EmployeeDto? employee = await response.Content.ReadFromJsonAsync<EmployeeDto>();
                    if (employee == null)
                        return Result<EmployeeDto>.Error("В ответе не содержатся обновленные данные сотрудника.");
                    return Result<EmployeeDto>.Success(employee);
                }
                else
                {
                    IEnumerable<string>? errors = await response.Content
                        .ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: cancellationToken);
                    return Result<EmployeeDto>.Error(errors?.ToArray());
                }
            }
            catch (Exception e)
            {
                return Result<EmployeeDto>.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
