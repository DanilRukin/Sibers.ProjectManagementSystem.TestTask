using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Pages
{
    public partial class Employees
    {
        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        private ICollection<EmployeeViewModel> _employeesList;

        protected override async Task OnInitializedAsync()
        {
            await LoadEmployees();
        }

        private async Task LoadEmployees()
        {
            try
            {
                GetAllEmployeesQuery query = new GetAllEmployeesQuery(false);
                Result<IEnumerable<EmployeeDto>> result = await Mediator.Send(query);
                if (result == null)
                    _employeesList = new List<EmployeeViewModel>();
                else if (result.IsSuccess)
                    _employeesList = Mapper.Map<IEnumerable<EmployeeDto>, ICollection<EmployeeViewModel>>(result.Value);
                else
                {
                    _employeesList = new List<EmployeeViewModel>();
                    Snackbar.Add(result.Errors.AsOneString(), Severity.Error);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add($"Что-то пошло не так. Причина: {e.ReadErrors()}", Severity.Error);
            }
        }

        private async Task OnEmployeeEdit(int employeeId)
        {
            EmployeeViewModel? employeeToEdit = _employeesList.FirstOrDefault(e => e.Id == employeeId);
            if (employeeToEdit == null)
            {
                Snackbar.Add("Не удалось получить сотрудника для редактирования.", Severity.Warning);
                return;
            }
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(EditEmployeeDialog.EmployeeToEdit), employeeToEdit);
            var dialog = DialogService.Show<EditEmployeeDialog>("Управление сотрудником", parameters);
            using var task = dialog.Result;
            var result = await task;
            if (result != null && !result.Cancelled && (result.Data is bool ok))
            {
                GetEmployeeByIdQuery query = new(new EmployeeIncludeOptions(employeeId, false));
                Result<EmployeeDto> updatedResult = await Mediator.Send(query);
                if (updatedResult.IsSuccess)
                {
                    _employeesList.RemoveWithCriterion(e => e.Id == employeeId);
                    _employeesList.Add(Mapper.Map<EmployeeViewModel>(updatedResult.Value));
                }
                else
                {
                    Snackbar.Add($"Не удалось получить обновленные данные сотрудника."
                    + $" Причина: {updatedResult.Errors.AsOneString()}", Severity.Error);
                }
            }
        }

        private void OnEmployeeWatch(EmployeeViewModel employee)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(WatchEmployeeDialog.EmployeeToWatch), employee);
            var dialog = DialogService.Show<WatchEmployeeDialog>("Просмотр сотрудника", parameters);
        }

        private async Task OnEmployeeCreate()
        {
            var dialog = DialogService.Show<CreateEmployeeDialog>("Создание сотрудника");
            using var task = dialog.Result;
            var result = await task;
            if (result != null && !result.Cancelled && (result.Data is EmployeeViewModel employee))
            {
                _employeesList.Add(employee);
            }
        }

        private async Task OnEmployeeDelete(EmployeeViewModel employee)
        {
            bool? result = await DialogService.ShowMessageBox(
                title: $"Удаление сотрудника '{employee.FirstName} {employee.LastName} {employee.Patronymic}'",
                markupMessage: new MarkupString("Вы действительно хотите удалить сотрудника? Действие невозможно будет отменить"),
                yesText: "Удалить",
                cancelText: "Отмена"
            );
            if (result != null && result == true)
            {
                DeleteEmployeeCommand command = new(employee.Id);
                Result deleteResult = await Mediator.Send(command);
                if (deleteResult.IsSuccess)
                {
                    _employeesList.Remove(employee);
                    Snackbar.Add("Сотрудник был удален", Severity.Success);
                }
                else
                {
                    Snackbar.Add($"Сотрудник не был удален. Причина: {deleteResult.Errors.AsOneString()}", Severity.Error);
                }
            }
        }
    }
}
