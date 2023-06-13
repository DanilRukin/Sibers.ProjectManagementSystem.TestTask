using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sibers.ProjectManagementSystem.Api.Services;
using Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Commands;
using Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Queries;
using Sibers.ProjectManagementSystem.Application.EmployeeAgregate;

namespace Sibers.ProjectManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeDto>> Create([FromBody] EmployeeDto employee)
        {
            CreateEmployeeCommand request = new(employee);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Created("", response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpGet("{id:int}/{includeProjects:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeDto>> GetById(int id, bool includeProjects = false)
        {
            GetEmployeeByIdQuery request = new(new EmployeeIncludeOptions(id, includeProjects));
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpGet("range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetRangeOfEmployees([FromQuery] IEnumerable<EmployeeIncludeOptions> options)
        {
            GetRangeOfEmployeesQuery query = new GetRangeOfEmployeesQuery(options);
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpGet("all/{includeProjects:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetALl([FromRoute] bool includeProjects = false)
        {
            GetAllEmployeesQuery query = new(includeProjects);
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeDto>> Update([FromBody] EmployeeDto employee)
        {
            UpdateEmployeesDataCommand command = new(employee);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpDelete("{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int employeeId)
        {
            DeleteEmployeeCommand query = new(employeeId);
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }
    }
}
