using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Tasks
{
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

        }

        private async Task Submit()
        {
            try
            {
                
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
