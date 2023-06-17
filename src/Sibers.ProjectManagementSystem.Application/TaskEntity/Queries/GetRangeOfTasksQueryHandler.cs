using AutoMapper;
using Azure.Core;
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
    internal class GetRangeOfTasksQueryHandler : IRequestHandler<GetRangeOfTasksQuery, Result<IEnumerable<TaskDto>>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public GetRangeOfTasksQueryHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<TaskDto>>> Handle(GetRangeOfTasksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Work> selectedTasks = new List<Work>();
                Work? work;
                foreach (var id in request.TasksIds)
                {
                    work = null;
                    work = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
                    if (work != null)
                        selectedTasks.Add(work);
                }
                return Result.Success(_mapper.Map<List<Work>, IEnumerable<TaskDto>>(selectedTasks));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<IEnumerable<TaskDto>>(ex);
            }
        }
    }
}
