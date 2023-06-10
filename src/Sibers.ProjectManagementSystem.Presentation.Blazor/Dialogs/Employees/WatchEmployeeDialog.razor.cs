using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees
{
    public partial class WatchEmployeeDialog
    {
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
        public EmployeeDto EmployeeToWatch { get; set; }

        private ICollection<ProjectDto> _projects;

        protected override async Task OnInitializedAsync()
        {
            await LoadProjects();
        }

        private async Task LoadProjects()
        {
            if (EmployeeToWatch != null)
            {
                if (EmployeeToWatch.ProjectsIds == null || EmployeeToWatch.ProjectsIds.Count == 0)
                {
                    GetEmployeeByIdQuery query = new GetEmployeeByIdQuery(new EmployeeIncludeOptions(EmployeeToWatch.Id, true));
                    Result<EmployeeDto> employeeResult = await Mediator.Send(query);
                    if (!employeeResult.IsSuccess)
                    {
                        Snackbar.Add($"Произошла ошибка при обновлении данных сотрудника. Причина: {employeeResult.Errors.AsOneString()}", Severity.Error);
                    }
                    else
                    {
                        EmployeeToWatch = employeeResult.Value;
                    }
                }
                GetRangeOfProjectsQuery projectsQuery = new GetRangeOfProjectsQuery(EmployeeToWatch.ProjectsIds.Select(id => new ProjectIncludeOptions(id, true)));
                var projectsResult = await Mediator.Send(projectsQuery);
                if (!projectsResult.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить проекты сотрудника. Причина: {projectsResult.Errors.AsOneString()}", Severity.Error);
                    _projects = new List<ProjectDto>();
                }
                else
                    _projects = projectsResult.Value.ToList();
            }
            else
            {
                Snackbar.Add("Не удалось получить сотрудника.", Severity.Warning);
                EmployeeToWatch = new EmployeeDto();
            }
        }

        private void Cancel() => MudDialog.Cancel();
    }
}
