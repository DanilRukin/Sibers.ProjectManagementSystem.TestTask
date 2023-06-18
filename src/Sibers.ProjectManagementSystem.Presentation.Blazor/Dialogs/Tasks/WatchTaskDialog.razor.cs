using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Projects;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Tasks
{
    public partial class WatchTaskDialog
    {
        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        /// <summary>
        /// Задача для просмотра. Должна быть установлена извне.
        /// </summary>
        [Parameter]
        public TaskViewModel TaskToWatch { get; set; }

        private ProjectViewModel _projectOfTask = new ProjectViewModel();
        private EmployeeViewModel _author = new EmployeeViewModel();
        private EmployeeViewModel _contractor = new EmployeeViewModel();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            GetProjectByIdQuery projectQuery = new(new ProjectIncludeOptions(TaskToWatch.ProjectId, true, false));
            var projectResponse = await Mediator.Send(projectQuery);
            if (!projectResponse.IsSuccess)
            {
                Snackbar.Add($"Не удалось загрузить данные проекта. Причина: {projectResponse.Errors.AsOneString()}", Severity.Info);
            }
            else
            {
                _projectOfTask = Mapper.Map<ProjectViewModel>(projectResponse.Value);
            }
            GetEmployeeByIdQuery authorQuery = new(new(TaskToWatch.AuthorEmployeeId, false, false, false));
            var authorResponse = await Mediator.Send(authorQuery);
            if (!authorResponse.IsSuccess)
            {
                Snackbar.Add($"Не удалось загрузить данные автора задачи. Причина: {authorResponse.Errors.AsOneString()}", Severity.Info);
            }
            else
            {
                _author = Mapper.Map<EmployeeViewModel>(authorResponse.Value);
            }
            if (TaskToWatch.ContractorEmployeeId != null)
            {
                GetEmployeeByIdQuery contractorQuery = new(new((int)TaskToWatch.ContractorEmployeeId, false, false, false));
                var contractorResponse = await Mediator.Send(contractorQuery);
                if (!contractorResponse.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить данные исполнителя задачи. Причина: {contractorResponse.Errors.AsOneString()}", Severity.Info);
                }
                else
                {
                    _contractor = Mapper.Map<EmployeeViewModel>(contractorResponse.Value);
                }
            }
        }

        private async Task OnEmployeeWatch(int? employeeId)
        {
            if (employeeId == null)
            {
                Snackbar.Add("Невозможно загрузить данные сотрудника, т.к. его id = null", Severity.Info);
            }
            else
            {
                EmployeeViewModel employee = new EmployeeViewModel();
                GetEmployeeByIdQuery employeeQuery = new(new((int)employeeId, true, false, false));
                var employeeResponse = await Mediator.Send(employeeQuery);
                if (!employeeResponse.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить данные сотрудника (id = {employeeId}). Причина: {employeeResponse.Errors.AsOneString()}", Severity.Info);
                }
                else
                {
                    employee = Mapper.Map<EmployeeViewModel>(employeeResponse.Value);
                }
                DialogParameters parameters = new DialogParameters
                {
                    { nameof(WatchEmployeeDialog.EmployeeToWatch), employee }
                };
                DialogService.Show<WatchEmployeeDialog>("Просмотр сотрудника", parameters);
            }
        }

        private void OnProjectWatch()
        {
            if (_projectOfTask == null)
            {
                Snackbar.Add("Невозможно отобразить данные проекта", Severity.Info);
            }
            else
            {                
                DialogParameters parameters = new DialogParameters
                {
                    { nameof(WatchProjectDialog.Project), _projectOfTask }
                };
                DialogService.Show<WatchProjectDialog>("Просмотр проекта");
            }
        }

        private void Cancel() => MudDialog.Cancel();
    }
}
