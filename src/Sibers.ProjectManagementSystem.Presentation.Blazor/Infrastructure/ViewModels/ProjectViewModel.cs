namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string NameOfTheCustomerCompany { get; set; } = string.Empty;
        public string NameOfTheContractorCompany { get; set; } = string.Empty;
        public List<int> EmployeesIds { get; set; } = new List<int>();
        public int ManagerId { get; set; }
    }
}
