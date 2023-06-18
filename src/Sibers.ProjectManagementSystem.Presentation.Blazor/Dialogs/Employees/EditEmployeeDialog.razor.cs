using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Projects;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees
{
    public partial class EditEmployeeDialog
    {
        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }


        ///<summary>
        /// Employee to edit
        /// </summary>
        [Parameter]
        public EmployeeViewModel EmployeeToEdit { get; set; }

        private ICollection<ProjectViewModel> _employeesProjects;

        protected override async Task OnInitializedAsync()
        {
            if (EmployeeToEdit == null)
            {
                Snackbar.Add("Не удалось получить сотрудника для редактирования.", Severity.Info);
                Cancel();
            }
            else
            {
                Result loadResult = await LoadEmployeeToEdit();
                if (!loadResult.IsSuccess)
                {
                    Snackbar.Add(loadResult.Errors.AsOneString(), Severity.Info);
                    Cancel();
                }
                else
                {
                    Result projectsResult = await LoadProjects(EmployeeToEdit.ProjectsIds);
                    if (!projectsResult.IsSuccess)
                    {
                        Snackbar.Add(projectsResult.Errors.AsOneString(), Severity.Info);
                        Cancel();
                    }
                }
            }
        }

        private async Task<Result> LoadEmployeeToEdit()
        {
            try
            {
                GetEmployeeByIdQuery query = new(new EmployeeIncludeOptions(EmployeeToEdit.Id, true, false, false));
                Result<EmployeeDto> response = await Mediator.Send(query);
                if (!response.IsSuccess)
                    return Result.Error("Не удалось загрузить данные сотрудника.");
                else
                {
                    EmployeeToEdit = Mapper.Map<EmployeeViewModel>(response.Value);
                    return Result.Success();
                }
            }
            catch (Exception e)
            {
                return Result.Error(e.ReadErrors());
            }
        }

        private async Task<Result> LoadProjects(IEnumerable<int> projectsIds)
        {
            GetRangeOfProjectsQuery query = new(projectsIds.Select(id => new ProjectIncludeOptions(id, true, false)));
            Result<IEnumerable<ProjectDto>> response = await Mediator.Send(query);
            if (!response.IsSuccess)
                return Result.Error($"Не удалось загрузить проекты сотрудника. Причина: {response.Errors.AsOneString()}");
            else
            {
                _employeesProjects = Mapper.Map<IEnumerable<ProjectDto>, ICollection<ProjectViewModel>>(response.Value);
                return Result.Success();
            }
        }

        private async Task OnProjectEditing(int projectId)
        {
            ProjectViewModel? projectToEdit = _employeesProjects.FirstOrDefault(p => p.Id == projectId);
            if (projectToEdit == null)
            {
                Snackbar.Add($"Не удается получить проект с id: {projectId}", Severity.Warning);
            }
            else
            {
                DialogParameters parameters = new DialogParameters();
                parameters.Add(nameof(EditProjectDialog.ProjectToEdit), projectToEdit);
                var dialog = DialogService.Show<EditProjectDialog>("Управление проектом", parameters);
                using var task = dialog.Result;
                var result = await task;
                if (result != null && !result.Cancelled && (result.Data is bool ok))
                {
                    if (ok)
                    {
                        GetProjectByIdQuery query = new(new ProjectIncludeOptions(projectId, true, false));
                        Result<ProjectDto> projectResult = await Mediator.Send(query);
                        if (projectResult.IsSuccess)
                        {
                            _employeesProjects.RemoveWithCriterion(p => p.Id == projectId);
                            _employeesProjects.Add(Mapper.Map<ProjectViewModel>(projectResult.Value));
                            Snackbar.Add("Данные проекта обновлены.", Severity.Success);
                        }
                        else
                        {
                            Snackbar.Add("Не удалось обновить данные проекта.", Severity.Warning);
                        }
                    }
                }
            }
        }

        private async Task Submit()
        {
            UpdateEmployeesDataCommand command = new(Mapper.Map<EmployeeDto>(EmployeeToEdit));
            Result<EmployeeDto> updateResult = await Mediator.Send(command);
            if (updateResult.IsSuccess)
            {
                Snackbar.Add("Данные сотрудника обновлены.", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add(updateResult.Errors.AsOneString(), Severity.Error);
            }
        }
        void Cancel() => MudDialog.Cancel();

        private string ShowRole(ProjectViewModel model) => model.ManagerId == EmployeeToEdit.Id ? "Менеджер" : "Сотрудник";

    }
}
