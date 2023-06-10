using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;
using Sibers.ProjectManagementSystem.SharedKernel.Results;


namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Projects
{
    public partial class EditProjectDialog
    {
        [Inject]
        protected IMediator Mediator { get; set; }
        [Inject]
        protected ISnackbar Snackbar { get; set; }
        [Inject]
        protected IDialogService DialogService { get; set; }
        [Inject]
        protected IMapper Mapper { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        private ICollection<EmployeeDto> _employeesOnProject = new List<EmployeeDto>();
        private ICollection<EmployeeDto> _initialSetOfEmployees = new List<EmployeeDto>();
        private EmployeeDto? _manager;  // текущий менеджер
        private EmployeeDto? _oldManager;  // менеджер, который был изначально
        private string _managerFullName = "";

        [Parameter]
        public ProjectViewModel ProjectToEdit { get; set; } = new ProjectViewModel();


        private async Task LoadProject()
        {
            if (ProjectToEdit == null)
            {
                Snackbar.Add("Can not to load project!", Severity.Info);
                Cancel();
            }
            else
            {
                GetProjectByIdQuery query = new GetProjectByIdQuery(new ProjectIncludeOptions(ProjectToEdit.Id, true));
                var result = await Mediator.Send(query);
                if (!result.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить проект. Причина: {result.Errors.AsOneString()}", Severity.Error);
                    Cancel();
                }
                else
                {
                    ProjectToEdit = Mapper.Map<ProjectViewModel>(result.Value);
                }
            }
        }

        private bool _loading = false;
        private DateTime? _projectStartDate;
        private DateTime? _projectEndDate;

        protected override async Task OnInitializedAsync()
        {
            await LoadProject();
            await LoadEmployees(ProjectToEdit.EmployeesIds);
            await SetManager();
            _managerFullName = $"{_manager?.FirstName} {_manager?.LastName} {_manager?.Patronymic}";
        }

        private async Task OnEmployeeRemovingFromProject(int employeeId)
        {
            bool? result = await DialogService.ShowMessageBox(
                title: $"Удаление сотрудника {employeeId}",
                markupMessage: new MarkupString("Вы действительно хотите снять сотрудника? Действие невозможно будет отменить"),
                yesText: "Снять",
                cancelText: "Отмена"
            );
            if (result != null && result == true)
            {
                if (_employeesOnProject.Any(e => e.Id == employeeId))
                {
                    _employeesOnProject.Remove(_employeesOnProject.First(e => e.Id == employeeId));
                }
            }
        }

        private async Task OnEmplyeesAdding()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(SelectEmployeesDialog.Multiselection), true);
            parameters.Add(nameof(SelectEmployeesDialog.LoadEmloyees), true);
            parameters.Add(nameof(SelectEmployeesDialog.ExcludedEmployeesList), _employeesOnProject);  // добавить можно только тех сотрудников, которых еще нет на проекте
            var dialog = DialogService.Show<SelectEmployeesDialog>("Выбор сотрудников", parameters);
            using var task = dialog.Result;
            var result = await task;
            if (result != null && !result.Cancelled)
            {
                if (result.Data is IEnumerable<EmployeeDto> employees)
                {
                    foreach (var employee in employees)
                    {
                        if (!_employeesOnProject.Any(e => e.Id == employee.Id))
                            _employeesOnProject.Add(employee);
                    }
                }
            }
        }

        private async Task Submit()
        {
            Result updateResult = await UpdateProjectsDataOnServer();
            if (!updateResult.IsSuccess)
            {
                Snackbar.Add($"Не удалось обновить данные проекта. Причина: {updateResult.Errors.AsOneString()}");
                MudDialog.Close(DialogResult.Ok(false));
                return;
            }
            Result changeManagerResult = await ChangeManagerOnServer();
            if (!changeManagerResult.IsSuccess)
            {
                Snackbar.Add($"Не удалось изменить руководителя проекта. Причина: {changeManagerResult.Errors.AsOneString()}");
                MudDialog.Close(DialogResult.Ok(false));
                return;
            }
            Result addEmployeesResult = await AddEmployeesOnProjectOnServer();
            if (!addEmployeesResult.IsSuccess)
            {
                Snackbar.Add($"Не удалось добавить сотрудников на проект. Причина: {addEmployeesResult.Errors.AsOneString()}");
                MudDialog.Close(DialogResult.Ok(false));
                return;
            }
            Result removeEmployeesResult = await RemoveEmployeesFromProjectOnServer();
            if (!removeEmployeesResult.IsSuccess)
            {
                Snackbar.Add($"Не удалось удалить сотрудников с проекта. Причина: {removeEmployeesResult.Errors.AsOneString()}");
                MudDialog.Close(DialogResult.Ok(false));
                return;
            }
            Snackbar.Add("Изменения успешно применены.", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }

        private async Task<Result> UpdateProjectsDataOnServer()
        {
            UpdateProjectsDataCommand command = new(Mapper.Map<ProjectDto>(ProjectToEdit));
            var result = await Mediator.Send(command);
            if (result.IsSuccess)
            {
                ProjectToEdit = Mapper.Map<ProjectViewModel>(result.Value);
                return Result.Success();
            }
            else
            {
                return Result.Error(result.Errors.ToArray());
            }
        }

        private async Task<Result> AddEmployeesOnProjectOnServer()
        {
            List<int> addedEmployees = new List<int>();
            foreach (var employee in _employeesOnProject)
            {
                if (!_initialSetOfEmployees.Any(e => e.Id == employee.Id))
                    addedEmployees.Add(employee.Id);
            }
            if (addedEmployees.Count > 0)
            {
                AddRangeOfEmployeesCommand command = new(addedEmployees, ProjectToEdit.Id);
                var result = await Mediator.Send(command);
                return result;
            }
            return Result.Success();
        }

        private async Task<Result> ChangeManagerOnServer()
        {
            if (_manager != null && _oldManager != null && !_manager.Equals(_oldManager)) // if manager was changed
            {
                Result removeResult = await DemoteOldManagerOnServer();
                if (!removeResult.IsSuccess)
                    return removeResult;
                // add manager to the project -> promote to manager; 
                return await AddNewManagerOnServer();
            }
            else if (_oldManager == null && _manager != null) // project has no has manager before
            {
                return await AddNewManagerOnServer();
            }
            else if (_oldManager != null && _manager == null)  // old manager was demoted
            {
                return await DemoteOldManagerOnServer();
            }
            return Result.Success();
        }

        private async Task<Result> AddNewManagerOnServer()
        {
            if (_manager == null)
                return Result.Error("New manager was null!");
            //Func<EmployeeDto, bool> criterion = new Func<EmployeeDto, bool>(e => e.Id == _manager.Id);
            //if (_employeesOnProject.Any(criterion))
            //    _employeesOnProject.RemoveWithCriterion(criterion);
            if (!_initialSetOfEmployees.Any(e => e.Id == _manager.Id))  // если менеджера не было на проекте вообще
            {
                AddEmployeeCommand addCommand = new(ProjectToEdit.Id, _manager.Id); // add him/her on a server
                var addResult = await Mediator.Send(addCommand);
                if (addResult == null)
                    return Result.Error("Result of add command was null!");
                if (!addResult.IsSuccess)
                {
                    return addResult;
                }
                else
                {
                    _employeesOnProject.RemoveWithCriterion(e => e.Id == _manager.Id); // абсолютно новый менеджер уже был добавлен, как сотрудник, поэтому его нужно убрать
                }
            }
            // manager was added to the project or was worked on it before
            PromoteEmployeeToManagerCommand promoteCommand = new(ProjectToEdit.Id, _manager.Id);
            var promoteResult = await Mediator.Send(promoteCommand);
            if (promoteResult == null)
                return Result.Error("Promote result was null!");
            if (!promoteResult.IsSuccess)
                return promoteResult;
            return Result.Success();
        }

        private async Task<Result> DemoteOldManagerOnServer()
        {
            if (_oldManager == null)
                return Result.Error("_oldManager was null");
            // demote old manager
            DemoteManagerToEmployeeCommand demoteCommand = new(ProjectToEdit.Id, "");
            var demoteResult = await Mediator.Send(demoteCommand);
            if (demoteResult == null)
                return Result.Error("Result of demote command was null!");
            if (!demoteResult.IsSuccess)
                return demoteResult;

            // old manager was added as employee on a server, so we must remove _oldManager from collections
            //Func<EmployeeDto, bool> criterion = new Func<EmployeeDto, bool>(e => e.Id == _oldManager.Id);
            //if (_employeesOnProject.Contains(criterion))
            //    _employeesOnProject.RemoveWithCriterion(criterion);
            // TODO: добавить менеджера в коллекцию сотрудников как сотрудника, т.е. просто обновить представление
            return Result.Success();
        }

        void Cancel() => MudDialog.Cancel();

        private async Task LoadEmployees(IEnumerable<int> ids)
        {
            try
            {
                GetRangeOfEmployeesQuery query = new(ids.Select(id => new EmployeeIncludeOptions(id, false)));
                Result<IEnumerable<EmployeeDto>> result = await Mediator.Send(query);
                if (!result.IsSuccess)
                {
                    _employeesOnProject = new List<EmployeeDto>();
                    Snackbar.Add($"Не удалось получить данные сотрудников. Причина: {result.Errors.AsOneString()}", Severity.Info);
                }
                else
                {
                    _employeesOnProject = result.Value.ToList();
                    _initialSetOfEmployees = result.Value.ToList();
                }
            }
            catch (Exception e)
            {
                Snackbar.Add($"Что-то пошло не так. Причина: {e.ReadErrors()}", Severity.Error);
            }
        }

        private async Task SetManager()
        {
            if (ProjectToEdit == null)
            {
                Snackbar.Add("Project to edit is null.", Severity.Error);
                Cancel();
                return;
            }
            if (_employeesOnProject == null || _employeesOnProject.Count() == 0)
                _manager = null;
            else
            {
                _manager = _employeesOnProject.FirstOrDefault(e => e.Id == ProjectToEdit.ManagerId);
                if (_manager == null)
                {
                    GetEmployeeByIdQuery query = new(new EmployeeIncludeOptions(ProjectToEdit.ManagerId, false));
                    var response = await Mediator.Send(query);
                    if (response.IsSuccess)
                    {
                        _manager = response.Value;
                    }
                }
            }
            _oldManager = _manager;
        }

        private async Task<Result> RemoveEmployeesFromProjectOnServer()
        {
            List<int> removedEmployeesIds = new List<int>();
            foreach (var employee in _initialSetOfEmployees)
            {
                if (!_employeesOnProject.Any(e => e.Id == employee.Id))
                    removedEmployeesIds.Add(employee.Id);
            }
            if (removedEmployeesIds.Count > 0)
            {
                RemoveRangeOfEmployeesCommand command = new(removedEmployeesIds, ProjectToEdit.Id);
                var result = await Mediator.Send(command);
                if (result == null)
                    return Result.Error("Remove employees result was null!");
                return result;
            }
            return Result.Success();
        }

        private async Task OnManagerChanging()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(SelectEmployeesDialog.Multiselection), false);
            parameters.Add(nameof(SelectEmployeesDialog.LoadEmloyees), false);
            parameters.Add(nameof(SelectEmployeesDialog.EmployeesList), _employeesOnProject);
            var dialog = DialogService.Show<SelectEmployeesDialog>("Выбор менеджера", parameters);
            using var task = dialog.Result;
            var result = await task;
            if (result != null && !result.Cancelled)
            {
                if (result.Data is EmployeeDto futureManager)
                {
                    //if (_manager != null)
                    //    _employeesOnProject.Add(_manager);
                    _manager = futureManager;  // old manager in _oldManager. 
                    //_employeesOnProject.Remove(futureManager);
                    _managerFullName = $"{_manager.FirstName} {_manager.LastName} {_manager.Patronymic}";
                }
            }
        }
    }
}
