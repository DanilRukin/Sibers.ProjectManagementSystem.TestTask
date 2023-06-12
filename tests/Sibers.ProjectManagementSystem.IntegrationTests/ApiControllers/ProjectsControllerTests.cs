using Azure.Core;
using Sibers.ProjectManagementSystem.Api;
using Sibers.ProjectManagementSystem.Application.ProjectAgregate;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sibers.ProjectManagementSystem.IntegrationTests.ApiControllers
{
    public class ProjectsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;
        private static readonly string _api = "api/projects";
        public ProjectsControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_CreateNewProject()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1);
            ProjectDto requestBody = new ProjectDto
            {
                Name = "SomeProjectName",
                Priority = 1,
                NameOfTheContractorCompany = "Microsoft",
                NameOfTheCustomerCompany = "Oracle",
                StartDate = startDate,
                EndDate = endDate,
                EmployeesIds = new List<int>(),
            };
            
            var response = await _client.PostAsJsonAsync<ProjectDto>(Post.Create(), requestBody);
            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var result = await response.Content.ReadFromJsonAsync<ProjectDto>();
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task GetAll_NotIncludeAdditioanlData_GetAllDataFromDatabase()
        {
            var result = await _client.GetFromJsonAsync<IEnumerable<ProjectDto>>(Get.All());

            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async Task GetAll_IncludeAdditionalData_GetAllFromDatabase()
        {
            var result = await _client.GetFromJsonAsync<IEnumerable<ProjectDto>>(Get.All(true));

            Assert.NotNull(result);
            ProjectDto project = result.FirstOrDefault(p => p.Id == SeedData.Project1.Id);
            Assert.NotNull(project);
            Assert.NotEmpty(project.EmployeesIds);
        }

        [Fact]
        public async Task GetById_NotIncludeAdditionalData_ReturnProjectDtoWithoutEmployeesAndTasks()
        {
            int id = SeedData.Project1.Id;

            ProjectDto? result = await _client.GetFromJsonAsync<ProjectDto?>(Get.ById(id, false));
            

            Assert.NotNull(result);
            Assert.Empty(result.EmployeesIds);
        }

        [Fact]
        public async Task GetById_IncludeAdditionalData_ReturnProjectDtoWithEmployeesIdsAndTasksIds()
        {
            int id = SeedData.Project1.Id;

            ProjectDto? result = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(id, true));

            Assert.NotNull(result);
            Assert.NotEmpty(result.EmployeesIds);
        }

        [Fact]
        public async Task AddEmployee_NoSuchEmployee_ReturnNotFoundResult()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = int.MaxValue;

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.AddEmployee(projectId, employeeId), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task AddEmployee_EmployeeIsAlreadyWorksOnAProject_ReturnBadRequestResultWithMessage()
        {
            _factory.ResetDatabase();
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee1_WorksOnProject1.Id;
            string message = $"Such employee (id: {employeeId}) is already works on this project";

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.AddEmployee(projectId, employeeId), UriKind.Relative),
            });
            
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var actualMessages = await result.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.Single(actualMessages);
            string actual = actualMessages.First();
            Assert.Equal(message, actual);
        }

        [Fact]
        public async Task AddEmployee_EmployeeWasAdded()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee3_WorksOnProject2.Id;
            Assert.DoesNotContain(SeedData.Employee3_WorksOnProject2, SeedData.Project1.Employees);
            ProjectDto? project = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(projectId, true));
            Assert.DoesNotContain(employeeId, project.EmployeesIds);

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.AddEmployee(projectId, employeeId), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            project = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(projectId, true));
            Assert.Contains(employeeId, project.EmployeesIds);
        }

        [Fact]
        public async Task PromoteEmployee_NoSuchEmployeeOnAProject_ReturnsBadRequestWithMessage()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee3_WorksOnProject2.Id;
            string message = "Current emplyee is not work on this project. Add him/her to project first";

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.PromoteEmployeeToManager(projectId, employeeId), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var errors = await result.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.Single(errors);
            Assert.Equal(message, errors.First());
        }

        [Fact]
        public async Task PromoteEmployee_NoSuchEmployeeInDatabase_ReturnsNotFoundResult()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = int.MaxValue;
            string message = $"No such employee with id: {employeeId}";

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.PromoteEmployeeToManager(projectId, employeeId), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            var errors = await result.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.Single(errors);
            Assert.Equal(message, errors.First());
        }

        [Fact]
        public async Task PromoteEmployee_EmployeeWasPromoted()
        {
            _factory.ResetDatabase();
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee1_WorksOnProject1.Id;           
            Assert.Null(SeedData.Project1.Manager);
            ProjectDto? project = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(projectId, true));
            Assert.NotEqual(0, employeeId);
            Assert.Equal(0, project.ManagerId);

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.PromoteEmployeeToManager(projectId, employeeId), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            project = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(projectId, true));
            Assert.NotEmpty(project.EmployeesIds);
            Assert.Equal(employeeId, project.ManagerId);
        }

        [Fact]
        public async Task DemoteManager_EmployeeIsNotAManager_ReturnsBadRequestWithMessage()
        {
            int projectId = SeedData.Project1.Id;
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.DemoteManager(projectId, "reason"), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var errors = await result.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.Single(errors);
            Assert.Equal(message, errors.First());
        }

        [Fact]
        public async Task DemoteManager_ReturnsOkStatusCodeAndManagerDemoted()
        {
            _factory.ResetDatabase();
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee1_WorksOnProject1.Id;
            Assert.Null(SeedData.Project1.Manager);
            ProjectDto? project = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(projectId, true));
            Assert.NotEqual(0, employeeId);
            Assert.Equal(0, project.ManagerId);

            await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.PromoteEmployeeToManager(projectId, employeeId), UriKind.Relative)
            });

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.DemoteManager(projectId, "reason"), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            project = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(projectId, true));
            Assert.NotEmpty(project.EmployeesIds);
            Assert.NotEqual(employeeId, project.ManagerId);
        }

        [Fact]
        public async Task FireManager_NoManager_ReturnsBadRequestWithMessage()
        {
            _factory.ResetDatabase();
            int projectId = SeedData.Project1.Id;
            Assert.Null(SeedData.Project1.Manager);
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.FireManager(projectId, "he is bad"), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var errors = await result.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.Single(errors);
            Assert.Equal(message, errors.First());
        }

        [Fact]
        public async Task FireManager_ManagerFired_ReturnOkStatusCode()
        {
            _factory.ResetDatabase();
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee1_WorksOnProject1.Id;
            Assert.Null(SeedData.Project1.Manager);
            ProjectDto? project = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(projectId, true));
            Assert.NotEqual(0, employeeId);
            Assert.Equal(0, project.ManagerId);

            await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.PromoteEmployeeToManager(projectId, employeeId), UriKind.Relative)
            });

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.FireManager(projectId, "reason"), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            project = await _client.GetFromJsonAsync<ProjectDto>(Get.ById(projectId, true));
            Assert.DoesNotContain(employeeId, project.EmployeesIds);
            Assert.Equal(0, project.ManagerId);
        }

        private static class Get
        {
            internal static string ById(int id, bool includeAdditionalData = false) 
                => $"{_api}/{id}/{includeAdditionalData}";
            internal static string All(bool includeAdditionalData = false) 
                => $"{_api}/all?includeAdditionalData={includeAdditionalData}";
        }

        private static class Put
        {
            internal static string AddEmployee(int projectId, int employeeId)
                => $"{_api}/addemployee/{projectId}/{employeeId}";
            internal static string RemoveEmployee(int projectId, int emplyeeId)
                => $"{_api}/removeemployee/{projectId}/{emplyeeId}";
            internal static string FireManager(int projectId, string reason)
                => $"{_api}/firemanager/{projectId}/{reason}";
            internal static string DemoteManager(int projectId, string reason)
                => $"{_api}/demotemanager/{projectId}/{reason}";
            internal static string PromoteEmployeeToManager(int projectId, int employeeId)
                => $"{_api}/promoteemployee/{projectId}/{employeeId}";
            internal static string TransferEmployee(int currentProjectId, int futureProjectId, int employeeId)
                => $"{_api}/transferemployee/{currentProjectId}/{futureProjectId}/{employeeId}";
        }

        private static class Post
        {
            internal static string Create() => _api;
        }

        private static class Delete
        {
            internal static string ById(int id) => $"{_api}/{id}";
        }
    }
}
