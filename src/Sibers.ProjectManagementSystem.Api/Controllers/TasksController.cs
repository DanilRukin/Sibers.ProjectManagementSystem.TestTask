using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sibers.ProjectManagementSystem.Api.Services;
using Sibers.ProjectManagementSystem.Application.TaskEntity;
using Sibers.ProjectManagementSystem.Application.TaskEntity.Commands;
using Sibers.ProjectManagementSystem.Application.TaskEntity.Queries;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Api.Controllers
{
    /// <summary>
    /// Only for Get requests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskDto>> GetById(string id)
        {
            GetTaskByIdQuery query = new(Guid.Parse(id));
            var response = await _mediator.Send(query);
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
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetRangeOfTasks([FromQuery]IEnumerable<string> ids)
        {
            GetRangeOfTasksQuery query = new(ids.Select(id => Guid.Parse(id)));
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }
    }
}
