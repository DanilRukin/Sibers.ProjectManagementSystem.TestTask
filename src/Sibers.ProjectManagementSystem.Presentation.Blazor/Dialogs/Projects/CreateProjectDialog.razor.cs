using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Projects
{
    public partial class CreateProjectDialog
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

        private ICollection<EmployeeViewModel> _employeesOnProject = new List<EmployeeViewModel>();
        private EmployeeViewModel _manager = new EmployeeViewModel();
        private string _managerFullName = "";

        private ProjectViewModel project = new ProjectViewModel();

        private async Task OnEmplyeesSelecting()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(SelectEmployeesDialog.Multiselection), true);
            parameters.Add(nameof(SelectEmployeesDialog.LoadEmloyees), true);
            var dialog = DialogService.Show<SelectEmployeesDialog>("Выбор сотрудников", parameters);
            using var task = dialog.Result;
            var result = await task;
            if (!result.Cancelled)
            {
                if (result.Data is IEnumerable<EmployeeViewModel> employees)
                {
                    foreach (var item in employees)
                    {
                        _employeesOnProject.Add(item);
                        project.EmployeesIds.Add(item.Id);
                    }
                }
            }
        }

        private async Task OnManagerSelecting()
        {
            if (_employeesOnProject == null || _employeesOnProject.Count == 0)
            {
                Snackbar.Add("Руководителя можно выбрать только из сотрудников проекта", Severity.Info);
            }
            else
            {
                DialogParameters parameters = new DialogParameters();
                parameters.Add(nameof(SelectEmployeesDialog.Multiselection), false);
                parameters.Add(nameof(SelectEmployeesDialog.EmployeesList), _employeesOnProject);
                parameters.Add(nameof(SelectEmployeesDialog.LoadEmloyees), false);
                var dialog = DialogService.Show<SelectEmployeesDialog>("Выбор руководителя", parameters);
                using var task = dialog.Result;
                var result = await task;
                if (!result.Cancelled)
                {
                    if (result.Data is EmployeeViewModel employee)
                    {
                        _manager = employee;
                        _managerFullName = employee.FirstName + employee.LastName + employee.Patronymic;
                        project.ManagerId = employee.Id;
                    }
                }
            }
        }

        private void OnEmployeeDeleting(int employeeId)
        {
            _employeesOnProject.Remove(_employeesOnProject.First(e => e.Id == employeeId));
            if (_manager != null && _manager.Id == employeeId)
            {
                _manager = null;
                _managerFullName = "";
                project.ManagerId = 0;
            }
            // this project instance is not in database yet
            project.EmployeesIds.Remove(employeeId);
        }

        private async Task Submit()
        {
            try
            {
                CreateProjectCommand command = new(Mapper.Map<ProjectDto>(project));
                var result = await Mediator.Send(command);
                if (result.IsSuccess)
                {
                    await AddEmployeesToProject(result.Value.Id);
                    await AddManagerToProject(result.Value.Id);
                    Snackbar.Add("Проект успешно создан", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(Mapper.Map<ProjectViewModel>(result.Value)));
                }
                else
                    throw new Exception(result.Errors.AsOneString());
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
                MudDialog.Close(DialogResult.Ok(false));
            }
        }

        private async Task AddEmployeesToProject(int projectId)
        {
            if (_employeesOnProject != null && _employeesOnProject.Any())
            {
                AddRangeOfEmployeesCommand command = new(_employeesOnProject.Select(e => e.Id), projectId);
                var result = await Mediator.Send(command);
                if (!result.IsSuccess)
                    throw new Exception(result.Errors.AsOneString());
            }
            return;
        }

        private async Task AddManagerToProject(int projectId)
        {
            if (_manager == null)
                return;
            if (!_employeesOnProject.Contains(_manager))
                return;
            PromoteEmployeeToManagerCommand command = new(projectId, _manager.Id);
            var response = await Mediator.Send(command);
            if (!response.IsSuccess)
                throw new Exception(response.Errors.AsOneString());
            return;
        }

        void Cancel() => MudDialog.Cancel();
    }
}
