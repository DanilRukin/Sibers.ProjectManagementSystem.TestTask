﻿using AutoMapper;
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
    internal class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<IEnumerable<ProjectDto>>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public GetAllProjectsQueryHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Project> query = _context.Projects.AsQueryable();
                if (request.IncludeTasks)
                    query = query.IncludeTasks();
                if (request.IncludeEmployees)
                    query = query.IncludeEmployees();
                IList<Project> projects = await query.ToListAsync(cancellationToken);
                if (projects == null)
                    return Result.Success(new List<ProjectDto>().AsEnumerable());
                else
                    return Result.Success(_mapper.Map<IList<Project>, IEnumerable<ProjectDto>>(projects));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<IEnumerable<ProjectDto>>(ex);
            }
        }
    }
}
