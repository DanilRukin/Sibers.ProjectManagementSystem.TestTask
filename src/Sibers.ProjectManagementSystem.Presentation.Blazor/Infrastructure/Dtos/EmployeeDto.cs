namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos
{
    public class EmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Id { get; set; }
        public List<int> ProjectsIds { get; set; } = new List<int>();
    }
}
