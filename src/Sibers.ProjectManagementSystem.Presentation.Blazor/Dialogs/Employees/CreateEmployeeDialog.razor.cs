using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;

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

        [Inject]
        public IMapper Mapper { get; set; }


        [CascadingParameter]
        public MudDialogInstance MudDialog { get; set; }

        private EmployeeViewModel _employee = new EmployeeViewModel();

        async Task Submit()
        {
            CreateEmployeeCommand command = new CreateEmployeeCommand(Mapper.Map<EmployeeDto>(_employee));
            var result = await Mediator.Send(command);
            if (result.IsSuccess)
            {
                Snackbar.Add("Сотрудник создан", Severity.Success);
                MudDialog.Close(DialogResult.Ok(Mapper.Map<EmployeeViewModel>(result.Value)));
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
