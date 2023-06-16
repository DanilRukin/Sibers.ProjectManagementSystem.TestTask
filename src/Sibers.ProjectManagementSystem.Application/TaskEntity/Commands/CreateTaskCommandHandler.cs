using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work = Sibers.ProjectManagementSystem.Domain.TaskEntity.Task;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<TaskDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public CreateTaskCommandHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .IncludeTasks()
                    .IncludeEmployees()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (project == null)
                    return Result<TaskDto>.NotFound($"No project with id: {request.ProjectId}");
                Employee? author = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.AuthorId, cancellationToken);
                if (author == null)
                    return Result<TaskDto>.NotFound($"No employee with id: {request.AuthorId}");
                var task = author.CreateTask(project, request.TaskDto.Name, new Priority(request.TaskDto.Priority), request.TaskDto.Description);
                _context.Projects.Update(project);
                _context.Employees.Update(author);
                //_context.Tasks.Add(task);  // ??
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result<TaskDto>.Success(_mapper.Map<TaskDto>(task));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<TaskDto>(ex);
            }
        }
    }
}
