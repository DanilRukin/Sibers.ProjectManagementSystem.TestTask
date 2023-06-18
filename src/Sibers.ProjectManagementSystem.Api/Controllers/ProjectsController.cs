using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands;
using Sibers.ProjectManagementSystem.Application.ProjectAgregate.Queries;
using Sibers.ProjectManagementSystem.Application.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using Sibers.ProjectManagementSystem.Api.Services;


namespace Sibers.ProjectManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] ProjectDto dto)
        {
            CreateProjectCommand createProjectCommand = new CreateProjectCommand(dto);
            Result<ProjectDto> result = await _mediator.Send(createProjectCommand);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return ResultErrorsHandler.Handle(result);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll([FromQuery] bool includeEmployees = false, [FromQuery] bool includeTasks = false)
        {
            GetAllProjectsQuery query = new GetAllProjectsQuery(includeEmployees, includeTasks);
            Result<IEnumerable<ProjectDto>> result = await _mediator.Send(query);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return ResultErrorsHandler.Handle(result);
        }

        [HttpGet("range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetRange([FromQuery(Name = "options")]IEnumerable<ProjectIncludeOptions> options)
        {
            GetRangeOfProjectsQuery query = new GetRangeOfProjectsQuery(options);
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
                return Ok(response.GetValue());
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpGet("{id}/{includeEmployees}/{includeTasks}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProjectDto>> GetById([FromRoute] int id, [FromRoute] bool includeEmployees, [FromRoute] bool includeTasks)
        {
            GetProjectByIdQuery query = new GetProjectByIdQuery(new ProjectIncludeOptions(id, includeEmployees, includeTasks));
            Result<ProjectDto> result = await _mediator.Send(query);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return ResultErrorsHandler.Handle(result);
        }

        [HttpPut("addemployee/{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddEmployee([FromRoute] int projectId, [FromRoute] int employeeId)
        {
            AddEmployeeToTheProjectCommand request = new AddEmployeeToTheProjectCommand(projectId, employeeId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("removeemployee/{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveEmployee(int projectId, int employeeId)
        {
            RemoveEmployeeFromTheProjectCommand request = new(projectId, employeeId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("firemanager/{projectId}/{reason}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FireManager(int projectId, string reason)
        {
            FireManagerCommand request = new(projectId, reason);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("demotemanager/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DemoteManagerToEmployee([FromRoute] int projectId, [FromQuery] string? reason = "")
        {
            DemoteManagerToTheEmployeeCommand request = new(projectId, reason);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("promoteemployee/{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PromoteEmployeeToTheManager(int projectId, int employeeId)
        {
            PromoteEmployeeToManagerCommand request = new(projectId, employeeId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("transferemployee/{currentProjectId}/{futureProjectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> TransferEmployee(int currentProjectId, int futureProjectId, int employeeId)
        {
            TransferEmployeeToAnotherProjectCommand request = new(currentProjectId, futureProjectId, employeeId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("addrangeofemployees/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddRangeOfEmployees(
            [FromRoute] int projectId,
            [FromBody] IEnumerable<int> employeesIds)
        {
            AddRangeOfEmployeesCommand command = new(employeesIds, projectId);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("removerangeofemployees/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveRangeOfEmployees(
            [FromRoute] int projectId,
            [FromBody] IEnumerable<int> employeesIds)
        {
            RemoveRangeOfEmployeesFromTheProjectCommand command = new(employeesIds, projectId);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProjectDto>> Update([FromBody] ProjectDto project)
        {
            UpdateProjectsDataCommand command = new(project);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpDelete("{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int projectId)
        {
            DeleteProjectCommand request = new(projectId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }
    }
}
