using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Tasks
{
    public partial class EditTaskDialog
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
        /// Задача для редактирования. Должна быть установлена извне.
        /// </summary>
        [Parameter]
        public TaskViewModel TaskToEdit { get; set; }

        private ProjectViewModel _projectOfTask = new ProjectViewModel();
        private EmployeeViewModel _author = new EmployeeViewModel();
        private EmployeeViewModel _contractorOfTask = new EmployeeViewModel();

        /// <summary>
        /// ФИО исполнителя
        /// </summary>
        private string _contractorFullName = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            GetProjectByIdQuery projectQuery = new(new ProjectIncludeOptions(TaskToEdit.ProjectId, true));
            var projectResponse = await Mediator.Send(projectQuery);
            if (!projectResponse.IsSuccess) 
            {
                Snackbar.Add($"Не удалось загрузить данные проекта. Причина: {projectResponse.Errors.AsOneString()}", Severity.Info);
            }
            else
            {
                _projectOfTask = Mapper.Map<ProjectViewModel>(projectResponse.Value);
            }
            GetEmployeeByIdQuery authorQuery = new(new(TaskToEdit.AuthorEmployeeId, false));
            var authorResponse = await Mediator.Send(authorQuery);
            if (!authorResponse.IsSuccess)
            {
                Snackbar.Add($"Не удалось загрузить данные автора задачи. Причина: {authorResponse.Errors.AsOneString()}", Severity.Info);
            }
            else
            {
                _author = Mapper.Map<EmployeeViewModel>(authorResponse.Value);
            }
            if (TaskToEdit.ContractorEmployeeId != null)
            {
                GetEmployeeByIdQuery contractorQuery = new(new((int)TaskToEdit.ContractorEmployeeId, false));
                var contractorResponse = await Mediator.Send(contractorQuery);
                if (!contractorResponse.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить данные исполнителя задачи. Причина: {contractorResponse.Errors.AsOneString()}", Severity.Info);
                }
                else
                {
                    _contractorOfTask = Mapper.Map<EmployeeViewModel>(contractorResponse.Value);
                    _contractorFullName = $"{_contractorOfTask.FirstName} {_contractorOfTask.LastName} {_contractorOfTask.Patronymic}";
                }
            }           
        }

        private async Task OnSelectContractor()
        {
            DialogParameters parameters = new DialogParameters
            {
                { nameof(SelectEmployeesDialog.Multiselection), false },
                { nameof(SelectEmployeesDialog.LoadEmloyees), true },
                { nameof(SelectEmployeesDialog.IncludeOnly), new List<int>(_projectOfTask.EmployeesIds).Remove(_contractorOfTask.Id) },
            };
            var dialog = DialogService.Show<SelectEmployeesDialog>("Выбор исполнителя", parameters);
            var task = dialog.Result;
            var result = await task;
            if (result != null && !result.Canceled)
            {
                if (result.Data is EmployeeViewModel employee)
                {
                    _contractorOfTask = employee;
                    _contractorFullName = $"{_contractorOfTask.FirstName} {_contractorOfTask.LastName} {_contractorOfTask.Patronymic}";
                }
            }
        }

        /// <summary>
        /// Метод, вызываемый при нажатии на кнопку "Применить". 
        /// В случае успеха, создается <see cref="TaskViewModel"/> обновленной задачи и возвращается в <see cref="MudBlazor.DialogResult"/>.
        /// </summary>
        /// <returns></returns>
        private async Task Submit()
        {
            try
            {
                TaskToEdit.ContractorEmployeeId = _contractorOfTask.Id;
                UpdateTaskCommand updateTaskCommand = new(Mapper.Map<TaskDto>(TaskToEdit));
                var response = await Mediator.Send(updateTaskCommand);
                if (!response.IsSuccess)
                {
                    Snackbar.Add($"Данные не были обновлены. Причина: {response.Errors.AsOneString()}", Severity.Error);
                    MudDialog.Close(DialogResult.Ok(false));
                }
                else
                {
                    Snackbar.Add("Данные обновлены!", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(Mapper.Map<TaskViewModel>(response.Value)));
                }
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
                MudDialog.Close(DialogResult.Ok(false));
            }
        }

        private void Cancel() => MudDialog.Cancel();
    }
}
