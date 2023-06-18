using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Tasks;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;
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

        [Inject]
        public IMapper Mapper { get; set; }

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        private string DateFormat(DateTime? date) => date == null ? "" : $"{date.Value.Day}.{date.Value.Month}.{date.Value.Year}";

        private ICollection<EmployeeViewModel> _employees = new List<EmployeeViewModel>();
        private ICollection<TaskViewModel> _tasks = new List<TaskViewModel>();
        private EmployeeViewModel _manager = new EmployeeViewModel();
        private string _managerFullName;

        [Parameter]
        public ProjectViewModel Project { get; set; }

        private void OnEmployeeWatch(int employeeId)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(WatchEmployeeDialog.EmployeeToWatch), new EmployeeViewModel { Id = employeeId });
            var dialog = DialogService.Show<WatchEmployeeDialog>("Просмотр сотрудника", parameters);
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadProject();
                await LoadEmployees();
                
                await LoadTasks();
            }
            catch (Exception e)
            {
                Snackbar.Add($"Что-то пошло не так. Причина: {e.ReadErrors()}");
            }
        }

        private void Ok() => MudDialog.Close(DialogResult.Ok(true));
        private void Cancel() => MudDialog.Cancel();

        private async Task LoadProject()
        {
            GetProjectByIdQuery projectQuery = new GetProjectByIdQuery(new ProjectIncludeOptions(Project.Id, true, true));
            Result<ProjectDto> projectResult = await Mediator.Send(projectQuery);
            if (!projectResult.IsSuccess)
            {
                Snackbar.Add($"Ошибка при обновлении данных проекта. Причина: {projectResult.Errors.AsOneString()}", Severity.Error);
            }
            else
            {
                Project = Mapper.Map<ProjectViewModel>(projectResult.Value);
            }
        }

        private async Task LoadEmployees()
        {
            if (Project == null)
            {
                Snackbar.Add("Не удалось загрузить данные сотрудников, т.к. проекта нет", Severity.Error);
            }
            else
            {
                Project.EmployeesIds.Remove(Project.ManagerId);
                GetRangeOfEmployeesQuery employeesQuery = new GetRangeOfEmployeesQuery(Project.EmployeesIds.Select(id => new EmployeeIncludeOptions(id, false, false, false)));
                Result<IEnumerable<EmployeeDto>> result = await Mediator.Send(employeesQuery);
                if (!result.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить данные сотрудников. Причина: {result.Errors.AsOneString()}", Severity.Error);
                    _employees = new List<EmployeeViewModel>();
                }
                else
                    _employees = Mapper.Map<IEnumerable<EmployeeDto>, ICollection<EmployeeViewModel>>(result.Value);
                GetEmployeeByIdQuery managerQuery = new GetEmployeeByIdQuery(new EmployeeIncludeOptions(Project.ManagerId, false, false, false));
                var managerResult = await Mediator.Send(managerQuery);
                if (!managerResult.IsSuccess)
                    _manager = new EmployeeViewModel();
                else
                    _manager = Mapper.Map<EmployeeViewModel>(managerResult.Value);
                _managerFullName = $"{_manager.FirstName} {_manager.LastName} {_manager.Patronymic}";
            }
        }

        private async Task LoadTasks()
        {
            if (Project == null)
            {
                Snackbar.Add("Не удалось загрузить данные задач, т.к. проекта нет", Severity.Error);
            }
            else
            {
                GetRangeOfTasksQuery query = new(Project.TasksIds);
                var response = await Mediator.Send(query);
                if (!response.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить данные задач. Причина: {response.Errors.AsOneString()}", Severity.Error);
                    _tasks = new List<TaskViewModel>();
                }
                else
                    _tasks = Mapper.Map<IEnumerable<TaskDto>, ICollection<TaskViewModel>>(response.Value);
            }
        }

        private void OnTaskWatch(Guid taskId)
        {
            TaskViewModel? task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                Snackbar.Add("Невозможно просмотреть задачу, т.к. ее нет", Severity.Info);
            else
            {
                DialogParameters parameters = new DialogParameters()
                {
                    { nameof(WatchTaskDialog.TaskToWatch), task }
                };
                DialogService.Show<WatchTaskDialog>("Просмотр задачи", parameters);
            }
        }
    }
}
