using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Queries
{
    internal class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public GetProjectByIdQueryHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project;
                IQueryable<Project> query = _context.Projects.AsQueryable();
                if (request.Options.IncludeTasks)
                    query = query.IncludeTasks();
                if (request.Options.IncludeEmployees)
                    query = query.IncludeEmployees();
                project = await query.FirstOrDefaultAsync(p => p.Id == request.Options.ProjectId, cancellationToken);
                if (project == null)
                    return Result<ProjectDto>.NotFound($"no such project with id: {request.Options.ProjectId}");
                else
                    return Result.Success(_mapper.Map<ProjectDto>(project));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<ProjectDto>(ex);
            }
        }
    }
}
