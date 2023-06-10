using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees
{
    public partial class CreateEmployeeDialog
    {
        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }


        [CascadingParameter]
        public MudDialogInstance MudDialog { get; set; }

        private EmployeeDto _employee = new EmployeeDto();

        async Task Submit()
        {
            CreateEmployeeCommand command = new CreateEmployeeCommand(_employee);
            var result = await Mediator.Send(command);
            if (result.IsSuccess)
            {
                Snackbar.Add("Сотрудник создан", Severity.Success);
                MudDialog.Close(DialogResult.Ok<EmployeeDto>(result.Value));
            }
            else
            {
                Snackbar.Add($"Сотрудник не был создан. Причина: {result.Errors.AsOneString()}", Severity.Error);
                MudDialog.Close(DialogResult.Ok(false));
            }
        }
        void Cancel() => MudDialog.Cancel();
    }
}
