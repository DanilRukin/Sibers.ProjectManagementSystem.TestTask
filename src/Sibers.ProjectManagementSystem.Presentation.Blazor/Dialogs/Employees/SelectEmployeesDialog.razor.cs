using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Employees
{
    public partial class SelectEmployeesDialog
    {
        [Inject]
        public IMapper Mapper { get; set; }

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
        public IEnumerable<EmployeeViewModel> EmployeesList { get; set; } = new List<EmployeeViewModel>();

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
        public ICollection<EmployeeViewModel> ExcludedEmployeesList { get; set; } = new List<EmployeeViewModel>();

        private EmployeeViewModel _selectedEmployee;

        protected override async Task OnInitializedAsync()
        {
            if (LoadEmloyees)
                await LoadEmployees();
        }
        private HashSet<EmployeeViewModel> _selectedEmployees = new HashSet<EmployeeViewModel>();

        private async Task LoadEmployees()
        {
            IEnumerable<EmployeeDto>? employees = new List<EmployeeDto>();
            if (IncludeOnly == null || IncludeOnly.Count == 0)
            {
                GetAllEmployeesQuery query = new GetAllEmployeesQuery(false, false, false);
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
                GetRangeOfEmployeesQuery query = new GetRangeOfEmployeesQuery(IncludeOnly.Select(id => new EmployeeIncludeOptions(id, false, false, false)));
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
                EmployeesList = new List<EmployeeViewModel>();
            else
                EmployeesList = Mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<EmployeeViewModel>>(employees);
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
