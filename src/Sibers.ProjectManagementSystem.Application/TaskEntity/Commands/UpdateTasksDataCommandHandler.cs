using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity.Commands
{
    public class UpdateTasksDataCommandHandler : IRequestHandler<UpdateTasksDataCommand, Result<TaskDto>>
    {
        private ProjectManagementSystemContext _context;

        public UpdateTasksDataCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<TaskDto>> Handle(UpdateTasksDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.Task.Id, cancellationToken);
                if (task == null)
                    return Result<TaskDto>.NotFound($"No such task with id: {request.Task.Id}");
                task.ChangeName(request.Task.Name);
                task.ChangePriority(new Domain.ProjectAgregate.Priority(request.Task.Priority));
                task.ChangeDescription(request.Task.Description);
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result<TaskDto>.Success(request.Task);
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<TaskDto>(ex);
            }
        }
    }
}
