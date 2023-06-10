using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Projects
{
    public partial class WatchProjectDialog
    {
        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        private string DateFormat(DateTime date) => $"{date.Day}.{date.Month}.{date.Year}";

        private ICollection<EmployeeDto> _employees = new List<EmployeeDto>();
        private EmployeeDto _manager = new EmployeeDto();
        private string _managerFullName;

        [Parameter]
        public ProjectDto Project { get; set; }

        private DateTime? _projectStartDate;
        private DateTime? _projectEndDate;


        private void OnEmployeeWatch(int employeeId)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(WatchEmployeeDialog.EmployeeToWatch), new EmployeeDto { Id = employeeId });
            var dialog = DialogService.Show<WatchEmployeeDialog>("Просмотр сотрудника", parameters);
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadEmployees();
                _managerFullName = $"{_manager.FirstName} {_manager.LastName} {_manager.Patronymic}";
            }
            catch (Exception e)
            {
                Snackbar.Add($"Что-то пошло не так. Причина: {e.ReadErrors()}");
            }
        }

        void Ok() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();

        private async Task LoadEmployees()
        {
            GetProjectByIdQuery projectQuery = new GetProjectByIdQuery(new ProjectIncludeOptions(Project.Id, true));
            Result<ProjectDto> projectResult = await Mediator.Send(projectQuery);
            if (!projectResult.IsSuccess)
            {
                Snackbar.Add($"Ошибка при обновлении данных проекта. Причина: {projectResult.Errors.AsOneString()}", Severity.Error);
            }
            else
            {
                Project = projectResult.Value;
                Project.EmployeesIds.Remove(Project.ManagerId);
                GetRangeOfEmployeesQuery employeesQuery = new GetRangeOfEmployeesQuery(Project.EmployeesIds.Select(id => new EmployeeIncludeOptions(id, false)));
                Result<IEnumerable<EmployeeDto>> result = await Mediator.Send(employeesQuery);
                if (!result.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить данные сотрудников. Причина: {result.Errors.AsOneString()}", Severity.Error);
                    _employees = new List<EmployeeDto>();
                }
                else
                    _employees = result.Value.ToList();
                GetEmployeeByIdQuery managerQuery = new GetEmployeeByIdQuery(new EmployeeIncludeOptions(Project.ManagerId, false));
                var managerResult = await Mediator.Send(managerQuery);
                if (!managerResult.IsSuccess)
                    _manager = new EmployeeDto();
                else
                    _manager = managerResult.Value;
            }
        }
    }
}
