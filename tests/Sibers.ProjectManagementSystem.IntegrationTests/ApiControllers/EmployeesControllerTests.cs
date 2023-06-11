using Sibers.ProjectManagementSystem.Api;
using Sibers.ProjectManagementSystem.Api.Controllers;
using Sibers.ProjectManagementSystem.Application;
using Sibers.ProjectManagementSystem.Application.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.IntegrationTests.ApiControllers
{
    public class EmployeesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;
        private static string _api = "api/employees";

        public EmployeesControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_EmployeeCreated_ReturnsCreatedStatusCode()
        {
            EmployeeDto employee = new EmployeeDto
            {
                FirstName = "Rukin",
                LastName = "Danil",
                Patronymic = "Vitalievich",
                Email = "rukin2018d@yandex.ru",
                ProjectsIds = new List<int>(),
            };

            var response = await _client.PostAsJsonAsync<EmployeeDto>(Post.Create(), employee);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<EmployeeDto>();
            Assert.NotNull(result);
            Assert.NotEqual(employee.Id, result.Id);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task GetById_NotIncludeAdditionalData_DataNotIncluded()
        {
            int employeeId = SeedData.Employee1_WorksOnProject1.Id;
            bool includeAdditionalData = false;

            var response = await _client.GetFromJsonAsync<EmployeeDto>(Get.ById(employeeId, includeAdditionalData));

            Assert.NotNull(response);
            Assert.Equal(employeeId, response.Id);
            Assert.Equal(SeedData.Employee1_WorksOnProject1.PersonalData.FirstName, response.FirstName);
            Assert.Empty(response.ProjectsIds);
        }


        private static class Get
        {
            internal static string ById(int employeeId, bool includeData = false) 
                => $"{_api}/{employeeId}/{includeData}";
            internal static string All()
                => $"{_api}/all";
        }

        private static class Post
        {
            internal static string Create() => $"{_api}";
        }

        private static class Put
        {
            
        }

        private static class Delete
        {
            internal static string ById(int id) => $"{_api}/{id}";
        }
    }
}
