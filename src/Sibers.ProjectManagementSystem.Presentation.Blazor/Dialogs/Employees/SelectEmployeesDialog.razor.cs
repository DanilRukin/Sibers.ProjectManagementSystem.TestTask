using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees
{
    public partial class SelectEmployeesDialog
    {
        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }
        /// <summary>
        /// Employees to show. Can be initialized from client's code
        /// </summary>
        [Parameter]
        public IEnumerable<EmployeeDto> EmployeesList { get; set; } = new List<EmployeeDto>();

        /// <summary>
        /// Indicates the need for multiple employees selection. Default is true.
        /// </summary>
        [Parameter]
        public bool Multiselection { get; set; } = true;

        /// <summary>
        /// Indicates the need for load employees from database. Default is false.
        /// </summary>
        [Parameter]
        public bool LoadEmloyees { get; set; } = false;

        /// <summary>
        /// Employees Ids wich will be loaded
        /// </summary>
        [Parameter]
        public ICollection<int> IncludeOnly { get; set; }

        /// <summary>
        /// Employees wich must not be showed if <see cref="LoadEmloyees"/> is true
        /// </summary>
        [Parameter]
        public ICollection<EmployeeDto> ExcludedEmployeesList { get; set; } = new List<EmployeeDto>();

        private EmployeeDto _selectedEmployee;

        protected override async Task OnInitializedAsync()
        {
            if (LoadEmloyees)
                await LoadEmployees();
        }
        private HashSet<EmployeeDto> _selectedEmployees = new HashSet<EmployeeDto>();

        private async Task LoadEmployees()
        {
            IEnumerable<EmployeeDto>? employees = new List<EmployeeDto>();
            if (IncludeOnly == null || IncludeOnly.Count == 0)
            {
                GetAllEmployeesQuery query = new GetAllEmployeesQuery(false);
                Result<IEnumerable<EmployeeDto>> result = await Mediator.Send(query);
                if (result.IsSuccess)
                {
                    employees = result.Value;
                    if (employees != null)
                    {
                        ICollection<EmployeeDto> toClear = employees.ToList();
                        foreach (var item in ExcludedEmployeesList)
                        {
                            toClear.RemoveWithCriterion(e => e.Id == item.Id);
                        }
                        employees = toClear;
                    }
                }
                else
                {
                    Snackbar.Add($"Не удалось загрузить сотрудников. Причина: {result.Errors.AsOneString()}");
                }
            }
            else
            {
                GetRangeOfEmployeesQuery query = new GetRangeOfEmployeesQuery(IncludeOnly.Select(id => new EmployeeIncludeOptions(id, false)));
                Result<IEnumerable<EmployeeDto>> result = await Mediator.Send(query);
                if (!result.IsSuccess)
                {
                    Snackbar.Add($"Не удалось загрузить сотрудников. Причина: {result.Errors.AsOneString()}");
                }
                else
                {
                    employees = result.Value;
                }
            }
            if (employees == null)
                EmployeesList = new List<EmployeeDto>();
            else
                EmployeesList = employees.ToList();
        }

        private void Submit()
        {
            if (Multiselection)
            {
                MudDialog.Close(DialogResult.Ok(_selectedEmployees));
            }
            else
            {
                MudDialog.Close(DialogResult.Ok(_selectedEmployee));
            }
        }
    }
}
