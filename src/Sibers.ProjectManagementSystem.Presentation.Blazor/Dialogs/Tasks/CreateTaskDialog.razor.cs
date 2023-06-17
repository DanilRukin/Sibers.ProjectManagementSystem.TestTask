using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Tasks
{
    /// <summary>
    /// Диалог создания задачи. В случае успеха <see cref="MudBlazor.DialogResult"/> вернет <see cref="TaskViewModel"/> созданной задачи.
    /// В противном случае будет возвращен <see cref="MudBlazor.DialogResult.Ok{T}(T)"/>
    /// </summary>
    public partial class CreateTaskDialog
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
        /// Задача, которую необходимо создать
        /// </summary>
        private TaskViewModel _taskToCreate = new TaskViewModel();

        /// <summary>
        /// Проект, которому принадлежит задача
        /// </summary>
        [Parameter]
        public ProjectViewModel ProjectOfTask { get; set; }

        /// <summary>
        /// Автор задачи
        /// </summary>
        [Parameter]
        public EmployeeViewModel Author { get; set; }

        private EmployeeViewModel _contractor = new EmployeeViewModel();

        private string _contractorFullName = "";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (ProjectOfTask == null)
            {
                Snackbar.Add("Необходимо установить проект, которому принадлежит задача", Severity.Error);
                MudDialog.Cancel();
                return;
            }
            if (Author == null)
            {
                Snackbar.Add("Необходимо установить автора задачи", Severity.Error);
                MudDialog.Cancel();
                return;
            }
        }

        private async Task OnSelectContractor()
        {
            DialogParameters parameters = new DialogParameters
            {
                { nameof(SelectEmployeesDialog.Multiselection), false },
                { nameof(SelectEmployeesDialog.LoadEmloyees), true },
                { nameof(SelectEmployeesDialog.IncludeOnly), new List<int>(ProjectOfTask.EmployeesIds).Remove(_contractor.Id) },
            };
            var dialog = DialogService.Show<SelectEmployeesDialog>("Выбор исполнителя", parameters);
            var task = dialog.Result;
            var result = await task;
            if (result != null && !result.Canceled)
            {
                if (result.Data is EmployeeViewModel employee)
                {
                    _contractor = employee;
                    _contractorFullName = $"{_contractor.FirstName} {_contractor.LastName} {_contractor.Patronymic}";
                }
            }
        }
        /// <summary>
        /// Метод, вызываемый при нажатии на кнопку "Создать". 
        /// В случае успеха, создается <see cref="TaskViewModel"/> созданной задачи и возвращается в <see cref="MudBlazor.DialogResult"/>.
        /// </summary>
        /// <returns></returns>
        private async Task Submit()
        {
            try
            {
                _taskToCreate.ProjectId = ProjectOfTask.Id;
                _taskToCreate.AuthorEmployeeId = Author.Id;
                _taskToCreate.ContractorEmployeeId = _contractor.Id;
                CreateTaskCommand createTaskCommand = new(Mapper.Map<TaskDto>(_taskToCreate));
                var response = await Mediator.Send(createTaskCommand);
                if (!response.IsSuccess)
                {
                    Snackbar.Add($"Задача не была создана. Причина: {response.Errors.AsOneString()}", Severity.Error);
                    MudDialog.Close(DialogResult.Ok(false));
                }
                else
                {
                    Snackbar.Add("Задача создана!", Severity.Success);
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
