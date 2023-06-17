using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work = Sibers.ProjectManagementSystem.Domain.TaskEntity.Task;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity.Queries
{
    internal class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public GetTaskByIdQueryHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Work? task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.Taskid, cancellationToken);
                if (task == null)
                {
                    return Result<TaskDto>.NotFound($"Задача с id = '{request.Taskid}' не найдена");
                }
                else
                    return Result.Success(_mapper.Map<TaskDto>(task));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<TaskDto>(ex);
            }
        }
    }
}
