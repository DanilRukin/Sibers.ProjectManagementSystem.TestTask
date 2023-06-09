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
    internal class GetRangeOfProjectsQueryHandler : IRequestHandler<GetRangeOfProjectsQuery, Result<IEnumerable<ProjectDto>>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public GetRangeOfProjectsQueryHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetRangeOfProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project;
                List<Project> selectedProjects = new List<Project>();
                foreach (var option in request.Options)
                {
                    project = null;
                    if (option.IncludeEmployees)
                        project = await _context.Projects
                            .IncludeEmployees()
                            .FirstOrDefaultAsync(p => p.Id == option.ProjectId, cancellationToken);
                    else
                        project = await _context.Projects
                            .FirstOrDefaultAsync(p => p.Id == option.ProjectId, cancellationToken);
                    if (project != null)
                        selectedProjects.Add(project);
                }
                return Result.Success(_mapper.Map<List<Project>, IEnumerable<ProjectDto>>(selectedProjects));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<IEnumerable<ProjectDto>>(ex);
            }
        }
    }
}
