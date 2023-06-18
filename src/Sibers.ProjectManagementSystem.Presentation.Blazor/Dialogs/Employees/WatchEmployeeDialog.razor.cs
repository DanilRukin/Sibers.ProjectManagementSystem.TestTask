using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Tasks;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees
{
    public partial class WatchEmployeeDialog
    {
        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        /// <summary>
        /// Employee to watch
        /// </summary>
        [Parameter]
        public EmployeeViewModel EmployeeToWatch { get; set; }

        private ICollection<ProjectViewModel> _projects;

        private ICollection<TaskViewModel> _executableTasks;
        private ICollection<TaskViewModel> _createdTasks;

        protected override async Task OnInitializedAsync()
        {
            await LoadProjects();
            await LoadCreatedTasks();
            await LoadExecutableTasks();
        }

        private async Task LoadProjects()
        {
            if (EmployeeToWatch != null)
            {
                if (EmployeeToWatch.ProjectsIds == null 
                    || EmployeeToWatch.ProjectsIds.Count == 0 
                    || EmployeeToWatch.CreatedTasksIds.Count() == 0 
                    || EmployeeToWatch.ExecutableTasksIds.Count() == 0)
                {
                    GetEmployeeByIdQuery query = new GetEmployeeByIdQuery(new EmployeeIncludeOptions(EmployeeToWatch.Id, true, true, true));
                    Result<EmployeeDto> employeeResult = await Mediator.Send(query);
                    if (!employeeResult.IsSuccess)
                    {
                        Snackbar.Add($"Произошла ошибка при обновлении данных сотрудника. Причина: {employeeResult.Errors.AsOneString()}", Severity.Error);
                    }
                    else
                    {
                        EmployeeToWatch = Mapper.Map<EmployeeViewModel>(employeeResult.Value);
                    }
                }
                GetRangeOfProjectsQuery projectsQuery = new GetRangeOfProjectsQuery(EmployeeToWatch.ProjectsIds.Select(id => new ProjectIncludeOptions(id, true, false)));
                var projectsResult = await Mediator.Send(projectsQuery);
                if (!projectsResult.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить проекты сотрудника. Причина: {projectsResult.Errors.AsOneString()}", Severity.Error);
                    _projects = new List<ProjectViewModel>();
                }
                else
                    _projects = Mapper.Map<IEnumerable<ProjectDto>, ICollection<ProjectViewModel>>(projectsResult.Value);
            }
            else
            {
                Snackbar.Add("Не удалось получить сотрудника.", Severity.Warning);
                EmployeeToWatch = new EmployeeViewModel();
            }
        }

        private async Task LoadCreatedTasks()
        {
            if (EmployeeToWatch != null)
            {
                GetRangeOfTasksQuery query = new(EmployeeToWatch.CreatedTasksIds);
                var response = await Mediator.Send(query);
                if (!response.IsSuccess)
                {
                    Snackbar.Add($"Не удалось получить задачи, созданные сотрудником. Причина: {response.Errors.AsOneString()}", Severity.Error);
                }
                else
                {
                    _createdTasks = Mapper.Map<IEnumerable<TaskDto>, ICollection<TaskViewModel>>(response.Value);
                }
            }
            else
            {
                Snackbar.Add("Не удалось получить сотрудника.", Severity.Warning);
                EmployeeToWatch = new EmployeeViewModel();
            }
        }

        private async Task LoadExecutableTasks()
        {
            if (EmployeeToWatch != null)
            {
                GetRangeOfTasksQuery query = new(EmployeeToWatch.ExecutableTasksIds);
                var response = await Mediator.Send(query);
                if (!response.IsSuccess)
                {
                    Snackbar.Add($"Не удалось получить задачи, выполняемые сотрудником. Причина: {response.Errors.AsOneString()}", Severity.Error);
                }
                else
                {
                    _executableTasks = Mapper.Map<IEnumerable<TaskDto>, ICollection<TaskViewModel>>(response.Value);
                }
            }
            else
            {
                Snackbar.Add("Не удалось получить сотрудника.", Severity.Warning);
                EmployeeToWatch = new EmployeeViewModel();
            }
        }

        private void OnTaskWatch(Guid taskId)
        {
            TaskViewModel? task = _createdTasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                task = _executableTasks.FirstOrDefault(t => t.Id == taskId);
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

        private void Cancel() => MudDialog.Cancel();

        private string ShowRole(ProjectViewModel model) => model.ManagerId == EmployeeToWatch.Id ? "Менеджер" : "Сотрудник";
    }
}
